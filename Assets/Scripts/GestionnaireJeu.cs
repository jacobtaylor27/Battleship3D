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
    private Bot Bot { get; set; }//xav:je le mets public car j'ai besoin daller checher son panneau tir dans Bot
    public Joueur JoueurActif { get; private set; }
    public Joueur AutreJoueur { get; private set; }
    Button BoutonGameStart { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get; set; }
    public int Tour { get; private set; }//xav:je le mets public car besoin dans bot // remettre private set
    private bool EstEnPhaseDeTirs { get { return Tour >= 2; } }
    Button BoutonTirerBot { get; set; }//test

    void Start()
    {

        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;

        Joueur.BateauDétruit += SignalerBot;
        //Bot.BateauDétruit += MéthodeQuelconque; // Afficher un message?

        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        //BoutonGameStart.onClick.AddListener(CommencerPartie); A GARDER
        BoutonGameStart.onClick.AddListener(CommencerPartie);//Test seulement

        BoutonTirerBot = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "TirerBot");//test
        BoutonTirerBot.onClick.AddListener(TirerJoueur);//test
    }


    //private void TirerBotTest()//test
    //{
    //    CoordVisée = Bot.Tirer();
    //    Debug.Log(Joueur.PaneauJeu.TrouverCase(CoordVisée).ToString());
    //    DéterminerRésultatTir();
    //    Bot.PaneauTirs.TrouverCase(CoordVisée).TypeOccupation = TypeOccupation.Touché;
    //}
    void Awake()
    {
        Tour = 0;//xav: pour test bot
        manager = this;

        Joueur = new Joueur();
        Bot = new Bot();

        JoueurActif = Bot;
        AutreJoueur = Joueur;
    }
    private void CommencerPartie()
    {
        Bot.Placer();
        GetComponent<PlacementBateau>().EnterState();
    }

    public void TirerJoueur()
    {
        GetComponent<GestionTirs>().EnterState();
    }
    private void LancerAnimationBot(object sender, OccupationEventArgs e)
    {
        GetComponent<GestionAnimation>().EnterState();
        //Debug.Log("animBot");
    }

    private void LancerAnimationJoueur(object sender, OccupationEventArgs e)
    {
        GetComponent<GestionAnimation>().EnterState();
        //Debug.Log("animJoueur");
    }

    public void DéterminerRésultatTir()
    {
        //Case CaseÀChanger = JoueurActif.PaneauJeu.TrouverCase(CoordVisée);
        TypeOccupation tempOccupation;
        if (AutreJoueur.PaneauJeu.TrouverCase(CoordVisée).EstOccupé)
        {
            if (AutreJoueur == Joueur)
                AutreJoueur.SeFaireTouché(TrouverBateauSurCase(AutreJoueur, CoordVisée));

            tempOccupation = TypeOccupation.Touché;
        }
        else
            tempOccupation = TypeOccupation.Manqué;
        JoueurActif.PaneauTirs.ModifierÉtatCase(CoordVisée, tempOccupation);
        PasserAuProchainTour();

    }

    public void PlacerBateauLogique(int indiceBateau, Vector3 orientation, Case caseVisée)
    {
        for (int i = 0; i < JoueurActif.Arsenal[indiceBateau].Longueur; i++)
        {
            Coordonnées coordOccupée = new Coordonnées(caseVisée.Coordonnées.Rangée + i * -(int)orientation.z, caseVisée.Coordonnées.Colonne + i * (int)orientation.x);
            JoueurActif.PaneauJeu.ModifierÉtatCase(coordOccupée, TypeOccupation.Occupé);
            JoueurActif.Arsenal[indiceBateau].EstPlacé = true;
            JoueurActif.Arsenal[indiceBateau].CasesOccupées.Add(JoueurActif.PaneauJeu.TrouverCase(coordOccupée));
        }

    }

    public void PasserAuProchainTour()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;
        Tour++;
        if (EstEnPhaseDeTirs)
        {
            CoordVisée = null;
            JoueurActif.Tirer();
        }
    }
    public void AfficherBat(Bateau b, List<Case> pu)
    {
        Instantiate(b.PrefabBateau, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    }

    public Bateau TrouverBateauSurCase(Joueur joueurTouché, Coordonnées coordVoulue)
    {
        Bateau bateauSurCase = new Bateau(2, null, null); // bateau null, car la fonction sera forcément appellée sur une case occupée.
        foreach (Bateau b in joueurTouché.Arsenal)
        {
            if (b.CasesOccupées.At(coordVoulue.Rangée, coordVoulue.Colonne) != null)
            {
                bateauSurCase = b;
                break;
            }
        }
        return bateauSurCase;
    }
    public void TrouverPositionViséeCase()
    {
        //À trouver avec InfoTuile, définir une coordonnée à Case?
    }
    private void SignalerBot(object sender, BateauEventArgs e)
    {
        Bot.dernierTirCouler = true;
    }
}