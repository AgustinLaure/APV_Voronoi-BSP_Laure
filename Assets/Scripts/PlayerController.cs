using CustomMath;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private RenderOptimization renderOptimization;
    void Update()
    {
        RotatePlayer();
    }

   private void RotatePlayer()
    {
        Vec3 rotationDir = Vec3.Zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationDir = new Vec3(0f, -1f, 0f);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationDir = new Vec3(0f, 1f, 0f);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotationDir = new Vec3(1f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rotationDir = new Vec3(-1f, 0f, 0f);
        }

        transform.Rotate(rotationDir * (Time.deltaTime * rotationSpeed));

        renderOptimization.CalculateRays();
    }
}
