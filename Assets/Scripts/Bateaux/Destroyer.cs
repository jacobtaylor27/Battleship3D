﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : Bateau
{
    public Destroyer()
    {
        Nom = "Destroyer";
        Longueur = 3;
        BateauPrefab = GameObject.Find("Destroyer");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
