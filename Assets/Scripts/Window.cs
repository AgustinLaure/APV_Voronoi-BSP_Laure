using CustomMath;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Vec3 Min;
    public Vec3 Max;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Min = new Vec3(boxCollider.bounds.min);
        Max = new Vec3(boxCollider.bounds.max);
    }

    public bool IsInsideWindow(Vec3 point)
    {
        bool isInside = false;

        if (point.x > Min.x && point.y > Min.y && point.z > Min.z
            && point.x < Max.x && point.y < Max.y && point.z < Max.z)
        {
            isInside = true;
        }

        return isInside;
    }
}
