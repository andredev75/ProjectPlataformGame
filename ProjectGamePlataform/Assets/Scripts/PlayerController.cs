using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private float gravidadePadrao = 3f;
    public float velocidade = 5f;

    [Header("Pulo")]
    public float forcaPulo = 7f;
    public bool noChao;
    public Transform verificadorDeChao;
    public float raioDeVerificacao;
    public LayerMask layerDoChao;
    [Header("Gravidade Dinâmica")]
    public float multiplicadorQueda = 2.5f;
    public float multiplicadorPuloCurto = 2f;
    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeContador;
    [Header("Jump Buffer")]
    public float jumpBufferTempo = 0.15f;
    private float jumpBufferContador;

    [Header("Ataque")]
    public float tempoAtaque = 0.3f;
    private bool atacando;

    [Header("Dash")]
    public float dashForca = 15f;
    public float dashDuracao = 0.2f;
    public float dashCooldown = 0.5f;
    private bool dashando;
    private bool podeDashar = true;

    [Header("After Image")]
    public GameObject afterImagePrefab;
    public float intervaloAfterImage = 0.05f;


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
        Dash();
        GravidadeDinamica();
    }

    void Mover()
    {
        
        if (atacando == true || dashando == true) 
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
        if (dashando)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferContador = jumpBufferTempo;
        }
        else
        {
            jumpBufferContador -= Time.deltaTime;
        }

        noChao = Physics2D.OverlapCircle(verificadorDeChao.position, raioDeVerificacao, layerDoChao);
        
        if (noChao)
        {
            coyoteTimeContador = coyoteTime;
        }
        else
        {
            coyoteTimeContador -= Time.deltaTime;
        }
        
        if (jumpBufferContador > 0 && coyoteTimeContador > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            anim.Play("player_jump");
            coyoteTimeContador = 0;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.K) && podeDashar && !dashando && !atacando)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        podeDashar = false;
        dashando = true;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 direcao = new Vector2(h, v);

        // Se estiver parado, dash na direção que o personagem está olhando
        if (direcao == Vector2.zero)
        {
            direcao = sr.flipX ? Vector2.left : Vector2.right;
        }

        direcao.Normalize();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.linearVelocity = direcao * dashForca;

        anim.Play("player_dash");

        float tempo = 0f;

        // 🔥 AFTER IMAGE DURANTE O DASH
        while (tempo < dashDuracao)
        {
            CriarAfterImage();
            tempo += intervaloAfterImage;
            yield return new WaitForSeconds(intervaloAfterImage);
        }

        rb.gravityScale = gravidadePadrao;
        rb.linearVelocity = Vector2.zero;

        dashando = false;

        yield return new WaitForSeconds(dashCooldown);
        podeDashar = true;
    }

    void CriarAfterImage()
    {
        GameObject img = Instantiate(afterImagePrefab, transform.position, transform.rotation);

        SpriteRenderer imgSR = img.GetComponent<SpriteRenderer>();
        imgSR.sprite = sr.sprite;
        imgSR.flipX = sr.flipX;
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

    void GravidadeDinamica()
    {
        if (dashando)
        {
            return;
        }
        
        // Se estiver caindo
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravidadePadrao * multiplicadorQueda;
        }
        // Se estiver subindo mas soltou o botão de pulo
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = gravidadePadrao * multiplicadorPuloCurto;
        }
        else
        {
            rb.gravityScale = gravidadePadrao;
        }
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

