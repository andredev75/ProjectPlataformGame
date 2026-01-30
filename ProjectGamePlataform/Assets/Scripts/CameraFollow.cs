using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo; // jogador
    public float suavizacao = 0.15f;

    [Header("Look Ahead")]
    public float distanciaLookAhead = 2f;
    public float velocidadeLookAhead = 5f;

    private Vector3 velocidadeAtual = Vector3.zero;
    private Vector3 lookAheadAtual;
    private float ultimoXAlvo;

    void Start()
    {
        ultimoXAlvo = alvo.position.x;
    }

    void LateUpdate()
    {
        float deltaX = alvo.position.x - ultimoXAlvo;
        ultimoXAlvo = alvo.position.x;

        Vector3 lookAheadAlvo = Vector3.right * Mathf.Sign(deltaX) * distanciaLookAhead;
        lookAheadAtual = Vector3.Lerp(lookAheadAtual, lookAheadAlvo, Time.deltaTime * velocidadeLookAhead);

        Vector3 posicaoDesejada = alvo.position + lookAheadAtual;
        posicaoDesejada.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, posicaoDesejada, ref velocidadeAtual, suavizacao);
    }
}
