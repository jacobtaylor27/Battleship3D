using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Bateau
{
    public Carrier()
    {
        Nom = "Carrier";
        Longueur = 2;
        BateauPrefab = GameObject.Find("Carrier");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
