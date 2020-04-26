using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordonnées
{
    // ajouter vérification
    public int Rangée { get; set; }
    public int Colonne { get;
 set; }

    public Coordonnées()
    {
        Rangée = 0;
        Colonne = 0;
    }

    public Coordonnées(int rangée, int colonne)
    {
        Rangée = rangée;
        Colonne = colonne;
    }

    public bool Equals(Coordonnées obj)//aucune référence (26 avril)
    {
        return Colonne == obj.Colonne && Rangée == obj.Rangée;
    }

    public override string ToString()
    {
        return $"{Rangée},{Colonne}";
    }
}
