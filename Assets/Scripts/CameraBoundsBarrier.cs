using UnityEngine;

[ExecuteAlways]
public class CameraBoundsBarrier : MonoBehaviour
{
    [SerializeField] BoxCollider2D area;
    [Tooltip("Solid loop on the outside of the area. Its radius is the wall thickness on each side of the line.")]
    [SerializeField] EdgeCollider2D barrier;
    [Tooltip("Moves the top edge up, in the same local units as the area. Positive lets the body walk higher than the box. Half a tile is 0.5.")]
    [SerializeField] float topOffset;

    void OnEnable()
    {
        if (!Application.isPlaying)
        {
            Apply();
            return;
        }

        if (area == null)
            throw new MissingReferenceException($"{name} needs the area box assigned.");

        if (barrier == null)
            throw new MissingReferenceException($"{name} needs the barrier edge assigned.");

        Apply();
    }

    void OnValidate()
    {
        Apply();
    }

    void Update()
    {
        Apply();
    }

    void Apply()
    {
        if (area == null || barrier == null)
            return;

        float radius = Mathf.Max(0f, barrier.edgeRadius);
        Vector2 center = area.offset;
        Vector2 half = area.size * 0.5f + new Vector2(radius, radius);
        Vector2 min = center - half;
        Vector2 max = center + half;
        max.y += topOffset;
        Vector2[] points =
        {
            new Vector2(min.x, min.y),
            new Vector2(max.x, min.y),
            new Vector2(max.x, max.y),
            new Vector2(min.x, max.y),
            new Vector2(min.x, min.y)
        };

        if (Same(barrier.points, points))
            return;

        barrier.points = points;
    }

    static bool Same(Vector2[] current, Vector2[] next)
    {
        if (current == null || current.Length != next.Length)
            return false;

        for (int i = 0; i < current.Length; i++)
        {
            if ((current[i] - next[i]).sqrMagnitude > 0.000001f)
                return false;
        }

        return true;
    }
}
