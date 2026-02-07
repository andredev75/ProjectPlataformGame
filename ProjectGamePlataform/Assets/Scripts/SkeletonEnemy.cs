using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonEnemy : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public SpriteRenderer sr;
    public Animator anim;

    [Header("Movimento")]
    public float velocidade = 2f;
    public float distanciaAtaque = 1.5f;

    [Header("Teleport")]
    public float tempoParaTeleportar = 10f;
    public float tempoParadoAntesTeleport = 2f;
    public float distanciaMinimaDoPlayer = 2f;

    [Header("Limites da Tela")]
    public float limiteXMin = -8f;
    public float limiteXMax = 8f;

    private bool atacando;
    private bool teleportando;
    private float cronometroTeleport;


    void Start()
    {
        cronometroTeleport = tempoParaTeleportar;
    }

    void Update()
    {
        if (teleportando)
        {
            return;
        } 

        cronometroTeleport -= Time.deltaTime;

        float distanciaX = Mathf.Abs(player.position.x - transform.position.x);

        if (distanciaX <= distanciaAtaque)
        {
            Atacar();
        }
        else
        {
            Andar();
        }

        if (cronometroTeleport <= 0)
        {
            StartCoroutine(Teleportar());
        }
    }

    void Andar()
    {
        atacando = false;

        float direcao = Mathf.Sign(player.position.x - transform.position.x);
        transform.position += Vector3.right * direcao * velocidade * Time.deltaTime;
        Vector3 escala = transform.localScale;

        if (direcao > 0)
        {
            escala.x = Mathf.Abs(escala.x);
        }
        else
        {
            escala.x = -Mathf.Abs(escala.x);
        }

        transform.localScale = escala;
        anim.Play("esqueleto_walk");
    }

    void Atacar()
    {
        if (atacando) return;

        atacando = true;
        anim.Play("esqueleto_attack");
    }

    IEnumerator Teleportar()
    {
        teleportando = true;
        anim.Play("esqueleto_idle");

        yield return new WaitForSeconds(tempoParadoAntesTeleport);

        // Fade out
        for (float a = 1; a > 0; a -= Time.deltaTime * 2)
        {
            sr.color = new Color(1, 1, 1, a);
            yield return null;
        }

        TeleportarParaNovaPosicao();

        // Fade in
        for (float a = 0; a < 1; a += Time.deltaTime * 2)
        {
            sr.color = new Color(1, 1, 1, a);
            yield return null;
        }

        sr.color = Color.white;
        cronometroTeleport = tempoParaTeleportar;
        teleportando = false;
    }

    void TeleportarParaNovaPosicao()
    {
        float novoX;

        do
        {
            novoX = Random.Range(limiteXMin, limiteXMax);
        }
        while (Mathf.Abs(novoX - player.position.x) < distanciaMinimaDoPlayer);

        transform.position = new Vector3(
            novoX,
            player.position.y,
            transform.position.z
        );
    }

}
