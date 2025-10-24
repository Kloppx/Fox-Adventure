using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 2f; // O inimigo só pode causar dano a cada 2 segundos

    private bool canDealDamage = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Se não puder causar dano, ignora tudo
        if (!canDealDamage) return;

        // Verifica se colidiu com o Player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);

                // Inicia o tempo de recarga para não causar dano novamente
                StartCoroutine(Cooldown());
            }
        }
    }

    private IEnumerator Cooldown()
    {
        // Impede o inimigo de causar dano
        canDealDamage = false;
        // Espera o tempo definido
        yield return new WaitForSeconds(damageCooldown);
        // Permite que o inimigo cause dano novamente
        canDealDamage = true;
    }
}