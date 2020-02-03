﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bateau
{
    int longueur;

    public int cptTouchés { get; set; }

    Vector3 Direction { get; set; }
    
    Vector3 Origine { get; set; }
    
    ÉtatOccupation ÉtatInitial { get; set; }

    public int Longueur
    {
        get { return longueur; }
        set
        {
            if (value <= 5 && value >= 2)
                longueur = value;
        }
    }

    public Bateau(int longueur)
    {
        Direction = Vector3.left;
        Origine = Vector3.zero; // trouver la position de départ du bateau
        Longueur = longueur;
        cptTouchés = 0;
        ÉtatInitial = ÉtatOccupation.Vide;
    }
}