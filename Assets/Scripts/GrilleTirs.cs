using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleTirs
{
    const int dimensions = 10;
    int[,] InfoTirs = new int[dimensions, dimensions];
    //Coordonnées?

    public GrilleTirs()
    {
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                InfoTirs[i, j] = (int)ÉtatOccupation.Vide;
            }
        }
    }

    public void AjouterTir()
    {

    }

    public ÉtatOccupation VérifierRésultatTir(Coordonnées coord, GrilleBateau grille)
    {
        ÉtatOccupation occupÀAjouter;
        //Vérifier à coord x dans grille si il y a un bateau
        //Si oui État == touché else == manqué

        return occupÀAjouter;
    }

}
