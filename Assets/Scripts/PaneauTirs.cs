using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PaneauTirs : PaneauJeu
{
    public event EventHandler<OccupationEventArgs> OccupationModifiée;
    
    
    private void onOccupationModifiée(OccupationEventArgs dataOccupation)
    {
        this.OccupationModifiée?.Invoke(this, dataOccupation);
    }

    public List<Coordonnées> ChercherVoisinsDeTouché()
    {
        List<Case> cases = new List<Case>();

        // enumeration des cases touchées.
        var coups = Cases.Where(x => x.TypeOccupation == TypeOccupation.Touché);

        // pour chaque coups, on cherche les voisins de la case touché que l'on ajoute à la liste cases.
        foreach (var c in coups)
            cases.AddRange(ChercherVoisins(c.Coordonnées).ToList());

        // retoune les coordonnées des voisins du tirs (touché) vide dans une liste.
        return cases.Distinct().Where(x => x.TypeOccupation == TypeOccupation.Vide).Select(x => x.Coordonnées).ToList();
    }

    public List<Case> ChercherVoisins(Coordonnées coordonnées)
    {
        // on passe les coordonnées du coup pour trouver ses voisins
        var rangée = coordonnées.Rangée;
        var colonne = coordonnées.Colonne;

        List<Case> cases = new List<Case>();

        if (colonne > 1)
            cases.Add(Cases.At(rangée, colonne - 1));

        if (rangée > 1)
            cases.Add(Cases.At(rangée - 1, colonne));

        if (colonne < 10)
            cases.Add(Cases.At(rangée, colonne + 1));

        if (rangée < 10)
            cases.Add(Cases.At(rangée + 1, colonne));

        return cases;
    }
}
