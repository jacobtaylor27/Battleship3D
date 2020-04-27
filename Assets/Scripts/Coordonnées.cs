using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordonnées
{
    public int Rangée { get; set; }
    public int Colonne { get; set; }

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

    public override string ToString() => $"{Rangée},{Colonne}";
}
