using UnityEngine;
using UnityEngine.Events;

public enum PlayerStates
{
    Grounded,
    Jump,
    Fall
}

public class PlayerMovementHorizontal : MonoBehaviour
{
    public PlayerData Data;

    private Rigidbody rb;

    private float gravityScale = 1.0f;

    private float lastJumpInputTimer = 0.0f;

    private Vector3 moveDirection;

    public PlayerStates state = PlayerStates.Grounded;

    private Vector3 initPos;

    private Vector3 initRot;

    private bool move = false;

    public float rayDistance = 2f;

    public UnityEvent levelFinish;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivamos la gravedad predeterminada
        initPos = transform.position;
        initRot = transform.eulerAngles;

        // Inicia el movimiento hacia la derecha (eje X global positivo)
        moveDirection = Vector3.right;

        // Inicia los listeners
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playerRun.AddListener(this.SetMove);
        levelFinish.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerEndStage);
    }

    void Update()
    {
        // Movimiento horizontal con coordenadas globales
        if (move){
            transform.Translate(moveDirection * Data.playerSpeed * Time.deltaTime, Space.World);
        }

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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "LeftTurn":
                // Girar hacia la izquierda: 90 grados negativos
                RotateGlobal(-90);
                PositionOnBlock();
                break;

            case "RightTurn":
                // Girar hacia la derecha: 90 grados positivos
                RotateGlobal(90);
                PositionOnBlock();
                break;

            case "LevelFinish":
                // Reiniciar posición al inicio
                transform.position = initPos;
                transform.eulerAngles = initRot;
                SetmoveDirection(Vector3.right); // Reinicia dirección hacia la derecha
                levelFinish.Invoke();
                break;

            case "Spikes":
                // Reiniciar posición al inicio
                transform.position = initPos; //por si te pilla en medio de un salto
                transform.eulerAngles = initRot;
                SetmoveDirection(Vector3.right); // Reinicia dirección hacia la derecha
                break;
            case "JumpTrigger":
                lastJumpInputTimer = Data.jumpInputBufferTime;
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
        rb.AddForce(Vector3.up * Data.jumpForce, ForceMode.Impulse);
        state = PlayerStates.Jump;
        Debug.Log("Estado después de saltar: " + state);
    }

    void OnCollisionEnter(Collision collision)
    {
        state = PlayerStates.Grounded;
        SetGravityScale(1.0f); // Restaurar la gravedad normal
    }

    private void SetGravityScale(float scale)
    {
        gravityScale = scale;
    }

    void FixedUpdate()
    {
        // Aplicar gravedad manualmente
        Vector3 gravity = gravityScale * Data.gravityStrength * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void PositionOnBlock()
    {
        // Dirección del rayo (hacia abajo)
        Vector3 direction = Vector3.down;

        // Lanzar el raycast desde la posición del objeto
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, rayDistance))
        {
            // Si el raycast golpea un objeto, obtiene su Collider
            Collider blockCollider = hitInfo.collider;

            // Calcula el centro del bloque usando el bounding box del Collider
            Vector3 blockCenter = blockCollider.bounds.center;

            // Posiciona al personaje en el centro del bloque (ajusta la altura si es necesario)
            Vector3 newPosition = new Vector3(blockCenter.x, transform.position.y, blockCenter.z);
            transform.position = newPosition;

            Debug.Log($"Posicionado sobre el bloque: {blockCollider.gameObject.name}");
            Debug.Log($"Centro del bloque: {blockCenter}");
        }
        else
        {
            Debug.Log("No hay bloques debajo.");
        }
    }

    private void RotateGlobal(float angle)
    {
        // Rotar el objeto en el eje Y global
        transform.Rotate(Vector3.up, angle, Space.World);

        // Actualizar la dirección global basada en la rotación
        moveDirection = Quaternion.Euler(0, angle, 0) * moveDirection;
    }

    Vector3 FixCoords(Vector3 coords)
    {
        // Redondear las coordenadas globales
        return new Vector3(Mathf.RoundToInt(coords.x), coords.y, Mathf.RoundToInt(coords.z));
    }

        // Método para obtener el valor del atributo privado
    public Vector3 GetmoveDirection()
    {
        return moveDirection;
    }

    // Método para establecer el valor del atributo privado
    public void SetmoveDirection(Vector3 value)
    {
        moveDirection = value;
    }

    public void SetMove(bool b)
    { 
        move = b;
    }
}
