using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour moveBehaviour;
    //public Animator animator;
    private Rigidbody _rb;
    private Vector2 moveInput;
    private float velocity;
    [SerializeField]
    private GameObject camera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        moveBehaviour = GetComponent<MoveBehaviour>();
    }
    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    #region Metodos Input System
    public void OnMove(InputAction.CallbackContext context)
    {
      
        moveInput = context.ReadValue<Vector2>();
    }
    
    #endregion

    #region Metodos Update
    private void Update()
    {
        //animator.SetFloat("speed", velocity);
    }
    private void FixedUpdate()
    {
        // Calcula la velocidad horizontal ignorando el eje Y (gravedad)
        velocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;

        // Mueve al personaje pasando:
        // - La dirección del input del jugador
        // - La cámara activa para orientar el movimiento
        // - Si está corriendo o no
        // - Si está en primera persona (cambia cómo se calcula el forward)
        // - La rotación horizontal de la cámara en primera persona
        moveBehaviour.MoveCharacter(
            moveInput,
            camera.transform
        );
    }
    #endregion
}