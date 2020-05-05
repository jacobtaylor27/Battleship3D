using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;
public class Bot : Joueur
{
    private Predicate<TypeOccupation> EstTouché = (TypeOccupation t) => t == TypeOccupation.Touché;
    List<Coordonnées> DernierTirs = new List<Coordonnées>(5);
    List<TypeOccupation> ÉtatDerniersTirs = new List<TypeOccupation>(5);
    int AxeAuHasard() => UnityEngine.Random.Range(0, 10);
   
    Vector3 OrientationV { get; set; }
    public bool dernierTirCoulé = false;

    public Bot()
        : base(){ }
     
    public void Placer()
    {
        int indiceBateau = 0;
        foreach (var b in GestionnaireJeu.manager.JoueurActif.Arsenal)
        {
            List<Case> paneauxUtilisés = new List<Case>();
            bool estPlacer = false;
            while (!estPlacer)
            {
                int colonneInitiale = UnityEngine.Random.Range(0, 10);
                int rangéeInitiale = UnityEngine.Random.Range(0, 10);
                int rangéeFinale = rangéeInitiale;
                int colonneFinale = colonneInitiale;
                int orientation = UnityEngine.Random.Range(0, 4);
                //0 : vers la droite
                //1 : vers le bas
                //2 : vers la gauche
                //3 : vers le haut

                switch (orientation)
                {
                    case 0:
                        {
                            OrientationV = Vector3.right;
                            colonneFinale += b.Longueur - 1;
                            break;
                        }
                    case 1:
                        {
                            OrientationV = Vector3.up;
                            rangéeFinale += b.Longueur - 1;
                            break;
                        }
                    case 2:
                        {
                            OrientationV = Vector3.left;
                            colonneFinale -= b.Longueur - 1;
                            break;
                        }
                    case 3:
                        {
                            OrientationV = Vector3.back;
                            rangéeFinale -= b.Longueur - 1;
                            break;
                        }
                }
                var rangéesIF = (rangéeInitiale, rangéeFinale);
                var colonnesIF = (colonneInitiale, colonneFinale);

                if(rangéeFinale < rangéeInitiale)
                    rangéesIF = (rangéeFinale, rangéeInitiale);
                if (colonneFinale < colonneInitiale)
                    colonnesIF = (colonneFinale, colonneInitiale);

                paneauxUtilisés = PaneauJeu.Cases.Range(rangéesIF.Item1, colonnesIF.Item1, rangéesIF.Item2, colonnesIF.Item2);

                int cptOccupation = 0;

                if (rangéeFinale >= 0 && colonneFinale >= 0 && rangéeFinale < 10 && colonneFinale < 10)
                {
                    for (int i = 0; i < b.Longueur; i++)
                    {
                        Coordonnées temp = new Coordonnées(rangéeInitiale + i * (int)OrientationV.z, colonneInitiale + i * (int)OrientationV.x);
                        if (GestionnaireJeu.manager.JoueurActif.PaneauJeu.TrouverCase(new Coordonnées(rangéeInitiale + i * (int)OrientationV.z, colonneInitiale + i * (int)OrientationV.x)).EstOccupé)
                            break;
                        else
                            cptOccupation++;

                    }

                    if (cptOccupation == b.Longueur)
                    {
                        estPlacer = true;
                        foreach (var panneau in paneauxUtilisés)
                        {
                            GestionnaireJeu.manager.JoueurActif.PaneauJeu.TrouverCase(panneau.Coordonnées).TypeOccupation = TypeOccupation.Occupé;
                            //Debug.Log(panneau.ToString());

                        }
                    }
                }
            }
            GestionnaireJeu.manager.JoueurActif.Arsenal[indiceBateau].CasesOccupées = paneauxUtilisés;
            indiceBateau++;
        }
        GestionnaireJeu.manager.PasserAuProchainTour();

    }

    public override void Tirer()
    {

        Coordonnées tir = DéterminerProchainTir();

        GestionnaireJeu.manager.PositionVisée = GestionnaireJeu.manager.AutreJoueur.PaneauJeu.TrouverCase(tir).PositionMonde;
        GestionnaireJeu.manager.CoordVisée = tir;
        GestionnaireJeu.manager.DéterminerRésultatTir();

        DernierTirs.Add(tir);
        ÉtatDerniersTirs.Add(GestionnaireJeu.manager.OccupÀCoordVisée);

        if (DernierTirs.Count > 5)
            DernierTirs.RemoveAt(0);
        if (ÉtatDerniersTirs.Count > 5)
            ÉtatDerniersTirs.RemoveAt(0);
    }

    private Coordonnées PositionAuHasard()
    {
        bool ok = false;
        Coordonnées pos = new Coordonnées();
        while (!ok)
        {
            pos.Colonne = AxeAuHasard();
            pos.Rangée = AxeAuHasard();
            if (!Case.EstTiréRaté(pos))
                ok = true;
        }
        return pos;
    }

