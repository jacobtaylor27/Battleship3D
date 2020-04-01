using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : Bateau
{
    public Cruiser()
    {
        Nom = "Cruiser";
        Longueur = 4;
        BateauPrefab = GameObject.Find("Cruiser");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
