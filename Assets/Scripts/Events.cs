using System;

public class OccupationEventargs : EventArgs
{
    Coordonnées nouvelleCoord { get; set; }

    TypeOccupation nouvelleOccupation { get; set; }

    public OccupationEventargs(Coordonnées coord, TypeOccupation occup)
    {
        nouvelleCoord = coord;
        nouvelleOccupation = occup;
    }
}
