using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Bot : Joueur
{
    // Classe random pris de : http://stackoverflow.com/a/18267477/106356
    System.Random rng = new System.Random(Guid.NewGuid().GetHashCode());

    private Predicate<TypeOccupation> p = (TypeOccupation e) => e == TypeOccupation.Touché;
    List<Coordonnées> DernierTirs = new List<Coordonnées>(5);
    List<TypeOccupation> ÉtatDerniersTirs = new List<TypeOccupation>();
    int AxeAuHasard() => rng.Next(0,10);
    bool EstTiré(Coordonnées coord) => GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Touché ||
                                  GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Manqué;
    int cpt = 0;//test

    public void Placer()
    {
        foreach (var b in Arsenal)
        {
            bool estDisponible = true;
            while (estDisponible)
            {
                int colonneInitiale = rng.Next(1, 11);
                int rangéeInitiale = rng.Next(1, 11);
                int rangéeFinale = rangéeInitiale;
                int colonneFinale = colonneInitiale;
                int orientation = rng.Next(0, 4);
                List<Case> paneauxUtilisés = PaneauJeu.Cases.Range(rangéeInitiale, colonneInitiale, rangéeFinale, colonneFinale);

                if (orientation == 0)
                    for (int i = 1; 1 < b.Longueur; i++)
                        rangéeFinale++;

                else if (orientation == 1)
                    for (int i = 1; i < b.Longueur; i++)
                        colonneFinale++;
                
                else if (orientation == 2)
                    for (int i = 1; i < b.Longueur; i++)
                        colonneFinale--;
                
                else if (orientation == 3)
                    for (int i = 1; i < b.Longueur; i++)
                        colonneFinale--;

                if (rangéeFinale > 10 || colonneFinale > 10)
                {
                    estDisponible = true;
                    continue;
                }

                if (paneauxUtilisés.Any(x => x.EstOccupé))
                {
                    estDisponible = true;
                    continue;
                }

                foreach (var p in paneauxUtilisés)
                    p.TypeOccupation = b.TypeOccupation;

                estDisponible = false;

            }
        }
        GestionnaireJeu.manager.NextPlayer();
    }

    public Coordonnées Tirer()
    {
        
        Coordonnées tir = DéterminerProchainTir();

        DernierTirs.Add(tir);

        //test
        if (cpt== 0)
            ÉtatDerniersTirs.Add(TypeOccupation.Touché); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Touché;
        if (cpt == 1)
            ÉtatDerniersTirs.Add(TypeOccupation.Manqué); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Manqué;
        if (cpt == 2)
            ÉtatDerniersTirs.Add(TypeOccupation.Manqué); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Manqué;
        if (cpt == 3)
            ÉtatDerniersTirs.Add(TypeOccupation.Touché); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Touché;
        if (cpt == 4)
            ÉtatDerniersTirs.Add(TypeOccupation.Touché); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Touché;
        if (cpt == 5)
            ÉtatDerniersTirs.Add(TypeOccupation.Touché); GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(tir).TypeOccupation = TypeOccupation.Touché;
        //test

        if (DernierTirs.Count > 5)
            DernierTirs.RemoveAt(0);
        if (ÉtatDerniersTirs.Count > 5)
            ÉtatDerniersTirs.RemoveAt(0);
        cpt++;//test
        return tir;
    }

    private Coordonnées PositionAuHasard()
    {
        bool ok = false;
        Coordonnées pos = new Coordonnées();
        while (!ok)
        {
            pos.Colonne = AxeAuHasard();
            pos.Rangée = AxeAuHasard();
            if (!EstTiré(pos))
                ok = true;
        }
        return pos;
    }

    private Coordonnées DéterminerProchainTir()
    {
        if (GestionnaireJeu.manager.Tour == 0)
        {
            GestionnaireJeu.manager.Tour++;//test seulement
            return PositionAuHasard();
        }
            
        
        bool b = false;
        Coordonnées ProchainTir = new Coordonnées();
        Coordonnées DernierTir = DernierTirs[DernierTirs.Count-1];
        for (int i = 0; i <= DernierTirs.Count - 1; i++)//s'il y a aucune touche dans les 5 derniers tirs on tir au hasard
        {
            if (GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(DernierTirs[DernierTirs.Count - i - 1]).TypeOccupation != TypeOccupation.Manqué)
            {
                b = true;
                break;
            }
        }
        if (!b)
            ProchainTir = PositionAuHasard();
        else
        {
            int f = ÉtatDerniersTirs.FindIndex(p);//la première touche
            int l = ÉtatDerniersTirs.FindLastIndex(p);//la dernière touche
            int nt = ÉtatDerniersTirs.FindAll(p).Count();//le nombre de touche

            Coordonnées DernierTirTouché = DernierTirs[l];
            Coordonnées PremierTirTouché = DernierTirs[f];

            if (nt == 1)
                ProchainTir = TirerBas(DernierTirTouché);//on commence par tirer en bas puis on tourne dans le sens horaire si ce n'est pas possible ou déjà fait

            else if (nt >= 2)//s'il y a au moins deux touche on continue sur la même ligne
            {
                int diffX = DernierTirTouché.Colonne - PremierTirTouché.Colonne;
                int diffY = DernierTirTouché.Rangée - PremierTirTouché.Rangée;

                if (diffX == 0 && diffY >= 1)
                {
                    
                    if (DernierTirTouché.Rangée != 9/*bounds*/&& !EstTiré(new Coordonnées(DernierTirTouché.Rangée + 1, DernierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée + 1;
                    }
                    else if (DernierTirTouché.Rangée != 0/*bounds*/&& !EstTiré(new Coordonnées(PremierTirTouché.Rangée - 1, PremierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée - 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX == 0 && diffY <= -1)
                {
                    if (DernierTirTouché.Rangée != 0/*bounds*/&& !EstTiré(new Coordonnées(DernierTirTouché.Rangée-1, DernierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée - 1;
                    }
                    else if (DernierTirTouché.Rangée != 9/*bounds*/&& !EstTiré(new Coordonnées(PremierTirTouché.Rangée+1, PremierTirTouché.Colonne)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée + 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX >= 1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 9/*bounds*/&& !EstTiré(new Coordonnées(DernierTirTouché.Rangée, DernierTirTouché.Colonne+1)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne + 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 0/*bounds*/&& !EstTiré(new Coordonnées(PremierTirTouché.Rangée, PremierTirTouché.Colonne-1)))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = PremierTirTouché.Rangée;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX <= -1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 0/*bounds*/&& !EstTiré(new Coordonnées(DernierTirTouché.Rangée, DernierTirTouché.Colonne-1)))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 9/*bounds*/&& !EstTiré(new Coordonnées(PremierTirTouché.Rangée, PremierTirTouché.Colonne+1)))
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

    private Coordonnées TirerBas(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Rangée != 9/*bounds*/&& !EstTiré(new Coordonnées(last.Rangée+1, last.Colonne)))
        {
            ToReturn.Colonne = last.Colonne;
            ToReturn.Rangée = last.Rangée + 1;
            return ToReturn;
        }
        else
            return TirerGauche(last);
    }

    private Coordonnées TirerGauche(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Colonne != 0/*bounds*/&& !EstTiré(new Coordonnées(last.Rangée, last.Colonne-1)))
        {
            ToReturn.Colonne = last.Colonne - 1;
            ToReturn.Rangée = last.Rangée;
            return ToReturn;
        }
        else
            return TirerHaut(last);
    }

    private Coordonnées TirerHaut(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Rangée != 0/*bounds*/&& !EstTiré(new Coordonnées(last.Rangée-1, last.Colonne)))
        {
            ToReturn.Colonne = last.Colonne;
            ToReturn.Rangée = last.Rangée - 1;
            return ToReturn;
        }
        else
            return TirerDroite(last);
    }

    private Coordonnées TirerDroite(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Colonne != 9/*bounds*/&& !EstTiré(new Coordonnées(last.Rangée, last.Colonne+1)))
        {
            ToReturn.Colonne = last.Colonne + 1;
            ToReturn.Rangée = last.Rangée;
            return ToReturn;
        }
        else
            return PositionAuHasard();
    }

}