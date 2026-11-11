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
    int nextId = 1;

    public Spawned<T> Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Object owner) where T : Component
    {
        if (prefab == null)
            throw new MissingReferenceException($"{name} needs a prefab to spawn.");

        T instance = Instantiate(prefab, position, rotation);
        SpawnId id = new SpawnId(nextId++);
        spawned.Add(id.Value, new SpawnRecord(instance.gameObject, owner));
        return new Spawned<T>(id, instance);
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

    struct SpawnRecord
    {
        public readonly GameObject Instance;
        public readonly Object Owner;

        public SpawnRecord(GameObject instance, Object owner)
        {
            Instance = instance;
            Owner = owner;
        }
    }
}
