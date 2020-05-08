using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;

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
    Button Button { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get; set; }
    public TypeOccupation OccupÀCoordVisée { get; private set; }
    public int Tour { get; private set; }
    public EventHandler<TourEventArgs> TourChangé;
    TextMeshProUGUI CptBateauxRestants { get; set; }
    TextMeshProUGUI CptTourUI { get; set; }
    TextMeshProUGUI TexteMessages { get; set; }
    private bool EstEnPhaseDeTirs { get { return Tour >= 2; } }

    void Start()
    {
        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;
        Joueur.PaneauTirs.OccupationModifiée += RetirerCollider;
        
        Joueur.BateauDétruit += SignalerBot;
        Bot.BateauDétruit += AfficherBateau; // Afficher un message?

        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        BoutonGameStart.onClick.AddListener(CommencerPartie);
        

        CptTourUI = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "CptToursINT");
        TourChangé += IncrémenterTourUI;
        TourChangé += RetirerTexte;

        

        CptBateauxRestants = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "BateauxRestantsINT");
        TexteMessages = GameObject.Find("Canvas").GetComponentsInChildren<TextMeshProUGUI>().First(x => x.name == "TouchéCouléTxt");
        Bot.BateauDétruit += IncrémenterBateauxRestantsUI;
        Bot.BateauDétruit += ÉcrireMessage;
        Joueur.PartieTerminée += ÉcrireMessageFinPartieBot;
        Joueur.PartieTerminée += TerminerJeu;

        GameObject[] Canons = GameObject.FindGameObjectsWithTag("Canon");

        CanonBot = GameObject.Find("NPCCanon").GetComponentsInChildren<Transform>()[1].gameObject;
        CanonJoueur = GameObject.Find("PlayerCanon").GetComponentsInChildren<Transform>()[1].gameObject;

        CanonActif = CanonBot;
        AutreCanon = CanonJoueur;
        //foreach (Bateau b in Bot.Arsenal)
        //{
        //    foreach (Case c in b.CasesOccupées)
        //    {

        //        Debug.Log(" DEBUG "+c.Coordonnées);

        //    }
        //} 
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

    public void CommencerPartie()
    {
        BoutonGameStart.onClick.RemoveAllListeners();
        Bot.Placer();
        GetComponent<PlacementBateau>().EnterState();
    }

    public void QuitterPartie()
    {
        UnityEngine.Application.Quit();
    }

    void onTourChangé(TourEventArgs dataTour) => TourChangé?.Invoke(this, dataTour);

    public void IncrémenterTourUI(object sender, TourEventArgs e)
    {
        if (Tour % 2 == 0)
            CptTourUI.text = Tour.ToString() + " (Ordinateur)";
        else if (Tour % 2 == 1)
            CptTourUI.text = Tour.ToString() + " (Joueur)";
    }

    void IncrémenterBateauxRestantsUI(object sender, BateauEventArgs e)
    {
        CptBateauxRestants.text = Bot.BateauxRestants.ToString();
    }

    void ÉcrireMessage(object sender, BateauEventArgs e)
    {
        if (!Bot.Arsenal.All(x => x.EstCoulé))
            TexteMessages.text = "Touché coulé !";
        else if (Bot.Arsenal.All(x => x.EstCoulé))
            TexteMessages.text = "Vous avez gagné la partie !";        
    }                                                          
    void RetirerTexte(object sender, TourEventArgs e)
    {
        TexteMessages.text = "";
    }                                                          
    void ÉcrireMessageFinPartieBot(object sender, BateauEventArgs e)
    {
        TexteMessages.text = "Vous avez perdu la partie :(";
    }                                                          
    void TerminerJeu(object sender, BateauEventArgs e)
    {
        SceneManager.LoadScene("FinDePartie");
        SceneManager.UnloadSceneAsync("GameScene");
        //if((Joueur)sender == Bot)
           
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
    public void RetirerCollider(object sender, OccupationEventArgs e)
    {
        List<InformationTuile> infoTuile = GameObject.Find("ListeTuiles").GetComponentsInChildren<InformationTuile>().ToList();
        Destroy(infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<BoxCollider>());
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

        ModifierCouleur();

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
        onTourChangé(new TourEventArgs(Tour));
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
    private void AfficherBateau(object sender, BateauEventArgs e)
    {
        int direction = 0;//0:horizontal
                          //1:vertical
        Bateau bateauCouler = TrouverBateauSurCase(AutreJoueur, CoordVisée);
        if (bateauCouler.CasesOccupées[0].Coordonnées.Rangée == bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count-1].Coordonnées.Rangée)
        {
            direction = 1;
        }
        if(direction == 0)
        {
            if (bateauCouler.CasesOccupées[0].Coordonnées.Rangée > bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count - 1].Coordonnées.Rangée)
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation *Quaternion.Euler(0f,90f,0f));
            else
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation * Quaternion.Euler(0f, -90, 0f));
        }
        if (direction == 1)
        {
            if(bateauCouler.CasesOccupées[0].Coordonnées.Colonne > bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count - 1].Coordonnées.Colonne)
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation);
            else
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }

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
    private void ModifierCouleur()
    {
        List<InformationTuile> infoTuile = GameObject.Find("ListeTuiles").GetComponentsInChildren<InformationTuile>().ToList();

        if (OccupÀCoordVisée == TypeOccupation.Touché)
            infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/Touché");
        else if (OccupÀCoordVisée == TypeOccupation.Manqué)
            infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");
    }

}