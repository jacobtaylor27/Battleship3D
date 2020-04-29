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

    public static bool operator !=(Coordonnées coord1,Coordonnées coord2)
    {
        return (coord1.Rangée != coord2.Rangée && coord1.Colonne != coord2.Colonne);
    }
    public static bool operator ==(Coordonnées coord1, Coordonnées coord2)
    {
        return (coord1.Rangée == coord2.Rangée && coord1.Colonne == coord2.Colonne);
    }
}
