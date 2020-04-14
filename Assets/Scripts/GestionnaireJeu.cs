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
    KeyCode Placer { get; set; }
    KeyCode Tourner { get; set; }
    Button BoutonGameStart { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get; set; }
    bool Fait { get; set; }
    public int Tour { get; set; }//xav:je le mets public car besoin dans bot // remettre private set

    Button BoutonTirerBot { get; set; }//test

    void Start()
    {

        Placer = KeyCode.Mouse0; // CLICK GAUCHE
        Tourner = KeyCode.R;

        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;
        //Joueur.BateauDétruit += MéthodeQuelconque; //Trigger un event pour dire que le bateau est détruit, ex: un message s'affiche?
        //Bot.BateauDétruit += MéthodeQuelconque;

        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        //BoutonGameStart.onClick.AddListener(CommencerPartie); A GARDER
        BoutonGameStart.onClick.AddListener(CommencerPartie);//Test seulement

        BoutonTirerBot = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "TirerBot");//test
        BoutonTirerBot.onClick.AddListener(TirerBotTest);//test
    }

    private void TirerBotTest()//test
    {
        CoordVisée = Bot.Tirer();
        Debug.Log(Joueur.PaneauJeu.TrouverCase(CoordVisée).ToString());
        DéterminerRésultatTir();
        Bot.PaneauTirs.TrouverCase(CoordVisée).TypeOccupation = TypeOccupation.Touché;
    }
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
        //Bot.Placer(); // Bot devra appeler NextPlayer()
        NextPlayer();
        GetComponent<PlacementBateau>().EnterState();
    }

    private void CommencerPhaseTirs()
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
            tempOccupation = TypeOccupation.Touché;
        // Modifier l'état bateau touché ici?
        else
            tempOccupation = TypeOccupation.Manqué;
        JoueurActif.PaneauTirs.ModifierÉtatCase(CoordVisée, tempOccupation);
    }

    public void UpdateOccupation(int indiceBateau, Vector3 orientation, Case caseVisée)
    {
        for (int i = 0; i < JoueurActif.Arsenal[indiceBateau].Longueur; i++)
        {
            Coordonnées coordOccupée = new Coordonnées(caseVisée.Coordonnées.Rangée + i * (int)orientation.z, caseVisée.Coordonnées.Colonne + i * (int)orientation.x);
            JoueurActif.PaneauJeu.ModifierÉtatCase(coordOccupée, TypeOccupation.Occupé);
            JoueurActif.Arsenal[indiceBateau].CasesOccupées.Add(JoueurActif.PaneauJeu.TrouverCase(coordOccupée));
        }

    }

    public void NextPlayer()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;
        Tour++;
    }




}