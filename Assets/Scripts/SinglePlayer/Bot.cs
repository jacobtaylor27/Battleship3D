using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    GrilleTirs GrilleDeTirs = new GrilleTirs(); // modele pour ce que le bot touche et ne touche pas
    GrilleBateau GrilleDeBateaux = new GrilleBateau();
    Coordonnées DernierTir = new Coordonnées();
    Coordonnées ProchainTir = new Coordonnées();

    public void GénérerDirectionAléatoire()
    {
        foreach (Bateau b in GrilleDeBateaux.BateauxPlacés)
        {
            var dir = Random.Range(0, 4);

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

    //public void DéterminerProchainTir()
    //{
    //    if (GrilleDeTirs[DernierTir.X, DernierTir.Y] == /*pas touché*/ )
    //    {
    //        ProchainTir.X = Random.Range(0, 11);
    //        ProchainTir.Y = Random.Range(0, 11);
    //    }
    //    else
    //    {

    //    }
    //}
    public void Tirer()
    {
        DéterminerProchainTir();

    }
}