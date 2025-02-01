using System.Collections;
using UnityEngine;

public class Pink : MonoBehaviour
{
    public float fuerzaSalto = 300f;
    public float velocidadMovimiento = 5f;
    public float velocidadCorrer = 8f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = false;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evitar rotación
    }

    void Update()
    {
        if (isAttacking) return; 

        float horizontalInput = 0f;

        // Movimiento lateral
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
            transform.localScale = new Vector3(1, 1, 1);
        }

        bool isRunning = Input.GetKey(KeyCode.S);
        float velocidadActual = isRunning ? velocidadCorrer : velocidadMovimiento;
        rb.velocity = new Vector2(horizontalInput * velocidadActual, rb.velocity.y);

        
        animator.SetBool("is_walking", horizontalInput != 0 && !isRunning);
        animator.SetBool("is_running", horizontalInput != 0 && isRunning);

        
        if (horizontalInput == 0)
        {
            animator.SetBool("is_walking", false);
            animator.SetBool("is_running", false);
        }

        // Salto y doble salto
        if (Input.GetKeyDown(KeyCode.C) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * fuerzaSalto);
            jumpCount++;
            animator.SetBool("is_jumping", true);
        }

        // ataquess
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(PerformAttack("is_attacking"));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(PerformAttack("is_attacking2"));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = true;
            jumpCount = 0;
            animator.SetBool("is_jumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            isGrounded = false;
        }
    }

    private IEnumerator PerformAttack(string attackParameter)
    {
        isAttacking = true;
        animator.SetTrigger(attackParameter);

        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
    }

    
    public void EndAttack()
    {
        isAttacking = false;
    }
}
