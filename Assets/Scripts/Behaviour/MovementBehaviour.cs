using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float jumpForce = 7f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void MoveCharacter(Vector2 input, Transform cameraTransform)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;
        float currentSpeed = walkSpeed;

        _rb.linearVelocity = new Vector3(
            move.x * currentSpeed,
            _rb.linearVelocity.y,
            move.z * currentSpeed
        );

        Vector3 lookDirection = new Vector3(move.x, 0, move.z);
        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
}
