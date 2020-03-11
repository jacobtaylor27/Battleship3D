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
    bool Fait { get; set; }
    int Tour { get; set; }
    Button BoutonGameStart { get; set; }
    public Vector3 PositionVisée { get; set; }
    public Coordonnées CaseVisée { get;set; }


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

    }
    void Awake()
    {
        BoutonGameStart = GetComponents<Button>().First(x => x.name == "BtnCommencer");
        BoutonGameStart.onClick.AddListener(CommencerPartie);
    }
    private void CommencerPartie()
    {
        Bot.Placer();
        GetComponent<PlacementBateau>().EnterState();
    }

    private void CommencerPhaseTirs()
    {
        GetComponent<GestionTirs>().EnterState();

    }
    private void LancerAnimationBot(object sender, OccupationEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void LancerAnimationJoueur(object sender, OccupationEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void DéterminerRésultatTir()
    {
        Case CaseÀChanger = JoueurActif.PaneauJeu.TrouverCase(CaseVisée);
        TypeOccupation tempOccupation;
        if (AutreJoueur.PaneauJeu.TrouverCase(CaseVisée).EstOccupé)
            tempOccupation = TypeOccupation.Touché;
        else
            tempOccupation = TypeOccupation.Manqué;
        JoueurActif.PaneauTirs.OnOccupationModifiée(new OccupationEventArgs(new Case(CaseVisée, tempOccupation)));
    }

    public void NextPlayer()
    {
        Joueur tempPlayer = JoueurActif;
        JoueurActif = AutreJoueur;
        AutreJoueur = tempPlayer;
    }


    /*public void PlacerBateauxJoueur()
    {
        //à mettre dans joueur(je crois)
        var positionCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var b in Joueur.Arsenal)
        {
            Fait = false;
            while (!Fait)
            {
                Instantiate(b.Maquette, new Vector3(positionCamera.x, 1f, positionCamera.z), Quaternion.identity);
                if (Input.GetKeyDown(Placer))
                {
                    Fait = true;
                }
            }
        }
    }*/

    

}