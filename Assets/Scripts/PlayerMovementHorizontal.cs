using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementHorizontal : MonoBehaviour
{
    public PlayerData Data;

    private Vector3 moveDirection;
    private Vector3 initPos;

    private bool move = false;

    public float rayDistance = 2f;

    public UnityEvent levelFinish;
    
    void Start()
    {
        initPos = transform.position;

        // Inicia el movimiento hacia la derecha (eje X global positivo)
        moveDirection = Vector3.right;

        // Inicia los listeners
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playerRun.AddListener(this.SetMove);
        levelFinish.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().PlayerEndStage);
    }

    void Update()
    {
        // Movimiento horizontal con coordenadas globales
        if (move) transform.Translate(moveDirection * Data.playerSpeed * Time.deltaTime, Space.World);
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
                moveDirection = Vector3.right; // Reinicia dirección hacia la derecha
                levelFinish.Invoke();
                break;

            default:
                break;
        }
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
            Vector3 newPosition = new Vector3(blockCenter.x, blockCenter.y + blockCollider.bounds.extents.y, blockCenter.z);
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
