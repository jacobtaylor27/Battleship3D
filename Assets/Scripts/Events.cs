using System;
using System.Collections.Generic;
using UnityEngine;

public class OccupationEventArgs : EventArgs
{
    Coordonnées NouvelleCoord { get; set; }
    TypeOccupation NouvelleOccupation { get; set; }

    public OccupationEventArgs(Case caseJeu)
    {
        NouvelleCoord = caseJeu.Coordonnées;
        NouvelleOccupation = caseJeu.TypeOccupation;
    }
}

public class TourEventArgs : EventArgs
{
    int Tour { get; set; }

    public TourEventArgs(int val)
    {
        Tour = val;
    }
}

public class PartieEventArgs : EventArgs
{
    public PartieEventArgs()
    {

    }
}

public class BateauEventArgs : EventArgs
{
    Bateau Bateau { get; set; }

    public BateauEventArgs(Bateau bat)
    {
        Bateau = new Bateau(bat.Longueur, bat.PrefabBateau, bat.PrefabCube)
        {
            Coups = bat.Coups
        };
    }
}