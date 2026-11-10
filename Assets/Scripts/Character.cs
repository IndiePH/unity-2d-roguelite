using UnityEngine;

public class Character : MonoBehaviour
{
    float invulnerableUntil;

    public bool IsInvulnerable => Time.time < invulnerableUntil;
    public bool HoldMovement { get; private set; }
    public bool IsDead { get; private set; }

    public void SetHoldMovement(bool hold)
    {
        if (IsDead)
            return;

        HoldMovement = hold;
    }

    public void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        HoldMovement = true;
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
