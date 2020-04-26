using System;
using System.Collections.Generic;

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

public class BateauEventArgs : EventArgs
{
    Bateau Bateau { get; set; }

    public BateauEventArgs(Bateau bat)
    {
        Bateau = new Bateau(bat.Longueur, bat.PrefabBateau, bat.PrefabCube);
        Bateau.Coups = bat.Coups;
    }
}