using System;
using System.Collections.Generic;

public class OccupationEventArgs : EventArgs
{
    Coordonnées nouvelleCoord { get; set; }

    TypeOccupation nouvelleOccupation { get; set; }

    public OccupationEventArgs(Case caseJeu)
    {
        nouvelleCoord = caseJeu.Coordonnées;
        nouvelleOccupation = caseJeu.TypeOccupation;
    }
}

public class BateauEventArgs : EventArgs
{
    Bateau Bateau { get; set; }

    public BateauEventArgs(Bateau bat)
    {
        Bateau.Coups = bat.Coups;
        Bateau.Longueur = bat.Longueur;
    }
}