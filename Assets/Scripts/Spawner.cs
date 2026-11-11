using System.Collections.Generic;
using UnityEngine;

public readonly struct SpawnId
{
    public readonly int Value;

    public SpawnId(int value)
    {
        Value = value;
    }
}

public readonly struct Spawned<T> where T : Object
{
    public readonly SpawnId Id;
    public readonly T Instance;

    public Spawned(SpawnId id, T instance)
    {
        Id = id;
        Instance = instance;
    }
}

public class Spawner : MonoBehaviour
{
    readonly Dictionary<int, SpawnRecord> spawned = new Dictionary<int, SpawnRecord>();
    readonly Dictionary<EntityId, Queue<GameObject>> pools = new Dictionary<EntityId, Queue<GameObject>>();
    int nextId = 1;

    public Spawned<T> Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Object owner) where T : Component
    {
        if (prefab == null)
            throw new MissingReferenceException($"{name} needs a prefab to spawn.");

        EntityId prefabId = prefab.gameObject.GetEntityId();
        GameObject instanceObject = Rent(prefabId);
        T instance;
        if (instanceObject == null)
        {
            instance = Instantiate(prefab, position, rotation, transform);
            instanceObject = instance.gameObject;
        }
        else
        {
            Transform instanceTransform = instanceObject.transform;
            instanceTransform.SetParent(transform, false);
            instanceTransform.SetPositionAndRotation(position, rotation);
            instanceTransform.localScale = prefab.transform.localScale;
            instanceObject.SetActive(true);
            instance = instanceObject.GetComponent<T>();
            if (instance == null)
            {
                Store(prefabId, instanceObject);
                throw new MissingReferenceException($"{name} spawned {prefab.name} without {typeof(T).Name}.");
            }
        }

        SpawnId id = new SpawnId(nextId++);
        spawned.Add(id.Value, new SpawnRecord(instanceObject, owner, prefabId));
        return new Spawned<T>(id, instance);
    }

    public void Release(SpawnId id)
    {
        SpawnRecord record;
        if (!spawned.TryGetValue(id.Value, out record))
            throw new MissingReferenceException($"{name} has no spawn {id.Value}.");

        spawned.Remove(id.Value);
        if (record.Instance == null)
            return;

        Store(record.PrefabId, record.Instance);
    }

    public bool TryGet(SpawnId id, out GameObject instance)
    {
        SpawnRecord record;
        if (spawned.TryGetValue(id.Value, out record) && record.Instance != null)
        {
            instance = record.Instance;
            return true;
        }

        instance = null;
        return false;
    }

    public Object GetOwner(SpawnId id)
    {
        SpawnRecord record;
        if (!spawned.TryGetValue(id.Value, out record) || record.Instance == null)
            throw new MissingReferenceException($"{name} has no spawn {id.Value}.");

        return record.Owner;
    }

    GameObject Rent(EntityId prefabId)
    {
        Queue<GameObject> pool;
        if (!pools.TryGetValue(prefabId, out pool))
            return null;

        while (pool.Count > 0)
        {
            GameObject instance = pool.Dequeue();
            if (instance != null)
                return instance;
        }

        return null;
    }

    void Store(EntityId prefabId, GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform, false);
        PoolFor(prefabId).Enqueue(instance);
    }

    Queue<GameObject> PoolFor(EntityId prefabId)
    {
        Queue<GameObject> pool;
        if (!pools.TryGetValue(prefabId, out pool))
        {
            pool = new Queue<GameObject>();
            pools.Add(prefabId, pool);
        }

        return pool;
    }

    struct SpawnRecord
    {
        public readonly GameObject Instance;
        public readonly Object Owner;
        public readonly EntityId PrefabId;

        public SpawnRecord(GameObject instance, Object owner, EntityId prefabId)
        {
            Instance = instance;
            Owner = owner;
            PrefabId = prefabId;
        }
    }
}
