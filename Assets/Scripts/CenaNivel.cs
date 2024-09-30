using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CenaNivel : MonoBehaviour
{
    void Start()
    {
        // Verifica e inicializa os valores necessários para cada nível
        for (int i = 1; i <= 12; i++)
        {
            if (!PlayerPrefs.HasKey("n" + i))
            {
                PlayerPrefs.SetInt("n" + i, 0);
            }

            if (!PlayerPrefs.HasKey("bloqueado" + i))
            {
                PlayerPrefs.SetInt("bloqueado" + i, 1); // Todos os níveis iniciam bloqueados
            }

            if (!PlayerPrefs.HasKey("starDesNiveis" + i))
            {
                PlayerPrefs.SetInt("starDesNiveis" + i, 3);
            }
            else
            {
                // Se já existir, não redefine para evitar resetar as estrelas
                Debug.Log("starDesNiveis" + i + " já está definido.");
            }

            GameObject nivelObj = FindRecursively("Nivel" + i, transform);

            if (nivelObj != null)
            {
                AtualizarEstadoBloqueio(nivelObj, i);

                // Desativa o botão do nível por padrão, exceto para o nível 1
                nivelObj.GetComponent<Button>().interactable = (i == 1);
            }
            else
            {
                Debug.LogError("Objeto 'Nivel" + i + "' não encontrado!");
            }
        }

        // Verifica e ativa botões dos níveis com base nas estrelas do nível anterior
        for (int i = 2; i <= 12; i++)
        {
            if (PlayerPrefs.GetInt("starDesNiveis" + (i - 1)) <= 2)
            {
                AtivarBotaoNivel(i);
            }
        }
    }

    public void Clicou(int nivel)
    {
        if (nivel >= 1 && nivel <= 12)
        {
            int estrelasNivelAnterior = nivel > 1 ? PlayerPrefs.GetInt("starDesNiveis" + (nivel - 1)) : 1;

            if (estrelasNivelAnterior <= 2)
            {
                ZerarEstrelasNivel(nivel);
                DesbloquearNivelAtual(nivel);
                PlayerPrefs.SetInt("nAtual", nivel);
                changeScenes.proxCena("MatDourado2");
            }
            else
            {
                Debug.Log("Nível " + (nivel - 1) + " precisa ter pelo menos uma estrela para desbloquear o Nível " + nivel + ".");
            }
        }
        else
        {
            Debug.Log("Nível " + nivel + " está bloqueado. Complete o nível anterior para desbloqueá-lo.");
        }
    }

    private void ZerarEstrelasNivel(int nivel)
    {
        GameObject nivelObj = FindRecursively("Nivel" + nivel, transform);
        if (nivelObj != null)
        {
            Transform estrelasTransform = nivelObj.transform.Find("Estrelas");
            if (estrelasTransform != null)
            {
                for (int j = 1; j <= 3; j++)
                {
                    Transform estrela = estrelasTransform.Find("estrela" + j);
                    if (estrela != null)
                    {
                        estrela.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Objeto 'Estrelas' não encontrado dentro do Nivel" + nivel + "!");
            }
        }
        else
        {
            Debug.LogError("Objeto 'Nivel" + nivel + "' não encontrado!");
        }
    }

    private void DesbloquearNivelAtual(int nivel)
    {
        PlayerPrefs.SetInt("bloqueado" + nivel, 0);
        PlayerPrefs.Save();

        GameObject nivelObj = FindRecursively("Nivel" + nivel, transform);
        if (nivelObj != null)
        {
            Transform bloqueadoTransform = nivelObj.transform.Find("Estrelas/bloqueado");

            if (bloqueadoTransform != null)
            {
                bloqueadoTransform.gameObject.SetActive(false);
                Debug.Log("Nível " + nivel + " desbloqueado!");
            }
            else
            {
                Debug.LogError("Objeto 'bloqueado' não encontrado dentro do Nivel" + nivel + "!");
            }

            // Agora habilita o botão do nível
            nivelObj.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.LogError("Objeto 'Nivel" + nivel + "' não encontrado!");
        }
    }

    private GameObject FindRecursively(string name, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject found = FindRecursively(name, child);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    private void AtualizarEstadoBloqueio(GameObject nivelObj, int nivel)
    {
        // Encontra o objeto "bloqueado" dentro do nível
        Transform bloqueadoTransform = nivelObj.transform.Find("Estrelas/bloqueado");
        if (bloqueadoTransform != null)
        {
            int estadoBloqueado = PlayerPrefs.GetInt("bloqueado" + nivel);
            bloqueadoTransform.gameObject.SetActive(estadoBloqueado == 1);
        }
        else
        {
            Debug.LogError("Objeto 'bloqueado' não encontrado dentro do Nivel" + nivel + "!");
        }

        // Desativa o botão se o nível estiver bloqueado
        nivelObj.GetComponent<Button>().interactable = (PlayerPrefs.GetInt("bloqueado" + nivel) == 0);
    }

    private void AtivarBotaoNivel(int nivel)
    {
        GameObject nivelObj = FindRecursively("Nivel" + nivel, transform);
        if (nivelObj != null)
        {
            nivelObj.GetComponent<Button>().interactable = true;
        }
        else
        {
            Debug.LogError("Objeto 'Nivel" + nivel + "' não encontrado!");
        }
    }
}
