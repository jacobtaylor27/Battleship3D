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
                Bateaux[i, j] = (int)ÉtatBateau.Détruit;
            }
        }
    }

    public void AjouterBateau(Bateau bat)
    {
        //Prendre position du bateau dans grille et y placer l'origine
        //Changer état pour ÉtatBateau.Actif à l'origine
        //Changer état à ÉtatActif pour toutes les cases selon bat.Direction à partir de l'origine jusqu'a longueur
        //S'assurer que bateau respecte les dimensions
    }

}
