using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundDetection;
    public float rayDistance = 0.5f;

    private Rigidbody2D rb;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() // Usamos FixedUpdate para movimento baseado em física
    {
        // Movimenta o inimigo usando a velocidade do Rigidbody
        rb.velocity = new Vector2(movingRight ? speed : -speed, rb.velocity.y);

        // Lança um raio para baixo para detectar o fim da plataforma
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);

        // Se o raio não atingir nada (chegou na beirada)
        if (groundInfo.collider == null)
        {
            Flip();
        }
    }

    void Flip()
    {
        // Inverte a direção do movimento
        movingRight = !movingRight;

        // Inverte a escala do objeto no eixo X para virar o sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}