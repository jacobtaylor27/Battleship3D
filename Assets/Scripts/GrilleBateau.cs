using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
using System.IO;

public class GrilleBateau
{
    const int dimensions = 10;
    int[,] Bateaux = new int[dimensions, dimensions];
    //Coordonnées?

    public GrilleBateau()
    {
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                Bateaux[i, j] = (int)ÉtatOccupation.Vide;
            }
        }
    }

    public void AjouterBateau(Bateau bat)
    {
        Coordonnées coordOrigine = new Coordonnées();


        //Prendre position du bateau dans grille et y placer l'origine
        ConvertirPositionToCoordonnées(bat.Origine);
        //Changer état pour ÉtatBateau.Actif à l'origine
        Bateaux[coordOrigine.X,coordOrigine.Y] = (int)ÉtatOccupation.Occupé;
        //Changer état à ÉtatActif pour toutes les cases selon bat.Direction à partir de l'origine jusqu'a longueur
        for(int i = 1; i < bat.Longueur; i++)
        {
            //S'assurer que bateau respecte les dimensions
            Bateaux[coordOrigine.X + i * (int)bat.Direction.x, coordOrigine.Y + i * (int)bat.Direction.y] = (int)ÉtatOccupation.Occupé;
        }
    }

    public Coordonnées ConvertirPositionToCoordonnées(Vector3 origine)
    {
        int posX, posY;
        return new Coordonnées(posX, posY);
    }

}
