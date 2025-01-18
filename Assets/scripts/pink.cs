using System.Collections;
using UnityEngine;

public class Pink : MonoBehaviour
{
    public float fuerzaSalto = 300f; // Fuerza del salto
    public float velocidadMovimiento = 5f; // Velocidad al caminar
    public float velocidadCorrer = 8f; // Velocidad al correr

    private Rigidbody2D rb; // Referencia al Rigidbody2D
    private Animator animator; // Referencia al Animator

    private bool isGrounded = false; // ¿Está en el suelo?
    private int jumpCount = 0; // Contador de saltos
    private int maxJumps = 2; // Número máximo de saltos
    private bool isAttacking = false; //indica si el personaje esta atacando

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Asegúrate de congelar la rotación para evitar giros
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Movimiento lateral
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) // Flecha izquierda
        {
            horizontalInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1); // Voltear sprite a la izquierda
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Flecha derecha
        {
            horizontalInput = 1f;
            transform.localScale = new Vector3(1, 1, 1); // Voltear sprite a la derecha
        }
        else
        {
            horizontalInput = 0f; // Detener movimiento
        }

        // Detectar si está corriendo (mantener S)
        bool isRunning = Input.GetKey(KeyCode.S);

        // Aplicar velocidad
        float velocidadActual = isRunning ? velocidadCorrer : velocidadMovimiento;
        rb.velocity = new Vector2(horizontalInput * velocidadActual, rb.velocity.y);

        // Actualizar animaciones de caminar y correr
        animator.SetBool("is_walking", horizontalInput != 0 && !isRunning);
        animator.SetBool("is_running", horizontalInput != 0 && isRunning);

        // Salto y doble salto (tecla C)
        if (Input.GetKeyDown(KeyCode.C) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reiniciar velocidad vertical
            rb.AddForce(Vector2.up * fuerzaSalto);
            jumpCount++;
            animator.SetBool("is_jumping", true);
        }

        // Ataque principal (tecla X)
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("is_attacking"); // Trigger para ataque principal
        }

        // Ataque secundario (tecla D)
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("is_attacking2"); // Trigger para ataque secundario
        }
    }
    
    public void StartAttack()
    {
        isAttacking = true; // Bloquea el movimiento
    }

    public void EndAttack()
    {
        isAttacking = false; // Desbloquea el movimiento
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectar si aterrizó en el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            jumpCount = 0; // Reiniciar contador de saltos
            animator.SetBool("is_jumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detectar si dejó de tocar el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = false;
        }
    }

    private IEnumerator PerformAttack(string attackParameter)
    {
        isAttacking = true; // Bloquear movimiento
        animator.SetTrigger(attackParameter); // Activar animación de ataque

        // Esperar a que termine la animación (usa la duración real de tu clip)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false; // Permitir movimiento nuevamente
    }
}
