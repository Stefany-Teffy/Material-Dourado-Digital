using UnityEngine;

public class GerenciadorEstrelas : MonoBehaviour
{
    private int totalNiveis = 12;

    void Start()
    {
        for (int nivel = 1; nivel <= totalNiveis; nivel++)
        {
            if (!PlayerPrefs.HasKey("starDesNiveis" + nivel))
            {
                PlayerPrefs.SetInt("starDesNiveis" + nivel, 3);
            }
            Debug.Log("Verificando nível " + nivel);
        }
    }

    public void DefinirPorNivel(int nivel, int estDesativadas)
    {
        PlayerPrefs.SetInt("starDesNiveis" + nivel, estDesativadas);
    }

    public int ObterDoNivel(int nivel)
    {
        return PlayerPrefs.GetInt("starDesNiveis" + nivel);
    }

    public int ObterEstrelasDesativadasDoNivel(int nivel)
    {
        return ObterDoNivel(nivel);
    }

    public void RevalidarEstrelas()
    {
        EstrelaController[] estrelaControllers = FindObjectsOfType<EstrelaController>();

        foreach (EstrelaController estrelaController in estrelaControllers)
        {
            estrelaController.AtualizarEstadoEstrela();
        }
    }
}