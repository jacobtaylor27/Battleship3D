using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GestionnaireInterface : MonoBehaviour
{
    Button BoutonCommencerPartie { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI CompteurBateauxRestants { get; set; }
    TextMeshProUGUI CompteurTours { get; set; }
    TextMeshProUGUI Messages { get; set; }
    EventHandler<TourEventArgs> TourChangé;

    void onTourChangé(TourEventArgs dataTour) => TourChangé?.Invoke(this, dataTour);

    void Start()
    {
        AssignerVariables();
        AssignerCallback();
    }

    void AssignerCallback()
    {
        // Tour 
        TourChangé += IncrémenterTourUI;
        TourChangé += RetirerTexte;

        // Messages
        GestionnaireJeu.manager.JoueurActif.BateauDétruit += ÉcrireMessageTouchéCoulé;
        GestionnaireJeu.manager.JoueurActif.PartieTerminée += ÉcrireMessageVictoire;
    }

    void AssignerVariables()
    {
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
    }

    void IncrémenterTourUI(object sender, TourEventArgs e)
    {
        if (GestionnaireJeu.manager.Tour % 2 == 0)
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Ordinateur)";
        else if (GestionnaireJeu.manager.Tour % 2 == 1)
            CompteurTours.text = GestionnaireJeu.manager.Tour.ToString() + " (Joueur)";
    }

    void RetirerTexte(object sender, TourEventArgs e)
    {
        Messages.text = "";
    }

    void ÉcrireMessageDéfaite(object sender, BateauEventArgs e)
    {
        Messages.text = "Vous avez perdu la partie :(";
    }

    void ÉcrireMessageVictoire(object sender, BateauEventArgs e)
    {
        Messages.text = "Vous avez gagné la partie !";
    }

    void ÉcrireMessageTouchéCoulé(object sender, BateauEventArgs e)
    {
        Messages.text = "Touché coulé !";
    }
}
