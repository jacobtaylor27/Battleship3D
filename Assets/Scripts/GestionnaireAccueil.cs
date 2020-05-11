using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionnaireAccueil : MonoBehaviour
{
    Button BoutonJouer { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI TexteBoutonJouer { get; set; }
    TextMeshProUGUI TexteTitre { get; set; }

    void Start()
    {
        DéfinirValeursParDéfaut();
        AssignerCallbacks();
        GarderObjets();
    }

    public void DéfinirValeursParDéfaut()
    {
        // Boutons
        BoutonJouer = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnJouer");
        BoutonQuitter = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnQuitter");

        // Texte
        TexteBoutonJouer = BoutonJouer.GetComponentInChildren<TextMeshProUGUI>();
        TexteTitre = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "TxtTitre");
    }

    void AssignerCallbacks()
    {
        // Boutons
        BoutonJouer.onClick.AddListener(Jouer);
        BoutonQuitter.onClick.AddListener(Quitter);

        // Texte
        GestionnaireJeu.manager.JoueurActif.PartieTerminée += ModifierBoutonJouer;
        if (GestionnaireJeu.manager.DéterminerJoueurActif() == "Bot")
        {
            GestionnaireJeu.manager.JoueurActif.PartieTerminée += ÉcrireVictoire;
        }
        else
        {
            GestionnaireJeu.manager.JoueurActif.PartieTerminée += ÉcrireDéfaite;
        }
    }

    void ModifierBoutonJouer(object sender, BateauEventArgs e)
    {
        TexteBoutonJouer.text = "Rejouer";
    }

    void ÉcrireVictoire(object sender, BateauEventArgs e)
    {
        TexteTitre.text = "Vous avez gagné !";
    }

    void ÉcrireDéfaite(object sender, BateauEventArgs e)
    {
        TexteTitre.text = "Vous avez perdu :(";
    }

    void GarderObjets()
    {
        DontDestroyOnLoad(BoutonJouer);
        DontDestroyOnLoad(BoutonQuitter);
        DontDestroyOnLoad(TexteTitre);
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

    void Jouer() => SceneManager.LoadScene("GameScene");
}
