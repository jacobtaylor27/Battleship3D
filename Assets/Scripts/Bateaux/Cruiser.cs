using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : Bateau
{
    public Cruiser()
    {
        Nom = "Cruiser";
        Longueur = 3;
        Maquette = GameObject.Find("Cruiser");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
