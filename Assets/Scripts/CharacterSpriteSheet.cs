using System;
using UnityEngine;

[Serializable]
public struct SpriteClip
{
    [Tooltip("Frames from the sliced spritesheet, in playback order.")]
    public Sprite[] frames;

    [Tooltip("Seconds each frame stays on screen.")]
    public float frameDuration;

    public bool HasFrames => frames != null && frames.Length > 0;

    public float SafeDuration => frameDuration < 0.0001f ? 0.0001f : frameDuration;
}

[CreateAssetMenu(fileName = "CharacterSpriteSheet", menuName = "Game/Character Sprite Sheet")]
public class CharacterSpriteSheet : ScriptableObject
{
    [SerializeField] SpriteClip idle;
    [SerializeField] SpriteClip move;
    [SerializeField] SpriteClip dash;
    [SerializeField] SpriteClip hit;
    [SerializeField] SpriteClip attack;
    [SerializeField] SpriteClip death;

    public SpriteClip Idle => idle;
    public SpriteClip Move => move;
    public SpriteClip Dash => dash;
    public SpriteClip Hit => hit;
    public SpriteClip Attack => attack;
    public SpriteClip Death => death;
}
