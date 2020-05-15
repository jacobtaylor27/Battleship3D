using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Case
{
    public TypeOccupation TypeOccupation { get; set; }
    public Coordonnées Coordonnées { get; private set; }
    public Vector3 PositionMonde { get; private set; }
    public bool EstOccupé { get { return TypeOccupation == TypeOccupation.Occupé; } }

    static public bool EstTiréRaté(Coordonnées coord)
    {
        return GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Touché ||
               GestionnaireJeu.manager.JoueurActif.PaneauTirs.TrouverCase(coord).TypeOccupation == TypeOccupation.Manqué;
    }

    public Case(int rangée, int colonne)
    {
        TypeOccupation = TypeOccupation.Vide;
        Coordonnées = new Coordonnées(rangée, colonne);
    }

    public Case(Coordonnées coord, TypeOccupation occup, Vector3 position)
    {
        Coordonnées = coord;
        TypeOccupation = occup;
        PositionMonde = position;
    }

    public Case ChangerPosition(Vector3 position)
    {
        PositionMonde = position;
        return this;
    }

    // Pour les tests
    public override string ToString()
    {
        return string.Format("v = ({0}), {1}, {2}", Coordonnées.ToString(), TypeOccupation, PositionMonde.ToString());
    }

}
public static class CaseExtensions
{
    public static Case At(this List<Case> paneaux, int rangée, int colonne)
    {
        if (paneaux.Where(x => x.Coordonnées.Rangée == rangée && x.Coordonnées.Colonne == colonne).Count() != 0)
            return paneaux.Where(x => x.Coordonnées.Rangée == rangée && x.Coordonnées.Colonne == colonne).First();
        else
            return null;
    }

    public static List<Case> Range(this List<Case> paneaux, int rangéeI, int colonneI, int rangéeF, int colonneF)
    {
        return paneaux.Where(x => x.Coordonnées.Rangée >= rangéeI
                                  && x.Coordonnées.Colonne >= colonneI
                                  && x.Coordonnées.Rangée <= rangéeF
                                  && x.Coordonnées.Colonne <= colonneF).ToList();
    }
}
