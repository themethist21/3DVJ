using UnityEngine;

public class PlayerMovementHorizontal : MonoBehaviour
{
    public float playerSpeed = 5f;
    private Vector3 moveDirection;
    private Vector3 initPos;

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
                break;

            case "RightTurn":
                // Girar hacia la derecha: 90 grados positivos
                RotateGlobal(90);
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
}
