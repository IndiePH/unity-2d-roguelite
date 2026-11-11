using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool Occupied { get; private set; }

    // isActiveAndEnabled stays false through Awake, so a spawn in Awake would see every point as closed.
    public bool IsAvailable => gameObject.activeInHierarchy && enabled && !Occupied;

    public void Occupy()
    {
        if (Occupied)
            throw new System.InvalidOperationException($"{name} already has a character.");

        Occupied = true;
    }
}
