using UnityEngine;

public class PlayerMovementHorizontal : MonoBehaviour
{
    public PlayerData Data;

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
        transform.Translate(moveDirection * Data.playerSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "LeftTurn":
                // Girar hacia la izquierda: 90 grados negativos
                RotateGlobal(-90);
                transform.SetPositionAndRotation(FixCoords(transform.position), transform.rotation);
                break;

            case "RightTurn":
                // Girar hacia la derecha: 90 grados positivos
                RotateGlobal(90);
                transform.SetPositionAndRotation(FixCoords(transform.position), transform.rotation);

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
        moveDirection = Quaternion.Euler(0, angle, 0) * moveDirection;
    }

    Vector3 FixCoords(Vector3 coords)
    {
        // Redondear las coordenadas globales
        return new Vector3(Mathf.RoundToInt(coords.x), coords.y, Mathf.RoundToInt(coords.z));
    }
}
