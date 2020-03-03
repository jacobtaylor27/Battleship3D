using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PaneauJeu
{
    public List<Case> Cases { get; set; }

    public PaneauJeu()
    {
        Cases = new List<Case>();
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                Cases.Add(new Case(i, j));
            }
        }
    }

    public Case TrouverCase(Coordonnées coord)
    {
        return Cases.Where(x => x.Coordonnées.Rangée == coord.Rangée && x.Coordonnées.Colonne == coord.Colonne).First();
    }
}
