using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionnaireBoutons : MonoBehaviour
{
    Vector3 VecteurAgrandi { get; set; }
    Color CouleurDefault { get; set; }
    Vector3 ScaleInitial { get; set; }

    void Start()
    {
        PlayerPrefs.SetString("scènePrécédente",SceneManager.GetActiveScene().name);
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

    public void ChangerScène(string nomScene) => SceneManager.LoadScene(nomScene);
}
