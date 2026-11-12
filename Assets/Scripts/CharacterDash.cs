using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-10)]
public class CharacterDash : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] CharacterMovement movement;
    [SerializeField] CharacterAnimation characterAnimation;
    [SerializeField] Rigidbody2D body;
    [SerializeField] InputActionReference dash;
    [Tooltip("Units per second during the push.")]
    [SerializeField] float impulseSpeed = 16f;
    [Tooltip("How far the dash travels, in units. Twice the idle height at this size. A later change to character scale does not change this.")]
    [SerializeField] float distance = 0.6842f;
    [Tooltip("Invulnerable time from the start of the dash, in seconds. Longer than the push keeps them safe after it stops.")]
    [SerializeField] float invulnerableDuration = 0.35f;
    [Tooltip("How many dashes are stored. An upgrade can raise this later.")]
    [SerializeField] int maxUses = 2;
    [Tooltip("Seconds to restore one use. The timer starts when a dash ends.")]
    [SerializeField] float rechargeDuration = 1f;

    Vector2 direction;
    float traveled;
    float rechargeAt;
    int uses;
    bool dashing;
    bool releaseDash;
    bool wasDashing;

    public bool IsImpulse => dashing;

    public static Vector2 Direction(Vector2 input, int facing)
    {
        if (input.sqrMagnitude < 0.0001f)
            return facing < 0 ? Vector2.left : Vector2.right;

        return input.normalized;
    }

    public static float Step(float traveled, float speed, float deltaTime, float distance)
    {
        float remaining = distance - traveled;
        if (remaining <= 0f)
            return 0f;

        float step = speed * deltaTime;
        return step < remaining ? step : remaining;
    }

    void OnEnable()
    {
        if (character == null)
            throw new MissingReferenceException($"{name} needs a Character assigned.");

        if (movement == null)
            throw new MissingReferenceException($"{name} needs CharacterMovement assigned.");

        if (characterAnimation == null)
            throw new MissingReferenceException($"{name} needs CharacterAnimation assigned.");

        if (body == null)
            throw new MissingReferenceException($"{name} needs a Rigidbody2D assigned.");

        if (dash == null || dash.action == null)
            throw new MissingReferenceException($"{name} needs the Dash action assigned.");

        dash.action.Enable();
    }

    void Awake()
    {
        uses = maxUses < 0 ? 0 : maxUses;
    }

    void OnDisable()
    {
        if (dash != null && dash.action != null)
            dash.action.Disable();

        dashing = false;
        releaseDash = false;
        rechargeAt = 0f;
        if (body != null)
            body.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (character.IsDead)
        {
            dashing = false;
            releaseDash = false;
            rechargeAt = 0f;
            wasDashing = false;
            return;
        }

        if (character.HoldMovement)
        {
            dashing = false;
            releaseDash = false;
        }

        bool pushing = IsImpulse;
        if (wasDashing && !pushing && uses < maxUses)
            rechargeAt = Time.time + rechargeDuration;

        if (!pushing && rechargeAt > 0f && Time.time >= rechargeAt)
        {
            uses++;
            if (uses > maxUses)
                uses = maxUses;

            rechargeAt = uses < maxUses ? Time.time + rechargeDuration : 0f;
        }

        if (!character.HoldMovement && !pushing && uses > 0 && dash.action.WasPressedThisFrame())
            TryDash();

        wasDashing = IsImpulse;
    }

    void TryDash()
    {
        direction = Direction(movement.CurrentInput, characterAnimation.Facing);
        uses--;
        traveled = 0f;
        dashing = true;
        releaseDash = false;
        rechargeAt = 0f;
        character.StartInvulnerability(invulnerableDuration);
    }

    void FixedUpdate()
    {
        if (releaseDash)
        {
            dashing = false;
            releaseDash = false;
            return;
        }

        if (!dashing || character.IsDead || character.HoldMovement)
            return;

        float step = Step(traveled, impulseSpeed, Time.fixedDeltaTime, distance);
        if (step <= 0f)
        {
            dashing = false;
            return;
        }

        body.linearVelocity = direction * (step / Time.fixedDeltaTime);
        traveled += step;
        if (traveled >= distance)
            releaseDash = true;
    }
}
