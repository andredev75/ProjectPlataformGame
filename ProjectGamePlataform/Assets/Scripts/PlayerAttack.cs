using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Referências")]
    public Animator anim;
    public Transform pontoAtaque;
    public LayerMask layerInimigos;

    [Header("Ataque")]
    public int dano = 10;
    public float tempoAtaque = 0.3f;
    public float raioAtaque = 0.6f;

    private bool atacando;
    private PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TentarAtacar();
        }
    }

    void TentarAtacar()
    {
        if (atacando) return;
        if (!controller.PodeAtacar()) return;

        StartCoroutine(AtaqueCoroutine());
    }

    IEnumerator AtaqueCoroutine()
    {
        atacando = true;
        controller.atacando = true;

        anim.Play("player_attack_idle");
        DetectarInimigos();

        yield return new WaitForSeconds(tempoAtaque);

        atacando = false;
        controller.atacando = false;
    }

    void DetectarInimigos()
    {
        Collider2D[] inimigos = Physics2D.OverlapCircleAll(
            pontoAtaque.position,
            raioAtaque,
            layerInimigos
        );

        foreach (Collider2D col in inimigos)
        {
            LifeInimigos inimigoLife = col.GetComponent<LifeInimigos>();
            if (inimigoLife != null)
            {
                inimigoLife.ReceberDano(dano);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pontoAtaque == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pontoAtaque.position, raioAtaque);
    }
}