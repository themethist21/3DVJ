using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Ajusta la velocidad para que la animación dure 2 segundos
        float originalDuration = 3.160f; // Duración original de la animación (en segundos)
        float desiredDuration = 0.5f;  // Duración deseada de la animación
        animator.SetFloat("Multiplier", originalDuration / desiredDuration);
    }

    void Update()
    {
        // Ejemplo: Activar la animación con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("JumpTrigger");
        }
    }
}
