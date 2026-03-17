using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    [Header("Límite vertical")]
    public float minY = -20f;
    public float maxY = 60f;

    public Transform player;

    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Leer el ratón
        rotX += Input.GetAxis("Mouse X") * sensitivityX;
        rotY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotY = Mathf.Clamp(rotY, minY, maxY);

        // Rotar el CameraTarget
        transform.rotation = Quaternion.Euler(rotY, rotX, 0f);

        // Seguir la posición del jugador
        transform.position = player.position;
    }
}