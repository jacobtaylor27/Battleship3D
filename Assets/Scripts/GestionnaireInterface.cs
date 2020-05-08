using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GestionnaireInterface : MonoBehaviour
{
    Button BoutonCommencerPartie { get; set; }
    Button BoutonQuitter { get; set; }
    TextMeshProUGUI CompteurBateauxRestants { get; set; }
    TextMeshProUGUI CompteurTours { get; set; }
    TextMeshProUGUI Messages { get; set; }

    void Start()
    {
        AssignerVariables();
    }

    void AssignerVariables()
    {
        // Bouton start
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
        Messages = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "TouchéCouléTxt");
    }

    void Update()
    {
        
    }
}
