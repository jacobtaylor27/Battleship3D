using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GestionnaireBoutons : MonoBehaviour
{
    Vector3 VecteurAgrandi { get; set; }
    Color CouleurDefault { get; set; }
    Vector3 ScaleInitial { get; set; }
    List<string> Scènes { get; set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Scènes = new List<string>();
        VecteurAgrandi = new Vector3(3.5f, 3.5f, 3.5f);
        ScaleInitial = transform.localScale;
        CouleurDefault = GetComponent<Image>().color;
    }

    public void AgrandirBouton() => transform.localScale = VecteurAgrandi;

    public void RétrécirBouton() => transform.localScale = ScaleInitial;

    public void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    public void ChangerCouleurVert() => GetComponent<Image>().color = Color.green;

    public void ChangerCouleurDefault() => GetComponent<Image>().color = CouleurDefault;

    public void ChangerScenePrecedente()
    {
        Scènes.RemoveAt(Scènes.Count - 1);
        SceneManager.LoadScene(Scènes[Scènes.Count - 1]);
    }

    public void ChangerScène(string nomScene)
    {
        Scènes.Add(nomScene);
        SceneManager.LoadScene(nomScene);
    }
}
