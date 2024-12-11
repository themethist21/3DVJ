using UnityEngine;

public enum PlayerStates
{
    Grounded,
    Jump,
    Fall
}

public class PlayerJumpController : MonoBehaviour
{
    public PlayerData Data;

    private Rigidbody rb;
    private PlayerStates state = PlayerStates.Grounded;
    private float gravityScale = 1.0f;

    private float lastJumpInputTimer = 0.0f;

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
        //Timers
        lastJumpInputTimer -= Time.deltaTime;

        //Input

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpInputTimer = Data.jumpInputBufferTime;
        }

        // Detectar entrada para salto
        if (lastJumpInputTimer > 0.0f && state == PlayerStates.Grounded)
        {
            Jump();
        }

        // Cambiar a estado de caída si la velocidad vertical es negativa
        if (state == PlayerStates.Jump && rb.linearVelocity.y < 0)
        {
            state = PlayerStates.Fall;
            SetGravityScale(Data.fallGravityMult); // Aumentar la gravedad en la caída
        }
    }

    void FixedUpdate()
    {
        // Aplicar gravedad manualmente
        Vector3 gravity = gravityScale * Data.gravityStrength * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        state = PlayerStates.Grounded;
        SetGravityScale(1.0f); // Restaurar la gravedad normal
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JumpTrigger")
        {
            lastJumpInputTimer = Data.jumpInputBufferTime;
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
