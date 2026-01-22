using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float velocidade = 5f;
    public float forcaPulo = 7f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    public bool noChao;
    public Transform verificadorDeChao;
    public float raioDeVerificacao;
    public LayerMask layerDoChao;

    public float tempoAtaque = 0.3f;
    private bool atacando;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        Mover();
        Pular();
        Atacar();
    }

    void Mover()
    {
        if (atacando == true) 
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");

        Vector2 pos = transform.position;
        pos.x += h * velocidade * Time.deltaTime;
        transform.position = pos;

        // vira para o lado certo
        if (h > 0) sr.flipX = false;
        if (h < 0) sr.flipX = true;

        if (noChao == true && h != 0)
        {
            anim.Play("player_run");
        }
        else if (noChao == true && h == 0)
        {
            anim.Play("player_idle");
        }
        else
        {
            anim.Play("player_jump");
        }

    }

    void Pular()
    {
        noChao = Physics2D.OverlapCircle(verificadorDeChao.position, raioDeVerificacao, layerDoChao);

        if (Input.GetKeyDown(KeyCode.Space) && noChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            anim.Play("player_jump");
        }
    }

    void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.J) && !atacando)
        {
            StartCoroutine(AtaqueCoroutine());
        }
    }

    IEnumerator AtaqueCoroutine()
    {
        atacando = true;

        float h = Input.GetAxisRaw("Horizontal");

        if (noChao && h != 0)
        {
            anim.Play("player_attack_run");
        }
        else
        {
            anim.Play("player_attack_idle");
        }

    yield return new WaitForSeconds(tempoAtaque);

    atacando = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(verificadorDeChao.position, raioDeVerificacao);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("portal"))
        {
            //GameManager.instance.MostrarTelaDeVitoria();
        }
    }

}

