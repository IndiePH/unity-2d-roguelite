using System;
using NUnit.Framework;
using UnityEngine;

public class SpawnPointTests
{
    [Test]
    public void OccupyTakesThePoint()
    {
        GameObject pointObject = new GameObject("point");
        try
        {
            SpawnPoint point = pointObject.AddComponent<SpawnPoint>();

            Assert.IsTrue(point.IsAvailable);
            point.Occupy();
            Assert.IsFalse(point.IsAvailable);
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(pointObject);
        }
    }

    [Test]
    public void SecondOccupyThrows()
    {
        GameObject pointObject = new GameObject("point");
        try
        {
            SpawnPoint point = pointObject.AddComponent<SpawnPoint>();
            point.Occupy();

            Assert.Throws<InvalidOperationException>(() => point.Occupy());
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(pointObject);
        }
    }

    [Test]
    public void InactivePointIsUnavailable()
    {
        GameObject pointObject = new GameObject("point");
        try
        {
            SpawnPoint point = pointObject.AddComponent<SpawnPoint>();
            pointObject.SetActive(false);

            Assert.IsFalse(point.IsAvailable);
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(pointObject);
        }
    }

    [Test]
    public void DisabledPointIsUnavailable()
    {
        GameObject pointObject = new GameObject("point");
        try
        {
            SpawnPoint point = pointObject.AddComponent<SpawnPoint>();
            point.enabled = false;

            Assert.IsFalse(point.IsAvailable);
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(pointObject);
        }
    }
}
