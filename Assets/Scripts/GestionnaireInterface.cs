using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class GestionnaireInterface : MonoBehaviour
{
    public bool animation { get; set; }
    Toggle ToggleAnimation { get; set; }
    Button BoutonCommencerPartie { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI CompteurBateauxRestants { get; set; }
    TextMeshProUGUI CompteurTours { get; set; }
    TextMeshProUGUI Messages { get; set; }
    TextMeshProUGUI TitreFinDePartie { get; set; }

    void Start()
    {
        animation = true;
        AssignerVariables();
        AssignerCallback();
    }

    void AssignerCallback()
    {
        // Gestion tour 
        GestionnaireJeu.manager.TourChangé += IncrémenterTourUI;
        GestionnaireJeu.manager.TourChangé += RetirerTexte;

        // Gestion messages
        if (GestionnaireJeu.manager.DéterminerJoueurActif() == "Bot")
        {
            GestionnaireJeu.manager.JoueurActif.BateauDétruit += ÉcrireMessageTouchéCoulé;
            GestionnaireJeu.manager.JoueurActif.PartieTerminée += ÉcrireMessageVictoire;
            GestionnaireJeu.manager.JoueurActif.BateauDétruit += DécrémenterBateauxRestants;
        }
        else
        {
            GestionnaireJeu.manager.JoueurActif.PartieTerminée += ÉcrireMessageDéfaite;
        }
    }

    void AssignerVariables()
    {
        ToggleAnimation = GameObject.Find("Canvas").GetComponentsInChildren<Toggle>().First(x => x.name == "ToggleAnimation");
        ToggleAnimation.onValueChanged.AddListener(x => animation = !animation);

        // Bouton commencer
        BoutonCommencerPartie = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        BoutonCommencerPartie.onClick.AddListener(GestionnaireJeu.manager.CommencerPartie);

        // Bouton quitter
        BoutonQuitter = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        BoutonQuitter.onClick.AddListener(GestionnaireJeu.manager.QuitterPartie);

        // Compteur tours
        CompteurTours = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "CptToursINT");

        // Compteur bateaux restants
        CompteurBateauxRestants = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "BateauxRestantsINT");

        // Messages 
        Messages = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "MessagesTxt");

        // Titre de la scène fin de partie
        //AsyncOperation scene = SceneManager.LoadSceneAsync("FinDePartie", LoadSceneMode.Additive);
        //scene.allowSceneActivation = false;
        //TitreFinDePartie = SceneManager.GetSceneByName("FinDePartie").GetRootGameObjects().First(x => x.name == "Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "TitleTxt");
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

    void RetirerTexte(object sender, TourEventArgs e)
    {
        Messages.text = "";
    }

    void ÉcrireMessageDéfaite(object sender, BateauEventArgs e)
    {
        string message = "Vous avez perdu :(";
        TitreFinDePartie.text = message;
        Messages.text = message;
    }

    void ÉcrireMessageVictoire(object sender, BateauEventArgs e)
    {
        string message = "Vous avez gagné !";
        Messages.text = message;
        TitreFinDePartie.text = message;
    }

    void ÉcrireMessageTouchéCoulé(object sender, BateauEventArgs e)
    {
        Messages.text = "Touché coulé !";
    }

    void DécrémenterBateauxRestants(object sender, BateauEventArgs e)
    {
        CompteurBateauxRestants.text = GestionnaireJeu.manager.AutreJoueur.BateauxRestants.ToString();
    }
}
