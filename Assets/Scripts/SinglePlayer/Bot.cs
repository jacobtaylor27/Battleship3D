﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    GrilleTirs GrilleDeTirs = new GrilleTirs(); // modele pour ce que le bot touche et ne touche pas
    GrilleBateau GrilleDeBateaux = new GrilleBateau();
    List<Coordonnées> DernierTirs = new List<Coordonnées>();
    

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
    public Coordonnées ProchainTir4Possibilité(Coordonnées DernierTir)//s'il y a une case seule de touché
    {
        Coordonnées aReturn = new Coordonnées();
        var RNG = Random.Range(0, 4);
        switch (RNG)
        {
            case 1:
                aReturn.X = DernierTir.X + 1;
                aReturn.Y = DernierTir.Y;
                break;
            case 2:
                aReturn.X = DernierTir.X - 1;
                aReturn.Y = DernierTir.Y;
                break;
            case 3:
                aReturn.X = DernierTir.X;
                aReturn.Y = DernierTir.Y + 1;
                break;
            case 4:
                aReturn.X = DernierTir.X;
                aReturn.Y = DernierTir.Y - 1;
                break;
        }
        return aReturn;
    }

    public Coordonnées DéterminerProchainTir()
    {
        bool b=false;
        Coordonnées ProchainTir = new Coordonnées();
        //Coordonnées DernierTir = DernierTirs[DernierTirs.Count];
        for (int i = 0; i < DernierTirs.Count||b==true; i--)
        {
            if (GrilleDeTirs[DernierTirs[DernierTirs.Count-i].X, DernierTirs[DernierTirs.Count - i].Y] != ÉtatOccupation.Manqué)
            {
                b = true;
            }
        }
        if (!b)
        {
            ProchainTir.X = Random.Range(0, 11);
            ProchainTir.Y = Random.Range(0, 11);
        }
        else 
        {

            
        }
        return ProchainTir;
    }
    public void Tirer()
    {
        Coordonnées tir = DéterminerProchainTir();
        DernierTirs.Add(tir);
        if (DernierTirs.Count >= 5)
            DernierTirs.RemoveAt(0);
        
    }
}