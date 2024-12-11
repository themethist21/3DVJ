using UnityEngine;

public enum PlayerStates
{
    Grounded,
    Jump,
    Fall
}

public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 10f; // Fuerza del salto
    public float gravityStrength = -9.81f; // Gravedad aplicada
    public float fallGravityMultiplier = 2f; // Gravedad multiplicada en caída

    private Rigidbody rb;
    private PlayerStates state = PlayerStates.Grounded;
    private float gravityScale = 1.0f;

    private Vector3 initPos;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivamos la gravedad predeterminada

        Collider parentCollider = transform.parent.GetComponent<Collider>();
        Collider childCollider = GetComponent<Collider>();

        if (parentCollider != null && childCollider != null)
        {
            Physics.IgnoreCollision(parentCollider, childCollider);
            Debug.Log("Colisión entre padre e hijo ignorada.");
        }

        initPos = transform.parent.position;

        
        
    }

    void Update()
    {

        // Detectar entrada para salto
        if (Input.GetKeyDown(KeyCode.Space) && state == PlayerStates.Grounded)
        {
            Debug.Log("Saltando...");
            Jump();
        }

        // Cambiar a estado de caída si la velocidad vertical es negativa
        if (state == PlayerStates.Jump && rb.linearVelocity.y < 0)
        {
            state = PlayerStates.Fall;
            SetGravityScale(fallGravityMultiplier); // Aumentar la gravedad en la caída
            Debug.Log("Cambiando a estado Fall.");
        }
    }

    void FixedUpdate()
    {
        // Aplicar gravedad manualmente
        Vector3 gravity = Vector3.up * gravityStrength * gravityScale;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        state = PlayerStates.Grounded;
        SetGravityScale(1.0f); // Restaurar la gravedad normal
        Debug.Log("Cambiando a estado Grounded.");
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag){
            case "Spikes":
                // Reiniciar posición al inicio
                PlayerMovementHorizontal parentScript = transform.parent.GetComponent<PlayerMovementHorizontal>();
               
                transform.parent.position = initPos;
                transform.position = initPos; //por si te pilla en medio de un salto
                parentScript.SetmoveDirection(Vector3.right); // Reinicia dirección hacia la derecha
                break;
            default:
                break;
        }
    }


    private void Jump()
    {
        // Aplicar fuerza de salto
        Debug.Log("Saltando...");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reiniciar velocidad vertical
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        state = PlayerStates.Jump;
        Debug.Log("Estado después de saltar: " + state);
    }

    private void SetGravityScale(float scale)
    {
        gravityScale = scale;
    }
}
