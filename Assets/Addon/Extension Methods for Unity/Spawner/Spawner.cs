using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles spawning and despawning objects
/// </summary>
public class Spawner : MonoSingletonExt<Spawner>
{
    #region Variables

    /// <summary>
    /// List of pools
    /// </summary>
    private static readonly Dictionary<string, SpawnerPool> PoolList = new Dictionary<string, SpawnerPool>();

    // Variables
    #endregion

    #region CreateSpawn

    /// <summary>
    /// Creates one spawn _instance
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="poolName">name of the spawner pool to load from</param>
    /// <returns></returns>
    public static GameObject CreateSpawn(GameObject go, Vector3 position, Quaternion rotation, string poolName)
    {
        // see if we have the pool already
        SpawnerPool pool;
        if (PoolList.ContainsKey(poolName))
            pool = PoolList[poolName];

        // get the breed pool for this _instance
        else
        {
            pool = SpawnerBreeder.Get(go.name) ?? SpawnerBreeder.Create(poolName, go);
            PoolList.Add(poolName, pool);
        }

        // spawn the NPC
        return pool.Spawn(position, rotation);
    }

    /// <summary>
    /// Creates one spawn _instance
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static GameObject CreateSpawn(GameObject go, Vector3 position, Quaternion rotation)
    {
        return CreateSpawn(go, position, rotation, go.name);
    }

    /// <summary>
    /// Creates one spawn _instance
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="poolName">name of the spawner pool to use</param>
    /// <param name="spawnedAction">action to call when the object is spawned. The new spawned object is passed as the parameter</param>
    /// <returns></returns>
    public static GameObject CreateSpawn(GameObject go, Vector3 position, Quaternion rotation, string poolName, Action<GameObject> spawnedAction)
    {
        // see if we have the pool already
        SpawnerPool pool;
        if (PoolList.ContainsKey(poolName))
            pool = PoolList[poolName];

        // get the breed pool for this _instance
        else
        {
            pool = SpawnerBreeder.Get(go.name) ?? SpawnerBreeder.Create(poolName, go);
            PoolList.Add(poolName, pool);
        }

        // spawn the NPC
        return pool.Spawn(position, rotation, spawnedAction);
    }

    /// <summary>
    /// Creates one spawn _instance
    /// </summary>
    /// <param name="go"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="spawnedAction">action to call when the object is spawned. The new spawned object is passed as the parameter</param>
    /// <returns></returns>
    public static GameObject CreateSpawn(GameObject go, Vector3 position, Quaternion rotation, Action<GameObject> spawnedAction)
    {
        return CreateSpawn(go, position, rotation, go.name, spawnedAction);
    }

    // CreateSpawn
    #endregion

    #region Despawn

    /// <summary>
    /// Despawns and object
    /// </summary>
    /// <param name="go"></param>
    public static void Despawn(GameObject go)
    {
        // remove the number
        string goName = go.name.Substring(0, go.name.LastIndexOf('(') - 1);

        // see if we have the breed pool already
        if (PoolList.ContainsKey(goName))
            PoolList[goName].Despawn(go);
        else
            Destroy(go);
    }

    // Despawn
    #endregion

    #region Prespawn

    /// <summary>
    /// Spawn objects, unspawn them, then exit
    /// Useful for preloading objects, so they are ready to use
    /// </summary>
    /// <param name="go">game object to spawn</param>
    /// <param name="count">numbe of spawns to create</param>
    public static void Prespawn(GameObject go, int count)
    {
        string name = go.name;

        // see if we have the breed pool already
        SpawnerPool pool;
        if (PoolList.ContainsKey(name))
            pool = PoolList[name];

        // get the pool for this _instance
        else
        {
            pool = SpawnerBreeder.Get(name) ?? SpawnerBreeder.Create(name, go);
            PoolList.Add(name, pool);
        }

        // prespawn
        pool.Prespawn(count);
    }

    // Prespawn
    #endregion
}
