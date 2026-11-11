using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-10)]
public class CharacterDash : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] CharacterMovement movement;
    [SerializeField] Rigidbody2D body;
    [SerializeField] InputActionReference dash;
    [Tooltip("Units per second during the push.")]
    [SerializeField] float impulseSpeed = 16f;
    [Tooltip("How long the push lasts, in seconds.")]
    [SerializeField] float impulseDuration = 0.12f;
    [Tooltip("Invulnerable time from the start of the dash, in seconds. Longer than the push keeps them safe after it stops.")]
    [SerializeField] float invulnerableDuration = 0.35f;
    [Tooltip("How many dashes are stored. An upgrade can raise this later.")]
    [SerializeField] int maxUses = 2;
    [Tooltip("Seconds to restore one use. The timer starts when a dash ends.")]
    [SerializeField] float rechargeDuration = 1f;

    Vector2 direction;
    float impulseUntil;
    float rechargeAt;
    int uses;
    bool wasDashing;

    public bool IsImpulse => Time.time < impulseUntil;

    void OnEnable()
    {
        if (character == null)
            throw new MissingReferenceException($"{name} needs a Character assigned.");

        if (movement == null)
            throw new MissingReferenceException($"{name} needs CharacterMovement assigned.");

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

        impulseUntil = 0f;
        rechargeAt = 0f;
        if (body != null)
            body.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (character.IsDead)
        {
            impulseUntil = 0f;
            rechargeAt = 0f;
            wasDashing = false;
            return;
        }

        bool dashing = IsImpulse;
        if (wasDashing && !dashing && uses < maxUses)
            rechargeAt = Time.time + rechargeDuration;

        if (!dashing && rechargeAt > 0f && Time.time >= rechargeAt)
        {
            uses++;
            if (uses > maxUses)
                uses = maxUses;

            rechargeAt = uses < maxUses ? Time.time + rechargeDuration : 0f;
        }

        if (!character.HoldMovement && !dashing && uses > 0 && dash.action.WasPressedThisFrame())
            TryDash();

        wasDashing = IsImpulse;
    }

    void TryDash()
    {
        Vector2 input = movement.CurrentInput;
        if (input.sqrMagnitude < 0.0001f)
            return;

        direction = input.normalized;
        uses--;
        impulseUntil = Time.time + impulseDuration;
        rechargeAt = 0f;
        character.StartInvulnerability(invulnerableDuration);
    }

    void FixedUpdate()
    {
        if (character.IsDead || !IsImpulse || character.HoldMovement)
            return;

        body.linearVelocity = direction * impulseSpeed;
    }
}
