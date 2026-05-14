using CustomMath;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private RenderOptimization renderOptimization;
    void Update()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Vec3 rotationDir = Vec3.Zero;
        Vec3 moveDir = Vec3.Zero;

        //Rotate
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

        //Move
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = new Vec3(-1f, 0f, 0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir = new Vec3(1f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveDir = new Vec3(0f, 0f, 1f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir = new Vec3(0f, 0f, -1f);
        }

        transform.Rotate(rotationDir * (Time.deltaTime * rotationSpeed));
        transform.position += moveDir * (Time.deltaTime * moveSpeed);

        if (rotationDir != Vec3.Zero || moveDir != Vec3.Zero)
        {
            renderOptimization.CalculateRays();
        }
    }
}
