using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class CharacterProfileTests
{
    const string BloodAlchemist = "Assets/Settings/Characters/BloodAlchemist.asset";
    const string Warrior = "Assets/Settings/Characters/Warrior.asset";

    [Test]
    public void TheTwoCharactersHaveDifferentHitPoints()
    {
        CharacterProfile sturdy = AssetDatabase.LoadAssetAtPath<CharacterProfile>(BloodAlchemist);
        CharacterProfile light = AssetDatabase.LoadAssetAtPath<CharacterProfile>(Warrior);

        Assert.AreEqual(100, sturdy.MaxHp);
        Assert.AreEqual(70, light.MaxHp);
        Assert.AreEqual(0.2f, sturdy.CharacterScale, 0.0001f);
        Assert.AreEqual(0.2f, light.CharacterScale, 0.0001f);

        GameObject body = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Art/Characters/Character.prefab");
        Assert.AreSame(body, sturdy.Body);
        Assert.AreSame(body, light.Body);
    }

    [Test]
    public void AssignedSpriteReplacesTheMatchingPart()
    {
        CharacterProfile profile = ScriptableObject.CreateInstance<CharacterProfile>();
        Texture2D texture = new Texture2D(1, 1);
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), Vector2.zero);
        typeof(CharacterProfile)
            .GetField("head", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(profile, sprite);

        SpriteRenderer head = new GameObject("Head").AddComponent<SpriteRenderer>();
        SpriteRenderer bodyPart = new GameObject("Body").AddComponent<SpriteRenderer>();
        profile.ApplyParts(new[] { bodyPart, head });

        Assert.AreSame(sprite, head.sprite);
        Assert.IsNull(bodyPart.sprite);

        Object.DestroyImmediate(head.gameObject);
        Object.DestroyImmediate(bodyPart.gameObject);
        Object.DestroyImmediate(sprite);
        Object.DestroyImmediate(texture);
        Object.DestroyImmediate(profile);
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

    [Test]
    public void CharacterScaleStaysVisible()
    {
        CharacterProfile profile = ScriptableObject.CreateInstance<CharacterProfile>();
        typeof(CharacterProfile)
            .GetField("characterScale", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(profile, 0f);

        Assert.AreEqual(0.01f, profile.CharacterScale);

        Object.DestroyImmediate(profile);
    }
}
