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
    [Tooltip("Idle frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] idle;
    [Tooltip("Move frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] move;
    [Tooltip("Dash frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] dash;
    [Tooltip("Hit frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] hit;
    [Tooltip("Attack frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] attack;
    [Tooltip("Death frames from the sliced spritesheet, in playback order.")]
    [SerializeField] Sprite[] death;
    [Tooltip("Seconds each idle frame stays on screen.")]
    [SerializeField] float idleFrameDuration = 0.16f;
    [Tooltip("Seconds each move frame stays on screen.")]
    [SerializeField] float moveFrameDuration = 0.08f;
    [Tooltip("Seconds each dash frame stays on screen.")]
    [SerializeField] float dashFrameDuration = 0.06f;
    [Tooltip("Seconds each hit frame stays on screen.")]
    [SerializeField] float hitFrameDuration = 0.08f;
    [Tooltip("Seconds each attack frame stays on screen.")]
    [SerializeField] float attackFrameDuration = 0.08f;
    [Tooltip("Seconds each death frame stays on screen.")]
    [SerializeField] float deathFrameDuration = 0.08f;
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

            if (FinishedOnce(death, deathFrameDuration))
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
            bool finished = FinishedOnce(hit, hitFrameDuration);
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

        Sprite[] frames = Frames(clip);
        if (frames == null || frames.Length == 0)
            return;

        float duration = Duration(clip);
        if (duration < 0.0001f)
            duration = 0.0001f;

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
        if (character.IsDead || death == null || death.Length == 0)
            return;

        character.Die();
        playingHit = false;
        playingDeath = true;
        frame = 0;
        timer = 0f;
        blinkUntil = 0f;
        spriteRenderer.color = authoredColor;
        if (death[0] != null)
            spriteRenderer.sprite = death[0];
    }

    void BeginHit()
    {
        if (character.IsDead || hit == null || hit.Length == 0)
            return;

        playingHit = true;
        frame = 0;
        timer = 0f;
        blinkUntil = 0f;
        if (stopMovementOnHit)
            character.SetHoldMovement(true);

        if (hit[0] != null)
            spriteRenderer.sprite = hit[0];

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

    bool FinishedOnce(Sprite[] frames, float duration)
    {
        if (frames == null || frames.Length == 0)
            return true;

        if (duration < 0.0001f)
            duration = 0.0001f;

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
            return dash != null && dash.Length > 0 ? Preview.Dash : Preview.Move;

        return movement.IsMoving ? Preview.Move : Preview.Idle;
    }

    Sprite[] Frames(Preview clip)
    {
        switch (clip)
        {
            case Preview.Move: return move;
            case Preview.Dash: return dash;
            case Preview.Hit: return hit;
            case Preview.Attack: return attack;
            case Preview.Death: return death;
            default: return idle;
        }
    }

    float Duration(Preview clip)
    {
        switch (clip)
        {
            case Preview.Move: return moveFrameDuration;
            case Preview.Dash: return dashFrameDuration;
            case Preview.Hit: return hitFrameDuration;
            case Preview.Attack: return attackFrameDuration;
            case Preview.Death: return deathFrameDuration;
            default: return idleFrameDuration;
        }
    }
}
