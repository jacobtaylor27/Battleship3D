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
        for (int i = 0; i < DernierTirs.Count || b == true; i--)
        {
            if (GrilleDeTirs[DernierTirs[DernierTirs.Count - i].X, DernierTirs[DernierTirs.Count - i].Y] != ÉtatOccupation.Manqué)
            {
                b = true;
            }
        }
        if (!b)
        {
            ProchainTir.X = UnityEngine.Random.Range(0, 11);
            ProchainTir.Y = UnityEngine.Random.Range(0, 11);
        }
        else
        {
            int f = ÉtatDerniersTirs.FindIndex(p);//la première touche
            int l = ÉtatDerniersTirs.FindLastIndex(p);//la dernière touche
            int nt = ÉtatDerniersTirs.FindAll(p).Count();//le nombre de touche

            if (nt == 1 && l == nt)//si seul le dernier tir est une touche on tir en bas
            {
                if (DernierTirs[l].Y != 9/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y + 1;
                }
                else if (DernierTir.X !=0/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X-1;
                    ProchainTir.Y = DernierTir.Y;
                }
                else
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y-1;
                }

            }
            else if (nt == 1 && l == nt-1)//ensuite a gauche
            {
                if (DernierTirs[l].X != 0/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X - 1;
                    ProchainTir.Y = DernierTir.Y;
                }
                else if (DernierTirs[l].Y != 0/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y-1;
                }
                else
                {
                    ProchainTir.X = DernierTir.X+1;
                    ProchainTir.Y = DernierTir.Y;
                }
            }
            else if (nt == 1 && l == nt - 2)//ensuite en haut
            {
                if (DernierTirs[l].Y != 0/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y - 1;
                }
                else if(DernierTirs[l].X != 9/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X+1;
                    ProchainTir.Y = DernierTir.Y;
                }
                else
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y+1;
                }
            }
            else if (nt == 1 && l == nt - 3)//ensuite a droite
            {
                if (DernierTirs[l].X != 9/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X + 1;
                    ProchainTir.Y = DernierTir.Y;
                }
                else if (DernierTirs[l].Y != 9/*bounds*/)
                {
                    ProchainTir.X = DernierTir.X;
                    ProchainTir.Y = DernierTir.Y+1;
                }
                else
                {
                    ProchainTir.X = DernierTir.X - 1;
                    ProchainTir.Y = DernierTir.Y;
                }
            }
            else if (nt >= 2)//s'il y a au moins deux touche on continue sur la même ligne
            {
                int diffX = DernierTirs[l].X - DernierTirs[f].X;
                int diffY = DernierTirs[l].Y - DernierTirs[f].Y;

                if (diffX == 0 && diffY == 1)
                {
                    if (DernierTirs[l].Y != 9/*bounds*/)
                    {
                        ProchainTir.X = DernierTir.X;
                        ProchainTir.Y = DernierTir.Y + 1;
                    }
                    else
                    {
                        ProchainTir.X = DernierTir.X;
                        ProchainTir.Y = DernierTir.Y - 1;
                    }
                }
                else if (diffX == 0 && diffY == -1)
                {
                    if (DernierTirs[l].Y != 0/*bounds*/)
                    {
                        ProchainTir.X = DernierTir.X;
                        ProchainTir.Y = DernierTir.Y - 1;
                    }
                    else
                    {
                        ProchainTir.X = DernierTir.X;
                        ProchainTir.Y = DernierTir.Y + 1;
                    }
                }
                else if (diffX == 1 && diffY == 0)
                {
                    if (DernierTirs[l].X != 9/*bounds*/)
                    {
                        ProchainTir.X = DernierTir.X + 1;
                        ProchainTir.Y = DernierTir.Y;
                    }
                    else
                    {
                        ProchainTir.X = DernierTir.X - 1;
                        ProchainTir.Y = DernierTir.Y;
                    }
                }
                else if (diffX == -1 && diffY == 0)
                {
                    if (DernierTirs[l].X != 0/*bounds*/)
                    {
                        ProchainTir.X = DernierTir.X - 1;
                        ProchainTir.Y = DernierTir.Y;
                    }
                    else
                    {
                        ProchainTir.X = DernierTir.X + 1;
                        ProchainTir.Y = DernierTir.Y;
                    }
                }
            }
        }
        return ProchainTir;
    }
    public void Tirer()
    {
        Coordonnées tir = DéterminerProchainTir();
        DernierTirs.Add(tir);
        if (DernierTirs.Count > 5)
            DernierTirs.RemoveAt(0);

    }
    static private bool EstTouché(ÉtatOccupation E)
    {
        return E == ÉtatOccupation.Touché;
    }
}