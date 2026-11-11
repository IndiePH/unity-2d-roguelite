using System;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class CameraShake : MonoBehaviour
{
    [Serializable]
    public struct Profile
    {
        [Tooltip("How long this shake lasts, in seconds.")]
        [Min(0f)] public float duration;

        [Tooltip("Farthest the camera moves from its rest position, in world units.")]
        [Min(0f)] public float positionAmplitude;

        [Tooltip("Farthest the camera rolls, in degrees.")]
        [Min(0f)] public float rotationDegrees;

        [Tooltip("How fast the offset changes. Higher is tighter. Zero holds one offset and lets it fade.")]
        [Min(0f)] public float frequency;
    }

    [SerializeField] Profile hit = new Profile
    {
        duration = 0.16f,
        positionAmplitude = 0.25f,
        rotationDegrees = 1.5f,
        frequency = 24f
    };

    [SerializeField] Profile death = new Profile
    {
        duration = 0.36f,
        positionAmplitude = 0.4f,
        rotationDegrees = 2.5f,
        frequency = 16f
    };

    [Tooltip("Follow pose this shake offsets. That component runs first. Empty means this shake owns the camera pose.")]
    [SerializeField] CameraFollow follow;
    [SerializeField] CharacterHitChannel characterHit;
    [SerializeField] CharacterDeathChannel characterDeath;

    Vector3 restLocalPosition;
    Quaternion restLocalRotation;
    float timeLeft;
    float duration;
    float positionAmplitude;
    float rotationDegrees;
    float frequency;
    float seed;
    bool shaking;

    void OnEnable()
    {
        if (characterHit == null)
            throw new MissingReferenceException($"{name} needs a CharacterHit channel assigned.");

        if (characterDeath == null)
            throw new MissingReferenceException($"{name} needs a CharacterDeath channel assigned.");

        characterHit.Register(OnHit);
        characterDeath.Register(OnDeath);
    }

    void OnHit(Character character)
    {
        if (character == null)
            return;

        Play(hit);
    }

    void OnDeath(Character character)
    {
        if (character == null)
            return;

        Play(death);
    }

    void Play(Profile profile)
    {
        if (profile.duration <= 0f)
            return;

        if (!shaking)
        {
            restLocalPosition = transform.localPosition;
            restLocalRotation = transform.localRotation;
        }

        shaking = true;
        timeLeft = profile.duration;
        duration = profile.duration;
        positionAmplitude = Mathf.Max(0f, profile.positionAmplitude);
        rotationDegrees = Mathf.Max(0f, profile.rotationDegrees);
        frequency = Mathf.Max(0f, profile.frequency);
        seed = UnityEngine.Random.Range(0f, 1000f);
    }

    void LateUpdate()
    {
        if (!shaking)
            return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            Stop();
            return;
        }

        float envelope = timeLeft / duration;
        envelope *= envelope;
        float sample = (duration - timeLeft) * frequency;
        float x = Mathf.PerlinNoise(seed, sample) * 2f - 1f;
        float y = Mathf.PerlinNoise(seed + 19f, sample) * 2f - 1f;
        float roll = Mathf.PerlinNoise(seed + 47f, sample) * 2f - 1f;
        Vector3 offset = new Vector3(x, y, 0f) * (positionAmplitude * envelope);
        Quaternion rollRotation = restLocalRotation * Quaternion.Euler(0f, 0f, roll * rotationDegrees * envelope);
        if (follow != null)
            transform.localPosition += offset;
        else
            transform.localPosition = restLocalPosition + offset;

        transform.localRotation = rollRotation;
    }

    void OnDisable()
    {
        if (characterHit != null)
            characterHit.Unregister(OnHit);

        if (characterDeath != null)
            characterDeath.Unregister(OnDeath);

        if (shaking)
            Stop();
    }

    void Stop()
    {
        shaking = false;
        timeLeft = 0f;
        if (follow == null)
            transform.localPosition = restLocalPosition;

        transform.localRotation = restLocalRotation;
    }
}
