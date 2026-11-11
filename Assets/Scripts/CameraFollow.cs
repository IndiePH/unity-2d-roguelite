using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Camera view;
    [Tooltip("Arena edge. The view stays inside this trigger. The barrier on that object stops the body.")]
    [SerializeField] Collider2D confinement;

    void OnEnable()
    {
        if (!Application.isPlaying)
            return;

        if (target == null)
            throw new MissingReferenceException($"{name} needs a follow target assigned.");

        if (view == null)
            throw new MissingReferenceException($"{name} needs a camera assigned.");

        if (!view.orthographic)
            throw new MissingReferenceException($"{name} confines an orthographic camera.");

        if (confinement == null)
            throw new MissingReferenceException($"{name} needs a confinement collider assigned.");
    }

    void LateUpdate()
    {
        if (!Application.isPlaying || target == null || view == null)
            return;

        Vector3 focus = target.position;
        focus.z = transform.position.z;

        if (confinement != null)
            focus = Confine(focus);

        transform.position = focus;
    }

    Vector3 Confine(Vector3 focus)
    {
        Bounds bounds = confinement.bounds;
        float halfHeight = view.orthographicSize;
        float halfWidth = halfHeight * view.aspect;
        focus.x = ClampAxis(focus.x, bounds.min.x, bounds.max.x, bounds.center.x, halfWidth);
        focus.y = ClampAxis(focus.y, bounds.min.y, bounds.max.y, bounds.center.y, halfHeight);
        return focus;
    }

    static float ClampAxis(float value, float min, float max, float center, float halfView)
    {
        float limitMin = min + halfView;
        float limitMax = max - halfView;
        if (limitMin > limitMax)
            return center;

        return Mathf.Clamp(value, limitMin, limitMax);
    }

    void OnDrawGizmos()
    {
        if (confinement == null)
            return;

        Bounds bounds = confinement.bounds;
        Gizmos.color = new Color(0.2f, 0.85f, 1f, 0.9f);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
