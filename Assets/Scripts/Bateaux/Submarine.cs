﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : Bateau
{
    public Submarine()
    {
        Nom = "Submarine";
        Longueur = 3;
        BateauPrefab = GameObject.Find("Submarine");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
