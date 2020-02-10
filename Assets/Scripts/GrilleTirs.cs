using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleTirs
{
    const int dimensions = 10;
    private int[,] infoTirs = new int[dimensions, dimensions];
    //Coordonnées?

    //Propriété indexée
    public int this[int index1,int index2]
    {
        get { return infoTirs[index1, index2]; }
        private set { infoTirs[index1, index2] = value; }
    }

    public GrilleTirs()
    {
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                infoTirs[i, j] = (int)ÉtatOccupation.Vide;
            }
        }
    }

    public void AjouterTir(Coordonnées caseVisée, GrilleBateau grille)
    {
        VérifierRésultatTir(caseVisée,grille);

    }

    private ÉtatOccupation VérifierRésultatTir(Coordonnées caseVisée, GrilleBateau grille)
    {
        ÉtatOccupation occupÀAjouter = grille[caseVisée.X, caseVisée.Y];

        //Vérifier à coord x dans grille si il y a un bateau
        //Si oui État == touché else == manqué
        if (occupÀAjouter == ÉtatOccupation.Occupé)
            occupÀAjouter = ÉtatOccupation.Touché;
        else if (occupÀAjouter == ÉtatOccupation.Vide)
            occupÀAjouter = ÉtatOccupation.Manqué;
        
        return occupÀAjouter;
    }

}
