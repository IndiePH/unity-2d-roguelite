using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSpriteAnimation : MonoBehaviour
{
    enum Preview
    {
        Auto,
        Idle,
        Move,
        Dash,
        Hit,
        Attack,
        Death
    }

    [SerializeField] Character character;
    [SerializeField] CharacterMovement movement;
    [SerializeField] CharacterDash dashMotion;
    [SerializeField] SpriteRenderer spriteRenderer;
    [Tooltip("Auto follows idle and move. Any other value locks that clip so the frames can be checked.")]
    [SerializeField] Preview preview;
    [SerializeField] CharacterProfile profile;
    [Tooltip("Hide the sprite when the death clip ends. Off leaves the last frame visible.")]
    [SerializeField] bool hideSpriteOnDeath = true;
    [Tooltip("Stop walking and dashing while the hit clip plays.")]
    [SerializeField] bool stopMovementOnHit = true;
    [Tooltip("Seconds of invulnerability after the hurt clip ends. Another hit is ignored, and the sprite blinks.")]
    [SerializeField] float hitInvulnerableDuration = 0.5f;
    [Tooltip("Seconds between showing and hiding the sprite while hurt.")]
    [SerializeField] float blinkInterval = 0.08f;

    int frame;
    float timer;
    Preview showing;
    bool playingHit;
    bool playingDeath;
    Color authoredColor;
    float blinkUntil;

    void OnEnable()
    {
        if (character == null)
            throw new MissingReferenceException($"{name} needs a Character assigned.");

        if (movement == null)
            throw new MissingReferenceException($"{name} needs CharacterMovement assigned.");

        if (dashMotion == null)
            throw new MissingReferenceException($"{name} needs CharacterDash assigned.");

        if (spriteRenderer == null)
            throw new MissingReferenceException($"{name} needs a SpriteRenderer assigned.");

        if (profile == null)
            throw new MissingReferenceException($"{name} needs a CharacterProfile assigned.");

        if (profile.Sprites == null)
            throw new MissingReferenceException($"{name} needs a sprite sheet on {profile.name}.");

        authoredColor = spriteRenderer.color;
    }

    void OnDisable()
    {
        if (character != null && !character.IsDead)
            character.SetHoldMovement(false);

        if (spriteRenderer != null)
            spriteRenderer.color = authoredColor;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.lKey.wasPressedThisFrame)
            BeginDeath();

        if (character.IsDead)
        {
            if (!playingDeath)
                return;

            if (FinishedOnce(Clip(Preview.Death)))
            {
                playingDeath = false;
                if (hideSpriteOnDeath)
                    spriteRenderer.enabled = false;
            }

            return;
        }

        if (keyboard != null && keyboard.pKey.wasPressedThisFrame && !playingHit && !character.IsInvulnerable)
            BeginHit();

        if (playingHit)
        {
            bool finished = FinishedOnce(Clip(Preview.Hit));
            if (!finished)
            {
                ApplyFlip();
                UpdateBlink();
                return;
            }

            playingHit = false;
            if (stopMovementOnHit)
                character.SetHoldMovement(false);

            character.StartInvulnerability(hitInvulnerableDuration);
            blinkUntil = Time.time + hitInvulnerableDuration;
            showing = Preview.Hit;
        }

        Preview clip = preview == Preview.Auto ? AutoClip() : preview;

        if (clip != showing)
        {
            frame = 0;
            timer = 0f;
            showing = clip;
        }

        SpriteClip spriteClip = Clip(clip);
        if (!spriteClip.HasFrames)
            return;

        Sprite[] frames = spriteClip.frames;
        float duration = spriteClip.SafeDuration;

        timer += Time.deltaTime;
        while (timer >= duration)
        {
            timer -= duration;
            frame = (frame + 1) % frames.Length;
        }

        Sprite sprite = frames[frame];
        if (sprite != null)
            spriteRenderer.sprite = sprite;

        ApplyFlip();
        UpdateBlink();
    }

    void BeginDeath()
    {
        SpriteClip clip = Clip(Preview.Death);
        if (character.IsDead || !clip.HasFrames)
            return;

        character.Die();
        playingHit = false;
        playingDeath = true;
        frame = 0;
        timer = 0f;
        blinkUntil = 0f;
        spriteRenderer.color = authoredColor;
        if (clip.frames[0] != null)
            spriteRenderer.sprite = clip.frames[0];
    }

    void BeginHit()
    {
        SpriteClip clip = Clip(Preview.Hit);
        if (character.IsDead || !clip.HasFrames)
            return;

        playingHit = true;
        frame = 0;
        timer = 0f;
        blinkUntil = 0f;
        if (stopMovementOnHit)
            character.SetHoldMovement(true);

        if (clip.frames[0] != null)
            spriteRenderer.sprite = clip.frames[0];

        character.Hit();
    }

    void UpdateBlink()
    {
        if (Time.time < blinkUntil)
        {
            float interval = blinkInterval < 0.0001f ? 0.0001f : blinkInterval;
            bool visible = Mathf.FloorToInt(Time.time / interval) % 2 == 0;
            Color color = authoredColor;
            color.a = visible ? authoredColor.a : 0f;
            spriteRenderer.color = color;
            return;
        }

        if (spriteRenderer.color != authoredColor)
            spriteRenderer.color = authoredColor;
    }

    bool FinishedOnce(SpriteClip clip)
    {
        if (!clip.HasFrames)
            return true;

        Sprite[] frames = clip.frames;
        float duration = clip.SafeDuration;

        timer += Time.deltaTime;
        if (timer < duration)
            return false;

        timer -= duration;
        frame++;
        if (frame >= frames.Length)
            return true;

        Sprite sprite = frames[frame];
        if (sprite != null)
            spriteRenderer.sprite = sprite;

        return false;
    }

    void ApplyFlip()
    {
        float horizontal = movement.CurrentInput.x;
        if (horizontal < 0f)
            spriteRenderer.flipX = true;
        else if (horizontal > 0f)
            spriteRenderer.flipX = false;
    }

    Preview AutoClip()
    {
        if (dashMotion.IsImpulse)
            return Clip(Preview.Dash).HasFrames ? Preview.Dash : Preview.Move;

        return movement.IsMoving ? Preview.Move : Preview.Idle;
    }

    SpriteClip Clip(Preview clip)
    {
        CharacterSpriteSheet sheet = profile.Sprites;
        switch (clip)
        {
            case Preview.Move: return sheet.Move;
            case Preview.Dash: return sheet.Dash;
            case Preview.Hit: return sheet.Hit;
            case Preview.Attack: return sheet.Attack;
            case Preview.Death: return sheet.Death;
            default: return sheet.Idle;
        }
    }
}
