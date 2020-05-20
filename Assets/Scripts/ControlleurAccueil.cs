using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlleurAccueil : MonoBehaviour
{
    Button BoutonJouer { get; set; }
    Button BoutonQuitter { get; set; }

    void Start()
    {
        DéfinirValeursParDéfaut();
        AssignerCallbacks();
    }

    public void DéfinirValeursParDéfaut()
    {
        // Boutons
        BoutonJouer = GameObject.Find("CanvasAccueil").GetComponentsInChildren<Button>().First(x => x.name == "BtnJouer");
        BoutonQuitter = GameObject.Find("CanvasAccueil").GetComponentsInChildren<Button>().First(x => x.name == "BtnQuitter");
    }

    void AssignerCallbacks()
    {
        // Fonctions de rappel lors du clique sur les boutons
        BoutonJouer.onClick.AddListener(Jouer);
        BoutonQuitter.onClick.AddListener(Quitter);
    }

    void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    void Jouer()
    {
        SceneManager.LoadScene("GameScene");
    }
}
