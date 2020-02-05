using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bateau
{
    int longueur;
    public Vector3 Direction { get; set; }

    public int CptTouchés { get; set; }
    
    public Vector3 Origine { get; set; }

    public bool EstDétruit { get; set; }

    public int Longueur
    {
        get { return longueur; }
        set
        {
            if (value <= 5 && value >= 2)
                longueur = value;
        }
    }

    public Bateau()
    {
        Direction = Vector3.left;
        Origine = Vector3.zero;
        Longueur = 2;
        CptTouchés = 0;
        EstDétruit = false;
    }
    public Bateau(int longueur)
    {
        Direction = Vector3.left;
        Origine = Vector3.zero; // trouver la position de départ du bateau (centre de la grille)
        Longueur = longueur;
        CptTouchés = 0;
        EstDétruit = false;
    }


}