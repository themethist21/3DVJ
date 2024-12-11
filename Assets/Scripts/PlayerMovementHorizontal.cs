using UnityEngine;

public class PlayerMovementHorizontal : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Vector3 moveDirection;
    private Vector3 initPos;

    public float rayDistance = 2f;

    void Start()
    {
        initPos = transform.position;

        // Inicia el movimiento hacia la derecha (eje X global positivo)
        moveDirection = Vector3.right;
    }

    void Update()
    {
        // Movimiento horizontal con coordenadas globales
        transform.Translate(moveDirection * playerSpeed * Time.deltaTime, Space.World);
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
        if (angle < 0) // Giro a la izquierda
        {
            if (moveDirection == Vector3.right) moveDirection = Vector3.forward;
            else if (moveDirection == Vector3.forward) moveDirection = Vector3.left;
            else if (moveDirection == Vector3.left) moveDirection = Vector3.back;
            else if (moveDirection == Vector3.back) moveDirection = Vector3.right;
        }
        else // Giro a la derecha
        {
            if (moveDirection == Vector3.right) moveDirection = Vector3.back;
            else if (moveDirection == Vector3.forward) moveDirection = Vector3.right;
            else if (moveDirection == Vector3.left) moveDirection = Vector3.forward;
            else if (moveDirection == Vector3.back) moveDirection = Vector3.left;
        }
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
}
