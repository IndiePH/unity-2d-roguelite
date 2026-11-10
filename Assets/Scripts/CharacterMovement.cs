using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] InputActionReference move;
    [SerializeField] float speed = 5f;
    [Tooltip("Scale multiplier at one end of the walk bob. The other end is the authored scale.")]
    [SerializeField] Vector3 movingScale = new Vector3(1.15f, 0.85f, 1f);
    [Tooltip("Seconds for one half of the walk bob.")]
    [SerializeField] float squishDuration = 0.2f;
    [Tooltip("Scale multiplier at the top of an idle breath. The other end is the authored scale.")]
    [SerializeField] Vector3 breathScale = new Vector3(0.98f, 1.05f, 1f);
    [Tooltip("Seconds for one half of an idle breath.")]
    [SerializeField] float breathDuration = 1.5f;
    [Tooltip("How far the body lifts, in units, at the top of an idle breath.")]
    [SerializeField] float breathBob = 0.05f;
    [Tooltip("Capsule breath and walk squish. Leave this off for a sprite character.")]
    [SerializeField] bool useScaleMotion = true;
    [Tooltip("Optional. While its push is active, walking does not also move the body.")]
    [SerializeField] CharacterDash dashMotion;
    [Tooltip("Optional. While the body is holding movement, walking does not move it.")]
    [SerializeField] Character character;
    [Tooltip("Optional. When set, walking sets this body's velocity. The capsule leaves this empty and moves its transform.")]
    [SerializeField] Rigidbody2D body;

    Vector3 authoredScale;
    Vector2 currentInput;
    float idleBaseY;
    bool moving;
    MotionHandle squish;
    MotionHandle breath;
    MotionHandle bob;

    public bool IsMoving => moving;
    public Vector2 CurrentInput => currentInput;

    void Awake()
    {
        authoredScale = transform.localScale;
    }

    void OnEnable()
    {
        if (move == null || move.action == null)
            throw new MissingReferenceException($"{name} needs the MoveWasd action assigned.");

        move.action.Enable();
    }

    void Start()
    {
        if (useScaleMotion)
            PlayBreath();
    }

    void OnDisable()
    {
        if (move != null && move.action != null)
            move.action.Disable();

        CancelSquish();
        StopBreath();
        if (body != null)
            body.linearVelocity = Vector2.zero;
        if (authoredScale != Vector3.zero)
            transform.localScale = authoredScale;
    }

    void Update()
    {
        if (character != null && character.IsDead)
        {
            currentInput = Vector2.zero;
            moving = false;
            if (body != null)
                body.linearVelocity = Vector2.zero;
            return;
        }

        currentInput = move.action.ReadValue<Vector2>();
        if (currentInput.sqrMagnitude > 1f)
            currentInput.Normalize();

        bool isMoving = currentInput.sqrMagnitude > 0f;
        if (dashMotion != null && dashMotion.IsImpulse)
            return;

        if (character != null && character.HoldMovement)
        {
            moving = isMoving;
            return;
        }

        if (body == null && isMoving)
        {
            Vector3 step = new Vector3(currentInput.x, currentInput.y, 0f) * (speed * Time.deltaTime);
            transform.position += step;
        }

        if (isMoving == moving)
            return;

        moving = isMoving;
        if (!useScaleMotion)
            return;

        if (moving)
            PlayBob();
        else
            PlaySettle();
    }

    void FixedUpdate()
    {
        if (body == null)
            return;

        if (character != null && (character.IsDead || character.HoldMovement))
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        if (dashMotion != null && dashMotion.IsImpulse)
            return;

        body.linearVelocity = currentInput * speed;
    }

    void PlayBreath()
    {
        CancelSquish();
        if (breath.IsActive())
            breath.Cancel();
        if (bob.IsActive())
            bob.Cancel();

        idleBaseY = transform.localPosition.y;
        Vector3 inhaled = Vector3.Scale(authoredScale, breathScale);
        breath = LMotion.Create(authoredScale, inhaled, breathDuration)
            .WithLoops(-1, LoopType.Yoyo)
            .BindToLocalScale(transform)
            .AddTo(this);
        bob = LMotion.Create(idleBaseY, idleBaseY + breathBob, breathDuration)
            .WithLoops(-1, LoopType.Yoyo)
            .BindToLocalPositionY(transform)
            .AddTo(this);
    }

    void StopBreath()
    {
        bool restoreHeight = breath.IsActive() || bob.IsActive();
        if (breath.IsActive())
            breath.Cancel();
        if (bob.IsActive())
            bob.Cancel();

        if (!restoreHeight)
            return;

        Vector3 position = transform.localPosition;
        position.y = idleBaseY;
        transform.localPosition = position;
    }

    void PlayBob()
    {
        StopBreath();
        CancelSquish();
        Vector3 squished = Vector3.Scale(authoredScale, movingScale);
        squish = LMotion.Create(authoredScale, squished, squishDuration)
            .WithLoops(-1, LoopType.Yoyo)
            .BindToLocalScale(transform)
            .AddTo(this);
    }

    void PlaySettle()
    {
        CancelSquish();
        squish = LMotion.Create(transform.localScale, authoredScale, squishDuration)
            .WithOnComplete(BeginBreathIfIdle)
            .BindToLocalScale(transform)
            .AddTo(this);
    }

    void BeginBreathIfIdle()
    {
        if (!moving)
            PlayBreath();
    }

    void CancelSquish()
    {
        if (squish.IsActive())
            squish.Cancel();
    }
}
