﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Case
{
    public TypeOccupation TypeOccupation { get; set; }
    public Coordonnées Coordonnées { get; private set; }
    static public bool EstTiréRaté(Coordonnées coord) => GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Touché ||
                                 GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Manqué;

    public Case(int rangée, int colonne)
    {
        TypeOccupation = TypeOccupation.Vide;
        Coordonnées = new Coordonnées(rangée, colonne);
    }

    public Case(Coordonnées coord, TypeOccupation occup)
    {
        Coordonnées = coord;
        TypeOccupation = occup;
    }

    public bool EstOccupé
    {
        get
        {
            return TypeOccupation == TypeOccupation.Occupé;
        }
    }

    public override string ToString()
    {
        return string.Format("v = ({0},{1}), {2}", Coordonnées.Rangée, Coordonnées.Colonne, TypeOccupation);
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
