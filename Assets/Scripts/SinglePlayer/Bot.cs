using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Bot
{
    private Predicate<ÉtatOccupation> p = EstTouché;
    GrilleTirs GrilleDeTirs = new GrilleTirs(); // modele pour ce que le bot touche et ne touche pas
    GrilleBateau GrilleDeBateaux = new GrilleBateau();
    List<Coordonnées> DernierTirs = new List<Coordonnées>();
    List<ÉtatOccupation> ÉtatDerniersTirs = new List<ÉtatOccupation>(); //utilisé l'état de la grille de tir au coordonnées des derniers tirs

    public void GénérerDirectionAléatoire()
    {
        foreach (Bateau b in GrilleDeBateaux.BateauxPlacés)
        {
            var dir = UnityEngine.Random.Range(0, 4);

            switch (dir)
            {
                case 0:
                    b.Direction = new Vector3(-1, 0, 0);
                    break;
                case 1:
                    b.Direction = new Vector3(0, -1, 0);
                    break;
                case 2:
                    b.Direction = new Vector3(1, 0, 0);
                    break;
                case 3:
                    b.Direction = new Vector3(0, 1, 0);
                    break;
            }
        }
    }

    public void Placer()
    {
        // Classe random pris de : http://stackoverflow.com/a/18267477/106356
        System.Random rng = new System.Random(Guid.NewGuid().GetHashCode());

        foreach (var b in Arsenal)
        {
            bool estDisponible = true;
            while (estDisponible)
            {
                var colonneInitiale = rng.Next(1, 11);
                var rangéeInitiale = rng.Next(1, 11);
                var rangéeFinale = rangéeInitiale;
                var colonneFinale = colonneInitiale;
                var orientation = rng.Next(0, 2);
                var paneauxUtilisés = PaneauDeJeu.Paneaux.Range(rangéeInitiale, colonneInitiale, rangéeFinale, colonneFinale);

                if (orientation == 0)
                {
                    for (int i = 1; 1 < b.Longueur; i++)
                        rangéeFinale++;
                }
                else
                {
                    for (int i = 1; i < b.Longueur; i++)
                        colonneFinale++;
                }

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
                {
                    p.TypeOccupation = b.TypeOccupation;
                }
                estDisponible = false;
            }
        }
    }
    public void PlacerBateaux()
    {
        int positionXMaximale;
        int positionYMaximale;

        foreach (Bateau b in GrilleDeBateaux.BateauxPlacés)
        {
            if (b.Direction == Vector3.left)
                positionXMaximale = b.Longueur;

            else if (b.Direction == Vector3.down)
                positionYMaximale = 10 - b.Longueur;

            else if (b.Direction == Vector3.right)
                positionXMaximale = 10 - b.Longueur;

            else if (b.Direction == Vector3.up)
                positionYMaximale = b.Longueur;

            DéterminerPositionAléatoirement();
        }
    }
    public void DéterminerPositionAléatoirement()
    {

    }
    private Coordonnées DéterminerProchainTir()
    {
        bool b = false;
        Coordonnées ProchainTir = new Coordonnées();
        Coordonnées DernierTir = DernierTirs[DernierTirs.Count];
        for (int i = 0; i < DernierTirs.Count || b == true; i--)//s'il y a aucune touche dans les 5 derniers tirs on tir au hasard
        {
            if (GrilleDeTirs[DernierTirs[DernierTirs.Count - i].X, DernierTirs[DernierTirs.Count - i].Y] != ÉtatOccupation.Manqué)
            {
                b = true;
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
                int diffX = DernierTirTouché.X - PremierTirTouché.X;
                int diffY = DernierTirTouché.Y - PremierTirTouché.Y;

                if (diffX == 0 && diffY >= 1)
                {
                    if (DernierTirTouché.Y != 9/*bounds*/|| !EstTiré(DernierTirTouché.X, DernierTirTouché.Y + 1))
                    {
                        ProchainTir.X = DernierTirTouché.X;
                        ProchainTir.Y = DernierTirTouché.Y + 1;
                    }
                    else if (DernierTirTouché.Y != 0/*bounds*/|| !EstTiré(PremierTirTouché.X, PremierTirTouché.Y - 1))
                    {
                        ProchainTir.X = PremierTirTouché.X;
                        ProchainTir.Y = PremierTirTouché.Y - 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX == 0 && diffY <= -1)
                {
                    if (DernierTirTouché.Y != 0/*bounds*/|| !EstTiré(DernierTirTouché.X, DernierTirTouché.Y - 1))
                    {
                        ProchainTir.X = DernierTirTouché.X;
                        ProchainTir.Y = DernierTirTouché.Y - 1;
                    }
                    else if (DernierTirTouché.Y != 9/*bounds*/|| !EstTiré(PremierTirTouché.X, PremierTirTouché.Y + 1))
                    {
                        ProchainTir.X = PremierTirTouché.X;
                        ProchainTir.Y = PremierTirTouché.Y + 1;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX >= 1 && diffY == 0)
                {
                    if (DernierTirTouché.X != 9/*bounds*/|| !EstTiré(DernierTirTouché.X + 1, DernierTirTouché.Y))
                    {
                        ProchainTir.X = DernierTirTouché.X + 1;
                        ProchainTir.Y = DernierTirTouché.Y;
                    }
                    else if (DernierTirTouché.X != 0/*bounds*/|| !EstTiré(PremierTirTouché.X - 1, PremierTirTouché.Y))
                    {
                        ProchainTir.X = PremierTirTouché.X - 1;
                        ProchainTir.Y = PremierTirTouché.Y;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
                else if (diffX <= -1 && diffY == 0)
                {
                    if (DernierTirTouché.X != 0/*bounds*/|| !EstTiré(DernierTirTouché.X - 1, DernierTirTouché.Y))
                    {
                        ProchainTir.X = DernierTirTouché.X - 1;
                        ProchainTir.Y = DernierTirTouché.Y;
                    }
                    else if (DernierTirTouché.X != 9/*bounds*/|| !EstTiré(PremierTirTouché.X + 1, PremierTirTouché.Y))
                    {
                        ProchainTir.X = PremierTirTouché.X + 1;
                        ProchainTir.Y = PremierTirTouché.Y;
                    }
                    else
                        ProchainTir = PositionAuHasard();
                }
            }
        }
        return ProchainTir;
    }
    public void Tirer()
    {
        Coordonnées tir = DéterminerProchainTir();
        DernierTirs.Add(tir);
        ÉtatDerniersTirs.Add(GrilleDeTirs[tir.X, tir.Y]);
        if (DernierTirs.Count > 5)
            DernierTirs.RemoveAt(0);
        if (ÉtatDerniersTirs.Count > 5)
            ÉtatDerniersTirs.RemoveAt(0);
    }
    static private bool EstTouché (ÉtatOccupation e) => e == ÉtatOccupation.Touché;
    private bool EstTiré (int posX, int posY) => (GrilleDeTirs[posX, posY] == ÉtatOccupation.Manqué || GrilleDeTirs[posX, posY] == ÉtatOccupation.Touché);
    private Coordonnées PositionAuHasard()
    {
        bool ok = false;
        Coordonnées pos = new Coordonnées();
        while (!ok)
        {
            pos.X = AxeAuHasard();
            pos.Y = AxeAuHasard();
            if (!EstTiré(pos.X, pos.Y))
                ok = true;
        }
        return pos;
    }
    private int AxeAuHasard () => UnityEngine.Random.Range(0, 11);
    private Coordonnées TirerBas(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Y != 9/*bounds*/|| !EstTiré(last.X, last.Y + 1))
        {
            ToReturn.X = last.X;
            ToReturn.Y = last.Y + 1;
            return ToReturn;
        }
        else
            return TirerGauche(last);
    }
    private Coordonnées TirerGauche(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.X != 0/*bounds*/|| !EstTiré(last.X - 1, last.Y))
        {
            ToReturn.X = last.X - 1;
            ToReturn.Y = last.Y;
            return ToReturn;
        }
        else
            return TirerHaut(last);
    }
    private Coordonnées TirerHaut(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.Y != 0/*bounds*/|| !EstTiré(last.X, last.Y - 1))
        {
            ToReturn.X = last.X;
            ToReturn.Y = last.Y - 1;
            return ToReturn;
        }
        else
            return TirerDroite(last);
    }
    private Coordonnées TirerDroite(Coordonnées last)
    {
        Coordonnées ToReturn = new Coordonnées();
        if (last.X != 9/*bounds*/|| !EstTiré(last.X + 1, last.Y))
        {
            ToReturn.X = last.X + 1;
            ToReturn.Y = last.Y;
            return ToReturn;
        }
        else
            return PositionAuHasard();
    }
}