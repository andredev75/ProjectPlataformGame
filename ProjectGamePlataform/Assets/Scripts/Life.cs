using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour
{
    public static Life instance;
    [Header("Vida")]
    public int vidaMaxima = 3;
    public int vidaAtual;

    private bool invencivel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaAtual = vidaMaxima;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceberDano(int dano)
    {
        if (invencivel)
        {
            return;
        }

        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            StartCoroutine(InvencibilidadeCurta());
        }
    }

    void Morrer()
    {
        Debug.Log("Player morreu");
    }

    IEnumerator InvencibilidadeCurta()
    {
        invencivel = true;
        yield return new WaitForSeconds(0.2f);
        invencivel = false;
    }
}
