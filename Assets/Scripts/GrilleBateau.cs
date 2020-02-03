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
        int positionX;
        int positionY;

        //Prendre position du bateau dans grille et y placer l'origine
        ConvertirPositionToCoordonnées();
        //Changer état pour ÉtatBateau.Actif à l'origine
        Bateaux[positionX,positionY] = (int)ÉtatOccupation.Occupé;
        //Changer état à ÉtatActif pour toutes les cases selon bat.Direction à partir de l'origine jusqu'a longueur
        for(int i = 1; i < bat.Longueur; i++)
        {
            //S'assurer que bateau respecte les dimensions
            Bateaux[positionX + i * (int)bat.Direction.x, positionY + i * (int)bat.Direction.y] = (int)ÉtatOccupation.Occupé;
        }
    }

}
