using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public void TakeDamage()
    {
        // Futuramente, podemos adicionar uma animação de morte ou efeitos aqui.
        // Por enquanto, apenas destrói o objeto do inimigo.
        Destroy(gameObject);
    }
}