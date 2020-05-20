using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu manager;

    Joueur Joueur { get; set; }
    Bot Bot { get; set; }
    public Joueur JoueurActif { get; private set; }
    public Joueur AutreJoueur { get; private set; }

    GameObject CanonJoueur { get; set; }
    GameObject CanonBot { get; set; }
    public GameObject CanonActif { get; private set; }
    public GameObject AutreCanon { get; private set; }

    Button BoutonGameStart { get; set; }

    public Vector3 PositionVisée { get; set; }
    public Coordonnées CoordVisée { get; set; }
    public TypeOccupation OccupÀCoordVisée { get; private set; }

    public int Tour { get; private set; }
    bool EstEnPhaseDeTirs { get { return Tour >= 2; } }

    public EventHandler<TourEventArgs> TourChangé;

    void onTourChangé(TourEventArgs dataTour) => TourChangé?.Invoke(this, dataTour);

    void Start()
    {
        AssignerValeursInitiales();
        AssignerFonctionsDeRappel();
    }

    void AssignerValeursInitiales()
    {
        GameObject[] Canons = GameObject.FindGameObjectsWithTag("Canon");

        CanonBot = GameObject.Find("NPCCanon").GetComponentsInChildren<Transform>()[1].gameObject;
        
        CanonJoueur = GameObject.Find("PlayerCanon").GetComponentsInChildren<Transform>()[1].gameObject;
        
        BoutonGameStart = GameObject.Find("Canvas").GetComponentsInChildren<Button>().First(x => x.name == "BtnCommencer");
        
        CanonActif = CanonBot;
        
        AutreCanon = CanonJoueur;
    }

    void AssignerFonctionsDeRappel()
    {
        Joueur.PaneauTirs.OccupationModifiée += LancerAnimationJoueur;
        Bot.PaneauTirs.OccupationModifiée += LancerAnimationBot;
        Joueur.PaneauTirs.OccupationModifiée += RetirerCollider;
        Joueur.BateauDétruit += SignalerBot;
        Bot.BateauDétruit += AfficherBateau;
        JoueurActif.PartieTerminée += AfficherÉcranVictoire;
        AutreJoueur.PartieTerminée += AfficherÉcranDéfaite;
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

    #region Fonctions de Rappel
    public void CommencerPartie()
    {
        ModifierBoutonStart();
        Bot.Placer();
        GetComponent<GestionPlacement>().EnterState();
    }

    public void QuitterPartie()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
#else
            Application.Quit();
#endif
    }

    void AfficherÉcranDéfaite(object sender, PartieEventArgs e)
    {
        SceneManager.LoadScene("Défaite");
    }

    void AfficherÉcranVictoire(object sender, PartieEventArgs e)
    {
        SceneManager.LoadScene("Victoire");
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
        if (!GetComponent<ControlleurInterface>().AnimationEstActivée)
        {
            List<InformationTuile> infoTuile = GameObject.Find("ListeTuiles").GetComponentsInChildren<InformationTuile>().ToList();
            Destroy(infoTuile.FindAll(x => x.Case.Coordonnées == CoordVisée).Find(x => x.Case.PositionMonde == PositionVisée)
                .GetComponent<BoxCollider>());
        }
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
        
        if(!GetComponent<ControlleurInterface>().AnimationEstActivée)
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
        Joueur tempJoueur = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempJoueur;

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