    private Coordonnées DéterminerProchainTir()
    {
        if (GestionnaireJeu.manager.Tour == 2)//premier tir au hasard
        {
           return PositionAuHasard();
        }

        bool AucuneTouche = true;
        Coordonnées ProchainTir = new Coordonnées();
        Coordonnées DernierTir = DernierTirs[DernierTirs.Count - 1];
        for (int i = 0; i <= DernierTirs.Count - 1; i++)//s'il y a aucune touche dans les 5 derniers tirs on tir au hasard
        {
            if (GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(DernierTirs[DernierTirs.Count - i - 1]).TypeOccupation != TypeOccupation.Manqué)
            {
                AucuneTouche = false;
                break;
            }
        }
        if (AucuneTouche)
            ProchainTir = PositionAuHasard();
        else
        {
            if (dernierTirCoulé)//si le dernier tir à coulé on tir au hasard
            {
                DernierTirs.Clear();
                ÉtatDerniersTirs.Clear();
                dernierTirCoulé = false;
                return PositionAuHasard();
            }
            int p = ÉtatDerniersTirs.FindIndex(EstTouché);//la première touche
            int d = ÉtatDerniersTirs.FindLastIndex(EstTouché);//la dernière touche
            int nbt = ÉtatDerniersTirs.FindAll(EstTouché).Count();//le nombre de touche

            Coordonnées DernierTirTouché = DernierTirs[d];
            Coordonnées PremierTirTouché = DernierTirs[p];

            if (nbt == 1) 
                ProchainTir = TirerBas(DernierTirTouché);//on commence par tirer en bas puis on tourne dans le sens horaire si ce n'est pas possible

            else if (nbt >= 2)//s'il y a au moins deux touche on continue sur la même ligne
            {
                int diffX = DernierTirTouché.Colonne - PremierTirTouché.Colonne;
                int diffY = DernierTirTouché.Rangée - PremierTirTouché.Rangée;

                if (diffX == 0 && diffY >= 1)
                {

                    if (DernierTirTouché.Rangée != 9&& !Case.EstTiréRaté(new Coordonnées(DernierTirTouché.Rangée + 1, DernierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée + 1;
                    }
                    else if (DernierTirTouché.Rangée != 0&& !Case.EstTiréRaté(new Coordonnées(PremierTirTouché.Rangée - 1, PremierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée - 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX == 0 && diffY <= -1)
                {
                    if (DernierTirTouché.Rangée != 0&& !Case.EstTiréRaté(new Coordonnées(DernierTirTouché.Rangée - 1, DernierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée - 1;
                    }
                    else if (DernierTirTouché.Rangée != 9&& !Case.EstTiréRaté(new Coordonnées(PremierTirTouché.Rangée + 1, PremierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée + 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX >= 1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 9&& !Case.EstTiréRaté(new Coordonnées(DernierTirTouché.Rangée, DernierTirTouché.Colonne + 1)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne + 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 0&& !Case.EstTiréRaté(new Coordonnées(PremierTirTouché.Rangée, PremierTirTouché.Colonne - 1)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = PremierTirTouché.Rangée;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX <= -1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 0&& !Case.EstTiréRaté(new Coordonnées(DernierTirTouché.Rangée, DernierTirTouché.Colonne - 1)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 9&& !Case.EstTiréRaté(new Coordonnées(PremierTirTouché.Rangée, PremierTirTouché.Colonne + 1)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne + 1;
                        ProchainTir.Rangée = PremierTirTouché.Rangée;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
            }
        }
        return ProchainTir;
    }

    private Coordonnées TirerBas(Coordonnées dernier)
    {
        Coordonnées àRetourner = new Coordonnées();
        if (dernier.Rangée != 9&& !Case.EstTiréRaté(new Coordonnées(dernier.Rangée + 1, dernier.Colonne)))
        {
            àRetourner.Colonne = dernier.Colonne;
            àRetourner.Rangée = dernier.Rangée + 1;
            return àRetourner;
        }
        else
            return TirerGauche(dernier);
    }

    private Coordonnées TirerGauche(Coordonnées dernier)
    {
        Coordonnées àRetourner = new Coordonnées();
        if (dernier.Colonne != 0&& !Case.EstTiréRaté(new Coordonnées(dernier.Rangée, dernier.Colonne - 1)))
        {
            àRetourner.Colonne = dernier.Colonne - 1;
            àRetourner.Rangée = dernier.Rangée;
            return àRetourner;
        }
        else
            return TirerHaut(dernier);
    }

    private Coordonnées TirerHaut(Coordonnées dernier)
    {
        Coordonnées àRetourner = new Coordonnées();
        if (dernier.Rangée != 0&& !Case.EstTiréRaté(new Coordonnées(dernier.Rangée - 1, dernier.Colonne)))
        {
            àRetourner.Colonne = dernier.Colonne;
            àRetourner.Rangée = dernier.Rangée - 1;
            return àRetourner;
        }
        else
            return TirerDroite(dernier);
    }

    private Coordonnées TirerDroite(Coordonnées dernier)
    {
        Coordonnées àRetourner = new Coordonnées();
        if (dernier.Colonne != 9&& !Case.EstTiréRaté(new Coordonnées(dernier.Rangée, dernier.Colonne + 1)))
        {
            àRetourner.Colonne = dernier.Colonne + 1;
            àRetourner.Rangée = dernier.Rangée;
            return àRetourner;
        }
        else
            return PositionAuHasard();
    }

}