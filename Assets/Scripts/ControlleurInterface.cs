using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlleurInterface : MonoBehaviour
{
    public bool AnimationEstActivée { get; private set; }
    Toggle ToggleAnimation { get; set; }
    Button BoutonCommencerPartie { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI CompteurBateauxRestants { get; set; }
    TextMeshProUGUI CompteurTours { get; set; }
    TextMeshProUGUI Messages { get; set; }
    GameObject Canvas { get; set; }
    TextMeshProUGUI[] ÉlémentsTexte { get; set; }
    Toggle[] ÉlémentsToggle { get; set; }
    Button[] ÉlémentsBouton { get; set; }

    void Start()
    {
        AssignerValeursInitiales();
        AssignerFonctionsDeRappel();
    }

    void AssignerFonctionsDeRappel()
    {
        // Fonctions de rappel quand le tour change
        GestionnaireJeu.manager.TourChangé += IncrémenterToursInterface;

        // Fonctions de rappel quand un bateau est détruit
        GestionnaireJeu.manager.JoueurActif.BateauDétruit += ÉcrireTouchéCoulé;
        GestionnaireJeu.manager.JoueurActif.BateauDétruit += DécrémenterBateauxRestants;
    }

    void IncrémenterToursInterface(object sender, TourEventArgs e)
    {
        if (GestionnaireJeu.manager.Tour % 2 == 0)
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Ordinateur)";

        else if (GestionnaireJeu.manager.Tour % 2 == 1)
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Joueur)";
    }

    void AssignerValeursInitiales()
    {
        Canvas = GameObject.Find("Canvas");
        ÉlémentsTexte = Canvas.GetComponentsInChildren<TextMeshProUGUI>();
        ÉlémentsBouton = Canvas.GetComponentsInChildren<Button>();
        ÉlémentsToggle = Canvas.GetComponentsInChildren<Toggle>();

        ToggleAnimation = ÉlémentsToggle.First(x => x.name == "ToggleAnimation");
        ToggleAnimation.onValueChanged.AddListener(x => AnimationEstActivée = !AnimationEstActivée);
        AnimationEstActivée = true;

        BoutonCommencerPartie = ÉlémentsBouton.First(x => x.name == "BtnCommencer");
        BoutonCommencerPartie.onClick.AddListener(GestionnaireJeu.manager.CommencerPartie);
        
        BoutonQuitter = ÉlémentsBouton.First(x => x.name == "BtnQuitter");
        BoutonQuitter.onClick.AddListener(GestionnaireJeu.manager.QuitterPartie);

        CompteurTours = ÉlémentsTexte.First(x => x.name == "ValTour");
        CompteurBateauxRestants = ÉlémentsTexte.First(x => x.name == "ValBateaux");
        
        Messages = ÉlémentsTexte.First(x => x.name == "MessagesTxt");
    }

    IEnumerator RetirerTexte()
    {
        yield return new WaitForSecondsRealtime(2);
        Messages.text = "";
    }

    void ÉcrireTouchéCoulé(object sender, BateauEventArgs e)
    {
        Messages.text = "Touché coulé !";
        StartCoroutine(RetirerTexte());
    }

    void DécrémenterBateauxRestants(object sender, BateauEventArgs e)
    {
        CompteurBateauxRestants.text = GestionnaireJeu.manager.AutreJoueur.BateauxRestants.ToString();
    }
}
