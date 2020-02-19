using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Case
{
    public TypeOccupation TypeOccupation { get; set; }
    public Coordonnées Coordonnées { get; set; }

    public Case(int rangée, int colonne)
    {
        TypeOccupation = TypeOccupation.Vide;
        Coordonnées = new Coordonnées(rangée, colonne);
    }

    public void FairePerdreVieBateau()
    {
        if (TypeOccupation == TypeOccupation.Battleship)
            
    }

    public bool EstOccupé
    {
        get
        {
            return TypeOccupation == TypeOccupation.Battleship
                || TypeOccupation == TypeOccupation.Carrier
                || TypeOccupation == TypeOccupation.Cruiser
                || TypeOccupation == TypeOccupation.Destroyer
                || TypeOccupation == TypeOccupation.Submarine;
        }
    }
}

public static class CaseExtensions
{
    public static Case At(this List<Case> paneaux, int rangée, int colonne)
    {
        return paneaux.Where(x => x.Coordonnées.Rangée == rangée && x.Coordonnées.Colonne == colonne).First();
    }

    public static List<Case> Range(this List<Case> paneaux, int rangéeI, int colonneI, int rangéeF, int colonneF)
    {
        return paneaux.Where(x => x.Coordonnées.Rangée >= rangéeI
                                  && x.Coordonnées.Colonne >= colonneI
                                  && x.Coordonnées.Rangée <= rangéeF
                                  && x.Coordonnées.Colonne <= colonneF).ToList();
    }
}
