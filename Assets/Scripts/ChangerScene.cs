using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangerScene : MonoBehaviour
{
    List<string> HistoriqueScene { get; set; }

    void Start()
    {
        HistoriqueScene = new List<string>();
        DontDestroyOnLoad(gameObject);
    }

    public void ChangerDeScene(string nouvelleScene)
    {
        HistoriqueScene.Add(nouvelleScene);
        SceneManager.LoadScene(nouvelleScene);
    }

    public void ChangerScenePrecedente()
    {
        HistoriqueScene.RemoveAt(HistoriqueScene.Count - 1);
        SceneManager.LoadScene(HistoriqueScene[HistoriqueScene.Count - 1]);
    }

}