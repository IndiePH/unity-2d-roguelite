using System;
using NUnit.Framework;
using UnityEngine;

public class SpawnerPoolTests
{
    GameObject spawnerObject;
    Spawner spawner;
    GameObject prefabObject;
    SpawnerPoolMarker prefab;
    GameObject ownerObject;

    [SetUp]
    public void SetUp()
    {
        spawnerObject = new GameObject("spawner");
        spawner = spawnerObject.AddComponent<Spawner>();
        prefabObject = new GameObject("prefab");
        prefab = prefabObject.AddComponent<SpawnerPoolMarker>();
        ownerObject = new GameObject("owner");
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(spawnerObject);
        UnityEngine.Object.DestroyImmediate(prefabObject);
        UnityEngine.Object.DestroyImmediate(ownerObject);
    }

    [Test]
    public void SpawnParentsTheInstanceAndKeepsTheId()
    {
        Spawned<SpawnerPoolMarker> spawned = spawner.Spawn(
            prefab,
            new Vector3(1f, 2f, 0f),
            Quaternion.identity,
            ownerObject);

        GameObject instance;
        Assert.IsTrue(spawned.Instance.gameObject.activeSelf);
        Assert.AreSame(spawnerObject.transform, spawned.Instance.transform.parent);
        Assert.AreEqual(new Vector3(1f, 2f, 0f), spawned.Instance.transform.position);
        Assert.IsTrue(spawner.TryGet(spawned.Id, out instance));
        Assert.AreSame(spawned.Instance.gameObject, instance);
        Assert.AreSame(ownerObject, spawner.GetOwner(spawned.Id));
    }

    [Test]
    public void ReleaseReturnsTheInstanceForTheNextRent()
    {
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        GameObject owner = new GameObject("next-owner");
        try
        {
            spawner.Release(first.Id);

            Assert.IsFalse(first.Instance.gameObject.activeSelf);
            Assert.AreSame(spawnerObject.transform, first.Instance.transform.parent);
            GameObject released;
            Assert.IsFalse(spawner.TryGet(first.Id, out released));
            Assert.Throws<MissingReferenceException>(() => spawner.GetOwner(first.Id));

            Quaternion rotation = Quaternion.Euler(0f, 0f, 25f);
            Spawned<SpawnerPoolMarker> again = spawner.Spawn(
                prefab,
                new Vector3(3f, 4f, 0f),
                rotation,
                owner);

            Assert.AreSame(first.Instance, again.Instance);
            Assert.AreNotEqual(first.Id.Value, again.Id.Value);
            Assert.IsTrue(again.Instance.gameObject.activeSelf);
            Assert.AreEqual(new Vector3(3f, 4f, 0f), again.Instance.transform.position);
            Assert.AreEqual(0f, Quaternion.Angle(rotation, again.Instance.transform.rotation), 0.001f);
            Assert.AreSame(owner, spawner.GetOwner(again.Id));
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(owner);
        }
    }

    [Test]
    public void ReuseRunsOnEnableAgainAndDoesNotRunAwake()
    {
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        Assert.AreEqual(1, first.Instance.AwakeCount);
        Assert.AreEqual(1, first.Instance.EnableCount);
        spawner.Release(first.Id);

        Spawned<SpawnerPoolMarker> again = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);

        Assert.AreSame(first.Instance, again.Instance);
        Assert.AreEqual(1, again.Instance.AwakeCount);
        Assert.AreEqual(2, again.Instance.EnableCount);
    }

    [Test]
    public void ReuseRestoresThePrefabScale()
    {
        prefabObject.transform.localScale = new Vector3(2f, 3f, 1f);
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        first.Instance.transform.localScale = new Vector3(9f, 9f, 9f);
        spawner.Release(first.Id);

        Spawned<SpawnerPoolMarker> again = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);

        Assert.AreEqual(new Vector3(2f, 3f, 1f), again.Instance.transform.localScale);
    }

    [Test]
    public void NextRentIsTheOldestStoredInstance()
    {
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        Spawned<SpawnerPoolMarker> second = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        spawner.Release(first.Id);
        spawner.Release(second.Id);

        Spawned<SpawnerPoolMarker> again = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);

        Assert.AreSame(first.Instance, again.Instance);
        Assert.IsFalse(second.Instance.gameObject.activeSelf);
        spawner.Release(again.Id);

        Spawned<SpawnerPoolMarker> waiting = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);

        Assert.AreSame(second.Instance, waiting.Instance);
    }

    [Test]
    public void TwoLiveRentsAreDifferentInstances()
    {
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        Spawned<SpawnerPoolMarker> second = spawner.Spawn(
            prefab,
            new Vector3(1f, 0f, 0f),
            Quaternion.identity,
            ownerObject);

        Assert.AreNotSame(first.Instance, second.Instance);
        spawner.Release(first.Id);
        Assert.IsTrue(second.Instance.gameObject.activeSelf);
        Assert.IsTrue(spawner.TryGet(second.Id, out _));
    }

    [Test]
    public void ADifferentPrefabDoesNotReuseTheInstance()
    {
        GameObject otherObject = new GameObject("other");
        try
        {
            SpawnerPoolMarker other = otherObject.AddComponent<SpawnerPoolMarker>();
            Spawned<SpawnerPoolMarker> first = spawner.Spawn(
                prefab,
                Vector3.zero,
                Quaternion.identity,
                ownerObject);
            spawner.Release(first.Id);

            Spawned<SpawnerPoolMarker> spawned = spawner.Spawn(
                other,
                Vector3.zero,
                Quaternion.identity,
                ownerObject);

            Assert.AreNotSame(first.Instance, spawned.Instance);
            Assert.IsFalse(first.Instance.gameObject.activeSelf);
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(otherObject);
        }
    }

    [Test]
    public void ADestroyedReturnIsNotRented()
    {
        Spawned<SpawnerPoolMarker> first = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        GameObject dead = first.Instance.gameObject;
        spawner.Release(first.Id);
        UnityEngine.Object.DestroyImmediate(dead);

        Spawned<SpawnerPoolMarker> again = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);

        Assert.IsNotNull(again.Instance);
        Assert.AreNotSame(dead, again.Instance.gameObject);
    }

    [Test]
    public void ReleaseOfAnUnknownIdThrows()
    {
        Spawned<SpawnerPoolMarker> spawned = spawner.Spawn(
            prefab,
            Vector3.zero,
            Quaternion.identity,
            ownerObject);
        spawner.Release(spawned.Id);

        Assert.Throws<MissingReferenceException>(() => spawner.Release(spawned.Id));
        Assert.Throws<MissingReferenceException>(() => spawner.Release(new SpawnId(99)));
    }

    [Test]
    public void SpawnWithoutAPrefabThrows()
    {
        Assert.Throws<MissingReferenceException>(() => spawner.Spawn<SpawnerPoolMarker>(
            null,
            Vector3.zero,
            Quaternion.identity,
            ownerObject));
    }
}

[ExecuteAlways]
public class SpawnerPoolMarker : MonoBehaviour
{
    [System.NonSerialized] public int AwakeCount;
    [System.NonSerialized] public int EnableCount;

    void Awake()
    {
        AwakeCount++;
    }

    void OnEnable()
    {
        EnableCount++;
    }
}
