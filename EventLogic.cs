using System.Reflection;

/*
public class ExampleUsage
{
    public void ExampleInvoke()
    {
        new ExampleEvent() { paramA = 1, paramB = 13 }.Invoke();
    }

    [OnEvent]
    public void OnExampleEvent(ExampleEvent @event)
    {

    }

    public class ExampleEvent : Event<ExampleEvent>
    {
        public int paramA, paramB;
    }
}
*/
public abstract class Event<T> where T : Event<T>
{
    private static readonly SortedDictionary<int, Action<T>> Actions = new SortedDictionary<int, Action<T>>();

    public T Invoke()
    {
        foreach (var methodInfo in Actions)
            methodInfo.Value.Invoke((T)this);

        return (T)this;
    }
    
    protected static void AddListener(MethodInfo method, int order = 0)
    {
        try
        {
            var act = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), method);
            if (Actions.ContainsKey(order))
                Actions[order] += act;
            else
                Actions.Add(order, act);
            
        }
        catch (Exception e)
        { throw;
        }
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnEventAttribute : Attribute
{
    public int Order { get; set; }

    static OnEventAttribute()
    {
        Solve();
    }

    /// <summary>
    /// Attribute that automatically turns method into an event
    /// target method has to contain only one parameter - event it targets
    /// </summary>
    public OnEventAttribute()
    {

    }
    
    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
        while (toCheck != null && toCheck != typeof(object)) {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur) {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    public static void Solve()
    {
        var q = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass select t;
        q.ToList().ForEach(Solve);
    }
    private static void Solve(Type t)
    {
        var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var method in methods)
        {
            var att = (OnEventAttribute) GetCustomAttribute(method, typeof (OnEventAttribute));
            if (att == null) continue;

            var parameters = method.GetParameters();

            if (parameters.Length != 1) 
                throw new ArgumentException("Invalid amount of parameters, use only Event<T>");
            if (!IsSubclassOfRawGeneric(typeof(Event<>), parameters[0].ParameterType))
                throw new ArgumentException("Invalid parameter type - is not subclass of Event<T>");

            parameters[0].ParameterType.GetMethod("AddListener", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Invoke(null, new object[] {method, att.Order});
        }
    }
}
public class InvalidEventTypeException : Exception
{
    
}