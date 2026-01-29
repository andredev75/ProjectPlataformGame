using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float tempoVida = 0.25f;
    public float alphaInicial = 0.3f;
    private SpriteRenderer sr;
    private float tempoAtual;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tempoAtual = tempoVida;

        // Define transparência inicial
        Color cor = sr.color;
        cor.a = alphaInicial;
        sr.color = cor;
    }

    void Update()
    {
        tempoAtual -= Time.deltaTime;

        float alpha = Mathf.Lerp(0f, alphaInicial, tempoAtual / tempoVida);

        Color cor = sr.color;
        cor.a = alpha;
        sr.color = cor;

        if (tempoAtual <= 0)
        {
            Destroy(gameObject);
        }
    }
}
