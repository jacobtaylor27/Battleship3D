using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class Paneau
{
    public List<Case> Cases { get; set; }
    public event EventHandler<OccupationEventArgs> OccupationModifiée;

    public Paneau()
    {
        Cases = new List<Case>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Cases.Add(new Case(i, j));
            }
        }
    }

    public Case TrouverCase(Coordonnées coord)
    {
        return Cases.Where(x => x.Coordonnées.Rangée == coord.Rangée && x.Coordonnées.Colonne == coord.Colonne).First();
    }

    public void OnOccupationModifiée(OccupationEventArgs dataOccupation)
    {
        this.OccupationModifiée?.Invoke(this, dataOccupation);
    }

    public void ModifierÉtatCase(Coordonnées coord, TypeOccupation occup)
    {
        TrouverCase(coord).TypeOccupation = occup;
        OnOccupationModifiée(new OccupationEventArgs(new Case(coord, occup)));
       
    }
}
