using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterProfile profile;
    [SerializeField] CharacterHitChannel characterHit;
    [SerializeField] CharacterDeathChannel characterDeath;

    float invulnerableUntil;

    public CharacterProfile Profile => profile;
    public int MaxHp => profile.MaxHp;
    public float Scale => profile.CharacterScale;
    public bool IsInvulnerable => Time.time < invulnerableUntil;
    public bool HoldMovement { get; private set; }
    public bool IsDead { get; private set; }

    void OnEnable()
    {
        if (profile == null)
            throw new MissingReferenceException($"{name} needs a CharacterProfile assigned.");

        if (characterHit == null)
            throw new MissingReferenceException($"{name} needs a CharacterHit channel assigned.");

        if (characterDeath == null)
            throw new MissingReferenceException($"{name} needs a CharacterDeath channel assigned.");
    }

    public void SetHoldMovement(bool hold)
    {
        if (IsDead)
            return;

        HoldMovement = hold;
    }

    public void Hit()
    {
        if (IsDead)
            return;

        characterHit.Raise(this);
    }

    public void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        HoldMovement = true;
        characterDeath.Raise(this);
    }

    public void StartInvulnerability(float duration)
    {
        if (duration <= 0f)
            return;

        float until = Time.time + duration;
        if (until > invulnerableUntil)
            invulnerableUntil = until;
    }
}
