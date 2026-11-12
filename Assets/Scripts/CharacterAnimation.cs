using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimation : MonoBehaviour
{
    public const string Idle = "Idle";
    public const string Running = "Running";
    public const string Hurt = "Hurt";
    public const string Dying = "Dying";
    public const string Sliding = "Sliding";

    [SerializeField] Character character;
    [SerializeField] CharacterMovement movement;
    [SerializeField] CharacterDash dashMotion;
    [SerializeField] Animator animator;
    [SerializeField] Transform visual;
    [SerializeField] Transform bodyCollider;
    [SerializeField] Transform hurtCollider;
    [Tooltip("Profile scale the collider sizes on this prefab were fitted to.")]
    [SerializeField] float colliderFitScale = 0.055f;
    [Tooltip("Unscaled idle point that sits on the body. X is the center, Y is the lowest point.")]
    [SerializeField] Vector2 artAnchor = new Vector2(-0.23f, -0.31f);
    [Tooltip("Where that point sits on the collider, in the body's local space.")]
    [SerializeField] Vector2 bodyAnchor = new Vector2(-0.01f, -0.155f);
    [SerializeField] SpriteRenderer[] parts;
    [Tooltip("Hide the body parts when the death clip ends. Off leaves the last pose visible.")]
    [SerializeField] bool hideSpriteOnDeath = true;
    [Tooltip("Color of every body part once this character dies.")]
    [SerializeField] Color deathColor = new Color(243f / 255f, 243f / 255f, 243f / 255f, 1f);
    [Tooltip("Stop walking and dashing while the hurt clip plays.")]
    [SerializeField] bool stopMovementOnHit = true;
    [Tooltip("Seconds of invulnerability after the hurt clip ends. Another hit is ignored, and the parts blink.")]
    [SerializeField] float hitInvulnerableDuration = 1.25f;
    [Tooltip("Seconds between showing and hiding the parts while hurt.")]
    [SerializeField] float blinkInterval = 0.15f;

    int facing = 1;

    public int Facing => facing;
    string showing;
    bool playingHit;
    bool playingDeath;
    bool hidden;
    Color[] authoredColors;
    float blinkUntil;

    public static string State(bool dead, bool hurting, bool dashing, bool moving)
    {
        if (dead)
            return Dying;

        if (hurting)
            return Hurt;

        if (dashing)
            return Sliding;

        return moving ? Running : Idle;
    }

    void OnEnable()
    {
        if (character == null)
            throw new MissingReferenceException($"{name} needs a Character assigned.");

        if (movement == null)
            throw new MissingReferenceException($"{name} needs CharacterMovement assigned.");

        if (dashMotion == null)
            throw new MissingReferenceException($"{name} needs CharacterDash assigned.");

        if (animator == null)
            throw new MissingReferenceException($"{name} needs an Animator assigned.");

        if (visual == null)
            throw new MissingReferenceException($"{name} needs the visual assigned.");

        if (bodyCollider == null)
            throw new MissingReferenceException($"{name} needs the body collider assigned.");

        if (hurtCollider == null)
            throw new MissingReferenceException($"{name} needs the hurt collider assigned.");

        if (parts == null || parts.Length == 0)
            throw new MissingReferenceException($"{name} needs the body parts assigned.");

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == null)
                throw new MissingReferenceException($"{name} needs every body part assigned.");
        }

        character.Profile.ApplyParts(parts);

        authoredColors = new Color[parts.Length];
        for (int i = 0; i < parts.Length; i++)
            authoredColors[i] = parts[i].color;

        ApplyScale();
    }

    void OnDisable()
    {
        if (character != null && !character.IsDead)
            character.SetHoldMovement(false);

        Restore();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.lKey.wasPressedThisFrame)
            BeginDeath();

        if (character.IsDead)
        {
            ApplyScale();
            Play(Dying);
            if (playingDeath && Finished(Dying))
            {
                playingDeath = false;
                if (hideSpriteOnDeath)
                    Hide();
            }

            return;
        }

        if (keyboard != null && keyboard.pKey.wasPressedThisFrame && !playingHit && !character.IsInvulnerable)
            BeginHit();

        if (playingHit)
        {
            ApplyScale();
            if (!Finished(Hurt))
                return;

            playingHit = false;
            if (stopMovementOnHit)
                character.SetHoldMovement(false);

            character.StartInvulnerability(hitInvulnerableDuration);
            blinkUntil = Time.time + hitInvulnerableDuration;
        }

        Play(State(false, false, dashMotion.IsImpulse, movement.IsMoving));
        ApplyScale();
        UpdateBlink();
    }

    void BeginDeath()
    {
        if (character.IsDead)
            return;

        character.Die();
        playingHit = false;
        playingDeath = true;
        blinkUntil = 0f;
        Restore();
        ApplyDeathColor();
    }

    void BeginHit()
    {
        playingHit = true;
        blinkUntil = 0f;
        Restore();
        if (stopMovementOnHit)
            character.SetHoldMovement(true);

        character.Hit();
        Play(Hurt);
    }

    void Play(string stateName)
    {
        if (showing == stateName)
            return;

        showing = stateName;
        animator.Play(stateName, 0, 0f);
    }

    bool Finished(string stateName)
    {
        if (animator.IsInTransition(0))
            return false;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(stateName) && info.normalizedTime >= 1f;
    }

    void ApplyScale()
    {
        float horizontal = movement.CurrentInput.x;
        if (horizontal < 0f)
            facing = -1;
        else if (horizontal > 0f)
            facing = 1;

        float size = character.Scale;
        float fitted = colliderFitScale < 0.01f ? 0.01f : colliderFitScale;
        float factor = size / fitted;
        Vector3 colliderScale = new Vector3(factor, factor, 1f);
        bodyCollider.localScale = colliderScale;
        hurtCollider.localScale = colliderScale;

        float signed = facing * size;
        Vector2 anchor = bodyAnchor * factor;
        visual.localScale = new Vector3(signed, size, size);
        visual.localPosition = new Vector3(
            anchor.x - artAnchor.x * signed,
            anchor.y - artAnchor.y * size,
            0f);
    }

    void UpdateBlink()
    {
        if (hidden)
            return;

        if (Time.time < blinkUntil)
        {
            float interval = blinkInterval < 0.0001f ? 0.0001f : blinkInterval;
            bool visible = Mathf.FloorToInt(Time.time / interval) % 2 == 0;
            for (int i = 0; i < parts.Length; i++)
            {
                Color color = authoredColors[i];
                color.a = visible ? authoredColors[i].a : 0f;
                parts[i].color = color;
            }

            return;
        }

        Restore();
    }

    void ApplyDeathColor()
    {
        for (int i = 0; i < parts.Length; i++)
            parts[i].color = deathColor;
    }

    void Hide()
    {
        hidden = true;
        for (int i = 0; i < parts.Length; i++)
            parts[i].enabled = false;
    }

    void Restore()
    {
        if (hidden || parts == null || authoredColors == null)
            return;

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] != null)
                parts[i].color = authoredColors[i];
        }
    }
}
