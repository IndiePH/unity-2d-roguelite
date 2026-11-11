using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class CharacterProfileTests
{
    const string Character100 = "Assets/Settings/Characters/Character100.asset";
    const string Character70 = "Assets/Settings/Characters/Character70.asset";

    [Test]
    public void TheTwoCharactersHaveDifferentHitPoints()
    {
        CharacterProfile sturdy = AssetDatabase.LoadAssetAtPath<CharacterProfile>(Character100);
        CharacterProfile light = AssetDatabase.LoadAssetAtPath<CharacterProfile>(Character70);

        Assert.AreEqual(100, sturdy.MaxHp);
        Assert.AreEqual(70, light.MaxHp);
        Assert.AreNotSame(sturdy.Sprites, light.Sprites);
        Assert.IsNotNull(sturdy.Sprites);
        Assert.IsNotNull(light.Sprites);
    }

    [Test]
    public void MaxHpStaysAtLeastOne()
    {
        CharacterProfile profile = ScriptableObject.CreateInstance<CharacterProfile>();
        typeof(CharacterProfile)
            .GetField("maxHp", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(profile, 0);

        Assert.AreEqual(1, profile.MaxHp);

        Object.DestroyImmediate(profile);
    }
}
