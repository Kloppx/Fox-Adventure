using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public void TakeDamage()
    {
        // Futuramente, podemos adicionar uma anima��o de morte ou efeitos aqui.
        // Por enquanto, apenas destr�i o objeto do inimigo.
        Destroy(gameObject);
    }
}