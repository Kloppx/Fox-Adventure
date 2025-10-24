using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // O alvo que a c�mera deve seguir (nosso jogador)
    public Transform target;

    // A velocidade com que a c�mera se move para alcan�ar o alvo
    // Valores menores deixam a c�mera mais "solta", valores maiores deixam ela mais "presa".
    public float smoothSpeed = 0.125f;

    // Um deslocamento da c�mera em rela��o ao alvo (para ajustar o enquadramento)
    public Vector3 offset;

    // Usamos LateUpdate para garantir que o jogador j� se moveu naquele frame.
    // Isso evita que a c�mera trema.
    void LateUpdate()
    {
        // Posi��o desejada para a c�mera (posi��o do alvo + deslocamento)
        Vector3 desiredPosition = target.position + offset;

        // Interpola suavemente da posi��o atual da c�mera para a posi��o desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posi��o da c�mera, mas MANT�M o seu Z original.
        // Isso � crucial para a c�mera 2D n�o se perder.
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}