using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // O alvo que a câmera deve seguir (nosso jogador)
    public Transform target;

    // A velocidade com que a câmera se move para alcançar o alvo
    // Valores menores deixam a câmera mais "solta", valores maiores deixam ela mais "presa".
    public float smoothSpeed = 0.125f;

    // Um deslocamento da câmera em relação ao alvo (para ajustar o enquadramento)
    public Vector3 offset;

    // Usamos LateUpdate para garantir que o jogador já se moveu naquele frame.
    // Isso evita que a câmera trema.
    void LateUpdate()
    {
        // Posição desejada para a câmera (posição do alvo + deslocamento)
        Vector3 desiredPosition = target.position + offset;

        // Interpola suavemente da posição atual da câmera para a posição desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Atualiza a posição da câmera, mas MANTÉM o seu Z original.
        // Isso é crucial para a câmera 2D não se perder.
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}