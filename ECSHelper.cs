using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Ray = UnityEngine.Ray;

public class EntitySpawnHelper : MonoBehaviour
{
    public void OnDestroy()
    {
        EntityHelper.Dispose();
    }
}

public static class EntityHelper
{
    private static readonly Dictionary<int, Entity> EntityCache = new Dictionary<int, Entity>();

    private static BlobAssetStore _blobAsset;

    public static Entity Instantiate(GameObject prefab)
    {
        if(_blobAsset == null)
            _blobAsset = new BlobAssetStore();

//        Debug.Log("EntityCache Count: " +(EntityCache.Count));
        var key = prefab.GetHashCode();
        if (EntityCache.ContainsKey(key))
        {
            return World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate((EntityCache[key]));
        }
        else
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAsset);
            var e = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);
            EntityCache.Add(key, e);
            return World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate((EntityCache[key]));
        }
    }

    public static void Destroy(this Entity e)
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(e);
    }

    public static Entity GetComponent<T>(this Entity e, out T component) where T : struct, IComponentData
    {
        component = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(e);
        return e;
    }

    public static Entity SetComponent<T>(this Entity e, T component) where T : struct, IComponentData
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(e, component);
        return e;
    }

    public static Entity AddComponent<T>(this Entity e, T component) where T : struct, IComponentData
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent(e, component.GetType());

        return e.SetComponent(component);
    }

    public static bool TryGetComponent<T>(this Entity e, out T component) where T : struct, IComponentData
    {
        component = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<T>(e);
        return World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<T>(e);
    }

    public static bool TryGetSharedComponent<T>(this Entity e, out T component) where T : struct, ISharedComponentData
    {
        try
        {
            component = World.DefaultGameObjectInjectionWorld.EntityManager.GetSharedComponentData<T>(e);
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            component = default;
            return false;
        }
    }
    
    public static Entity SetSharedComponent<T>(this Entity e, T component) where T : struct, ISharedComponentData
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.SetSharedComponentData(e, component);
        return e;
    }

    public static Entity GetAllChildren(this Entity e, out List<Entity> children)
    {
        children = new List<Entity>();
        try
        {
            var childFromEntity = World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<Child>(e).ToList();

            for (int i = 0; i < childFromEntity.Count; i++)
            {
                children.Add(childFromEntity[i].Value);
            }
        }
        catch{ /* Entity doesnt have any children */}

        return e;
    }

    public static void Dispose()
    {
        EntityCache.Clear();
        _blobAsset.Dispose();
    }
}