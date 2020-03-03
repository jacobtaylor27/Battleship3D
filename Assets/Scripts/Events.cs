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

public class ArsenalEventArgs : EventArgs
{
    List<Bateau> Arsenal { get; set; }

    public ArsenalEventArgs(List<Bateau> newArsenal)
    {
        Arsenal = newArsenal;
    }
}