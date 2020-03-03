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
    List<Coordonnées> DernierTirs = new List<Coordonnées>();
    List<TypeOccupation> ÉtatDerniersTirs = new List<TypeOccupation>();
    int AxeAuHasard() => rng.Next(0,11);
    bool EstTiré(int x, int y) => PaneauTirs.Cases.Find(c => c.Coordonnées.Colonne == x && c.Coordonnées.Rangée == y).TypeOccupation == TypeOccupation.Touché ||
                                  PaneauTirs.Cases.Find(c => c.Coordonnées.Colonne == x && c.Coordonnées.Rangée == y).TypeOccupation == TypeOccupation.Manqué;

    public void Placer()
    {
        foreach (var b in Arsenal)
        {
            bool estDisponible = true;
            while (estDisponible)
            {
                var colonneInitiale = rng.Next(1, 11);
                var rangéeInitiale = rng.Next(1, 11);
                var rangéeFinale = rangéeInitiale;
                var colonneFinale = colonneInitiale;
                var orientation = rng.Next(0, 4);
                var paneauxUtilisés = PaneauJeu.Cases.Range(rangéeInitiale, colonneInitiale, rangéeFinale, colonneFinale);

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
    }

    public Coordonnées Tirer()
    {
        Coordonnées tir = DéterminerProchainTir();

        int indexCaseTiré = PaneauJeu.Cases.FindIndex(x => x.Coordonnées == tir);
        var typeOccupationCaseTiré = PaneauJeu.Cases[indexCaseTiré].TypeOccupation;

        PaneauJeu.Cases[indexCaseTiré].TypeOccupation = TypeOccupation.Touché;

        DernierTirs.Add(tir);
        ÉtatDerniersTirs.Add(PaneauTirs.Cases.Find(x => x.Coordonnées == tir).TypeOccupation);
        if (DernierTirs.Count > 5)
            DernierTirs.RemoveAt(0);
        if (ÉtatDerniersTirs.Count > 5)
            ÉtatDerniersTirs.RemoveAt(0);
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
            if (!EstTiré(pos.Colonne, pos.Rangée))
                ok = true;
        }
        return pos;
    }

    private Coordonnées DéterminerProchainTir()
    {
        bool b = false;
        Coordonnées ProchainTir = new Coordonnées();
        Coordonnées DernierTir = DernierTirs[DernierTirs.Count];
        for (int i = 0; i < DernierTirs.Count || b == true; i--)//s'il y a aucune touche dans les 5 derniers tirs on tir au hasard
        {

            if (PaneauTirs.Cases.Find(x => x.Coordonnées.Rangée == DernierTirs[DernierTirs.Count - i].Rangée &&
            x.Coordonnées.Colonne == DernierTirs[DernierTirs.Count - i].Colonne).TypeOccupation == TypeOccupation.Manqué)
                b = true;
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
                    if (DernierTirTouché.Rangée != 9/*bounds*/|| !EstTiré(DernierTirTouché.Colonne, DernierTirTouché.Rangée + 1))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée + 1;
                    }
                    else if (DernierTirTouché.Rangée != 0/*bounds*/|| !EstTiré(PremierTirTouché.Colonne, PremierTirTouché.Rangée - 1))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée - 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX == 0 && diffY <= -1)
                {
                    if (DernierTirTouché.Rangée != 0/*bounds*/|| !EstTiré(DernierTirTouché.Colonne, DernierTirTouché.Rangée - 1))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne;
                        ProchainTir.Rangée = DernierTirTouché.Rangée - 1;
                    }
                    else if (DernierTirTouché.Rangée != 9/*bounds*/|| !EstTiré(PremierTirTouché.Colonne, PremierTirTouché.Rangée + 1))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne;
                        ProchainTir.Rangée = PremierTirTouché.Rangée + 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX >= 1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 9/*bounds*/|| !EstTiré(DernierTirTouché.Colonne + 1, DernierTirTouché.Rangée))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne + 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 0/*bounds*/|| !EstTiré(PremierTirTouché.Colonne - 1, PremierTirTouché.Rangée))
                    {
                        ProchainTir.Colonne = PremierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = PremierTirTouché.Rangée;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX <= -1 && diffY == 0)
                {
                    if (DernierTirTouché.Colonne != 0/*bounds*/|| !EstTiré(DernierTirTouché.Colonne - 1, DernierTirTouché.Rangée))
                    {
                        ProchainTir.Colonne = DernierTirTouché.Colonne - 1;
                        ProchainTir.Rangée = DernierTirTouché.Rangée;
                    }
                    else if (DernierTirTouché.Colonne != 9/*bounds*/|| !EstTiré(PremierTirTouché.Colonne + 1, PremierTirTouché.Rangée))
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
        if (last.Rangée != 9/*bounds*/|| !EstTiré(last.Colonne, last.Rangée + 1))
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
        if (last.Colonne != 0/*bounds*/|| !EstTiré(last.Colonne - 1, last.Rangée))
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
        if (last.Rangée != 0/*bounds*/|| !EstTiré(last.Colonne, last.Rangée - 1))
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
        if (last.Colonne != 9/*bounds*/|| !EstTiré(last.Colonne + 1, last.Rangée))
        {
            ToReturn.Colonne = last.Colonne + 1;
            ToReturn.Rangée = last.Rangée;
            return ToReturn;
        }
        else
            return PositionAuHasard();
    }

}