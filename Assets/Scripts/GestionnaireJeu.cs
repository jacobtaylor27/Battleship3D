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
    TextMeshProUGUI CptBateauxRestants { get; set; }
    TextMeshProUGUI CptTourUI { get; set; }
    TextMeshProUGUI TexteMessages { get; set; }
    private bool EstEnPhaseDeTirs { get { return Tour >= 2; } }
    public EventHandler<TourEventArgs> TourChangé;

    void onTourChangé(TourEventArgs dataTour) => TourChangé?.Invoke(this, dataTour);

    void Start()
    {
        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");

        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;
        Joueur.PaneauTirs.OccupationModifiée += RetirerCollider;

        Joueur.BateauDétruit += SignalerBot;
        Bot.BateauDétruit += AfficherBateau;
        JoueurActif.PartieTerminée += TerminerJeu;

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

    #region Callbacks
    public void CommencerPartie()
    {
        ModifierBoutonStart();
        Bot.Placer();
        GetComponent<PlacementBateau>().EnterState();
    }

    public void QuitterPartie()
    {
        UnityEngine.Application.Quit();
    }

    public void TerminerJeu(object sender, BateauEventArgs e)
    {
        SceneManager.LoadScene("FinDePartie");
        SceneManager.UnloadSceneAsync("GameScene");
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

    void AfficherBateau(object sender, BateauEventArgs e)
    {
        // 0 = horizontal
        // 1 = vertical
        int direction = 0;

        Bateau bateauCouler = TrouverBateauSurCase(AutreJoueur, CoordVisée);
        if (bateauCouler.CasesOccupées[0].Coordonnées.Rangée == bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count - 1].Coordonnées.Rangée)
        {
            direction = 1;
        }

        if (direction == 0)
        {
            if (bateauCouler.CasesOccupées[0].Coordonnées.Rangée > bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count - 1].Coordonnées.Rangée)
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation * Quaternion.Euler(0f, 90f, 0f));
            else
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation * Quaternion.Euler(0f, -90, 0f));
        }

        if (direction == 1)
        {
            if (bateauCouler.CasesOccupées[0].Coordonnées.Colonne > bateauCouler.CasesOccupées[bateauCouler.CasesOccupées.Count - 1].Coordonnées.Colonne)
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation);
            else
                Instantiate(bateauCouler.PrefabBateau, bateauCouler.CasesOccupées[0].PositionMonde, bateauCouler.PrefabBateau.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }
    }

    void SignalerBot(object sender, BateauEventArgs e)
    {
        Bot.dernierTirCoulé = true;
    }
    #endregion

    #region Méthodes
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
        // Bateau null, car la fonction sera forcément appellée sur une case occupée.
        Bateau bateauSurCase = new Bateau(2, null, null); 
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

    void InverserJoueursEtCanons()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;

        GameObject tempCanon = CanonActif;
        CanonActif = AutreCanon;
        AutreCanon = tempCanon;
    }

    void ModifierCouleur()
    {
        List<InformationTuile> infoTuile = GameObject.Find("ListeTuiles").GetComponentsInChildren<InformationTuile>().ToList();

        if (OccupÀCoordVisée == TypeOccupation.Touché)
            infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/Touché");
        else if (OccupÀCoordVisée == TypeOccupation.Manqué)
            infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");
    }

    public string DéterminerJoueurActif() => JoueurActif.ToString();

    void RechargerScène() => SceneManager.LoadScene("GameScene");

    private void ModifierBoutonStart()
    {
        BoutonGameStart.onClick.RemoveAllListeners();
        BoutonGameStart.GetComponentInChildren<Text>().text = "Recommencer";
        BoutonGameStart.GetComponentInChildren<Text>().fontSize = 23;
        BoutonGameStart.onClick.AddListener(RechargerScène);
    }
    #endregion
}