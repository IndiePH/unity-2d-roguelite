using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Game/Character Profile")]
public class CharacterProfile : ScriptableObject
{
    [SerializeField] string displayName;
    [Tooltip("Hit points this character brings into a run.")]
    [SerializeField] int maxHp = 100;
    [SerializeField] CharacterSpriteSheet sprites;

    public string DisplayName => displayName;
    public int MaxHp => Mathf.Max(1, maxHp);
    public CharacterSpriteSheet Sprites => sprites;

    void OnValidate()
    {
        if (maxHp < 1)
            maxHp = 1;
    }
}
