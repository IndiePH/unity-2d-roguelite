using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Game/Character Profile")]
public class CharacterProfile : ScriptableObject
{
    [SerializeField] string displayName;
    [Tooltip("Hit points this character brings into a run.")]
    [SerializeField] int maxHp = 100;
    [Tooltip("Visual size. BodyCollider and Hurt scale with this.")]
    [SerializeField] float characterScale = 0.055f;
    [Tooltip("Shared rig. The animator on that prefab is the drawing.")]
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] Sprite head;
    [InspectorName("Body")]
    [SerializeField] Sprite bodySprite;
    [SerializeField] Sprite leftArm;
    [SerializeField] Sprite rightArm;
    [SerializeField] Sprite leftHand;
    [SerializeField] Sprite rightHand;
    [SerializeField] Sprite leftLeg;
    [SerializeField] Sprite rightLeg;

    public string DisplayName => displayName;
    public int MaxHp => Mathf.Max(1, maxHp);
    public float CharacterScale => characterScale < 0.01f ? 0.01f : characterScale;
    public GameObject Body => bodyPrefab;

    public void ApplyParts(SpriteRenderer[] parts)
    {
        Apply(parts, "Head", head);
        Apply(parts, "Body", bodySprite);
        Apply(parts, "Left Arm", leftArm);
        Apply(parts, "Right Arm", rightArm);
        Apply(parts, "Left Hand", leftHand);
        Apply(parts, "Right Hand", rightHand);
        Apply(parts, "Left Leg", leftLeg);
        Apply(parts, "Right Leg", rightLeg);
    }

    static void Apply(SpriteRenderer[] parts, string partName, Sprite sprite)
    {
        if (sprite == null || parts == null)
            return;

        for (int i = 0; i < parts.Length; i++)
        {
            SpriteRenderer part = parts[i];
            if (part != null && part.name == partName)
            {
                part.sprite = sprite;
                return;
            }
        }
    }

    void OnValidate()
    {
        if (maxHp < 1)
            maxHp = 1;

        if (characterScale < 0.01f)
            characterScale = 0.01f;
    }
}
