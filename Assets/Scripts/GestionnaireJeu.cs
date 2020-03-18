using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;

public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu manager;
    private Joueur Joueur { get; set; }
    private Bot Bot { get; set; }
    Joueur JoueurActif { get; set; }
    Joueur AutreJoueur { get; set; }
    KeyCode Placer { get; set; }
    KeyCode Tourner { get; set; }
    Button BoutonGameStart { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get;set; }
    bool Fait { get; set; }
    int Tour { get; set; }


    void Start()
    {
        Joueur = new Joueur();
        Bot = new Bot();

        JoueurActif = Bot;
        AutreJoueur = Joueur;

        Placer = KeyCode.Mouse0; // CLICK GAUCHE
        Tourner = KeyCode.R;

        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;

        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        //BoutonGameStart.onClick.AddListener(CommencerPartie); A GARDER
        BoutonGameStart.onClick.AddListener(CommencerPhaseTirs);//Test seulement

    }
    void Awake()
    {
        manager = this;
    } 
    private void CommencerPartie()
    {
        Bot.Placer(); // Bot devra appeler NextPlayer()
        GetComponent<PlacementBateau>().EnterState();// ExitState() devra appeler NextPlayer()
    }

    private void CommencerPhaseTirs()
    {
        GetComponent<GestionTirs>().EnterState();
    }
    private void LancerAnimationBot(object sender, OccupationEventArgs e)
    {
        Debug.Log("Animation bot lancée");
    }

    private void LancerAnimationJoueur(object sender, OccupationEventArgs e)
    {
        Debug.Log("Animation joueur Lancée");
    }

    public void DéterminerRésultatTir()
    {
        //Case CaseÀChanger = JoueurActif.PaneauJeu.TrouverCase(CoordVisée);
        TypeOccupation tempOccupation;
        if (AutreJoueur.PaneauJeu.TrouverCase(CoordVisée).EstOccupé)
            tempOccupation = TypeOccupation.Touché;
            // Modifier l'état bateau touché ici?
        else
            tempOccupation = TypeOccupation.Manqué;
        JoueurActif.PaneauTirs.ModifierÉtatCase(CoordVisée, tempOccupation);
    }

    public void NextPlayer()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;
    }
    

}