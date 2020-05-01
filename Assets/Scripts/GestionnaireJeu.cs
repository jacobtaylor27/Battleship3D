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
    public Joueur JoueurActif { get; private set; }
    public Joueur AutreJoueur { get; private set; }
    private GameObject CanonJoueur { get; set; }
    private GameObject CanonBot { get; set; }
    public GameObject CanonActif { get; private set; }
    public GameObject AutreCanon { get; private set; }
    Button BoutonGameStart { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get; set; }
    public TypeOccupation OccupÀCoordVisée { get; private set; }
    public int Tour { get; private set; }
    private bool EstEnPhaseDeTirs { get { return Tour >= 2; } }

    void Start()
    {

        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;

        Joueur.BateauDétruit += SignalerBot;
        //Bot.BateauDétruit += MéthodeQuelconque; // Afficher un message?

        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        BoutonGameStart.onClick.AddListener(CommencerPartie);


        GameObject[] Canons = GameObject.FindGameObjectsWithTag("Canon");

        CanonBot = GameObject.Find("NPCCanon").GetComponentsInChildren<Transform>()[1].gameObject;
        CanonJoueur = GameObject.Find("PlayerCanon").GetComponentsInChildren<Transform>()[1].gameObject;

        CanonActif = CanonBot;
        AutreCanon = CanonJoueur;
    }

    void Awake()
    {
        Tour = 0;
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
    }

    private void LancerAnimationJoueur(object sender, OccupationEventArgs e)
    {
        GetComponent<GestionAnimation>().EnterState();
    }

    public void DéterminerRésultatTir()
    {
        if (AutreJoueur.PaneauJeu.TrouverCase(CoordVisée).EstOccupé)
        {
            AutreJoueur.SeFaireToucher(TrouverBateauSurCase(AutreJoueur, CoordVisée));

            OccupÀCoordVisée = TypeOccupation.Touché;
        }
        else
            OccupÀCoordVisée = TypeOccupation.Manqué;
        JoueurActif.PaneauTirs.ModifierÉtatCase(CoordVisée, OccupÀCoordVisée);

        GestionnaireCouleur.ModifierCouleur();
        
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
        InverserJoueursEtCanons();
        Tour++;
        if (EstEnPhaseDeTirs)
        {
            CoordVisée = null;
            JoueurActif.Tirer();
        }
    }
    

    public Bateau TrouverBateauSurCase(Joueur joueurTouché, Coordonnées coordVoulue)
    {
        Bateau bateauSurCase = new Bateau(2, null, null); // bateau null, car la fonction sera forcément appellée sur une case occupée.
        foreach (Bateau b in joueurTouché.Arsenal)
        {
            Case temp = b.CasesOccupées.At(coordVoulue.Rangée, coordVoulue.Colonne);
            if (b.CasesOccupées.At(coordVoulue.Rangée, coordVoulue.Colonne) != null)
            {
                bateauSurCase = b;
                break;
            }
        }
        return bateauSurCase;
    }
    
    private void SignalerBot(object sender, BateauEventArgs e)
    {
        Bot.dernierTirCoulé = true;
    }

    private void InverserJoueursEtCanons()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;

        GameObject tempCanon = CanonActif;
        CanonActif = AutreCanon;
        AutreCanon = tempCanon;
    }
    
}