using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Variáveis para configurar no Inspector do Unity
    [Header("Movimento")]
    public float moveSpeed = 5f;

    [Header("Pulo")]
    public float jumpForce = 7f;
    public int extraJumpsValue = 1;

    [Header("Ataque Giratório")]
    public float planingFallSpeed = 2f; // Velocidade de queda ao planar

    [Header("Verificação de Chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Saúde e Dano")]
    public int maxHealth = 3;
    public float invincibilityTime = 1f;

    // Variáveis privadas
    private int currentHealth;
    private bool isInvincible = false;
    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private int extraJumps;
    private bool isSpinning = false;

    // A função Awake é chamada ANTES de qualquer função Start.
    // É o lugar mais seguro para inicializar variáveis críticas como a vida.
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        extraJumps = extraJumpsValue;
    }

    void Update()
    {
        // --- VERIFICAÇÃO DE ESTADO ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        // --- LEITURA DE INPUT ---
        horizontalInput = Input.GetAxis("Horizontal");

        // Input de Pulo
        if (Input.GetButtonDown("Jump"))
        {
            HandleJump();
        }

        // Input de Ataque Giratório
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("SpinAttack");
        }


        // --- ATUALIZAÇÃO DO ANIMATOR ---
        // Só atualiza as animações de movimento se não estiver girando.
        if (!isSpinning)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            animator.SetBool("IsJumping", !isGrounded);
        }
    }

    void FixedUpdate()
    {
        // --- MOVIMENTO FÍSICO ---
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Lógica de Planar
        if (!isGrounded && isSpinning)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -planingFallSpeed));
        }

        // Vira o personagem
        Flip();
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (extraJumps > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            extraJumps--;
        }
    }

    void Flip()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Funções para serem chamadas pelos Animation Events
    public void StartSpin()
    {
        isSpinning = true;
    }
    public void EndSpin()
    {
        isSpinning = false;
    }

    // LÓGICA DE SAÚDE E DANO
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        isInvincible = true;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFeedback());
        }
    }

    private IEnumerator InvincibilityFeedback()
    {
        // Loop para piscar
        for (float i = 0; i < invincibilityTime; i += 0.1f)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<SpriteRenderer>().enabled = true;
        isInvincible = false;
    }

    private void Die()
    {
        Debug.Log("Jogador foi derrotado!");
        // Por enquanto, vamos apenas recarregar a cena
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // LÓGICA DE COLISÃO PARA ATACAR
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Esta função agora só serve para pular na cabeça dos inimigos
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (groundCheck.position.y > collision.transform.position.y)
            {
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.75f);
                }
            }
        }
    }

    // Desenha o gizmo para o groundCheck no Editor
    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}