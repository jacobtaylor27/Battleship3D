using System;
using System.Collections.Generic;

public class OccupationEventArgs : EventArgs
{
    Coordonnées nouvelleCoord { get; set; }

    TypeOccupation nouvelleOccupation { get; set; }

    public OccupationEventArgs(Coordonnées coord, TypeOccupation occup)
    {
        nouvelleCoord = coord;
        nouvelleOccupation = occup;
    }
}

public class BateauEventArgs : EventArgs
{
    Bateau Bateau { get; set; }

    public BateauEventArgs(Bateau bat)
    {
        Bateau = bat;
    }
}