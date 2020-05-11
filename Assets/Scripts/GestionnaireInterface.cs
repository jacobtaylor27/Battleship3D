using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestionnaireInterface : MonoBehaviour
{
    public bool AnimationEstActivée { get; set; }
    Toggle ToggleAnimation { get; set; }
    Button BoutonCommencerPartie { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI CompteurBateauxRestants { get; set; }
    TextMeshProUGUI CompteurTours { get; set; }
    TextMeshProUGUI Messages { get; set; }

    void Start()
    {
        AnimationEstActivée = true;
        AssignerVariables();
        AssignerCallbacks();
    }

    void AssignerVariables()
    {
        // Animation
        ToggleAnimation = GameObject.Find("Canvas").GetComponentsInChildren<Toggle>().First(x => x.name == "ToggleAnimation");
        ToggleAnimation.onValueChanged.AddListener(x => AnimationEstActivée = !AnimationEstActivée);

        // Bouton commencer
        BoutonCommencerPartie = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        BoutonCommencerPartie.onClick.AddListener(GestionnaireJeu.manager.CommencerPartie);

        // Bouton quitter
        BoutonQuitter = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnQuitter");
        BoutonQuitter.onClick.AddListener(GestionnaireJeu.manager.QuitterPartie);

        // Compteur tours
        CompteurTours = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "CptToursINT");

        // Compteur bateaux restants
        CompteurBateauxRestants = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "BateauxRestantsINT");

        // Messages 
        Messages = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "MessagesTxt");
    }

    void AssignerCallbacks()
    {
        GestionnaireJeu.manager.TourChangé += IncrémenterTourUI;
        GestionnaireJeu.manager.TourChangé+= RetirerTexte;

        GestionnaireJeu.manager.JoueurActif.BateauDétruit += ÉcrireMessageTouchéCoulé;
        GestionnaireJeu.manager.JoueurActif.BateauDétruit += DécrémenterBateauxRestants;
    }

    void IncrémenterTourUI(object sender, TourEventArgs e)
    {
        if (GestionnaireJeu.manager.Tour % 2 == 0)
        {
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Ordinateur)";
        }
        else if (GestionnaireJeu.manager.Tour % 2 == 1)
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Joueur)";
    }
    
    void RetirerTexte(object sender, TourEventArgs e) => Messages.text = "";

    void ÉcrireMessageTouchéCoulé(object sender, BateauEventArgs e) => Messages.text = "Touché coulé !";

    void DécrémenterBateauxRestants(object sender, BateauEventArgs e) => CompteurBateauxRestants.text = GestionnaireJeu.manager.AutreJoueur.BateauxRestants.ToString();
}
