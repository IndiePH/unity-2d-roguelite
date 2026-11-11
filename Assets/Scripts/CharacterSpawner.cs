using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    [SerializeField] Character characterPrefab;
    [SerializeField] SpawnPoint[] spawnPoints;
    [SerializeField] CameraFollow cameraFollow;

    void Awake()
    {
        if (cameraFollow == null)
            throw new MissingReferenceException($"{name} needs a camera follow assigned.");

        Character character = SpawnCharacter();
        cameraFollow.Follow(character.transform);
    }

    public Character SpawnCharacter()
    {
        if (spawner == null)
            throw new MissingReferenceException($"{name} needs a spawner assigned.");

        if (characterPrefab == null)
            throw new MissingReferenceException($"{name} needs a character prefab assigned.");

        if (spawnPoints == null || spawnPoints.Length == 0)
            throw new MissingReferenceException($"{name} needs spawn points assigned.");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null)
                throw new MissingReferenceException($"{name} needs every spawn point assigned.");
        }

        int index = ChoosePoint();
        SpawnPoint point = spawnPoints[index];
        point.Occupy();
        Spawned<Character> spawned = spawner.Spawn(
            characterPrefab,
            point.transform.position,
            point.transform.rotation,
            this);
        return spawned.Instance;
    }

    int ChoosePoint()
    {
        bool[] available = new bool[spawnPoints.Length];
        int open = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            available[i] = spawnPoints[i].IsAvailable;
            if (available[i])
                open++;
        }

        if (open == 0)
            throw new System.InvalidOperationException($"{name} has no open spawn point.");

        int index = OpenSpawnPoint.Index(available, Random.Range(0, open));
        if (index < 0)
            throw new System.InvalidOperationException($"{name} has no open spawn point.");

        return index;
    }
}
