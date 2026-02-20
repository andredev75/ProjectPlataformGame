using UnityEngine;
using System.Collections;


public class LifeInimigos : MonoBehaviour
{
    [Header("Vida Inimigos")]
    public int vidaMaxima = 3;
    public int vidaAtual;

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
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log(gameObject.name + " morreu");

        Destroy(gameObject);
    }
}
