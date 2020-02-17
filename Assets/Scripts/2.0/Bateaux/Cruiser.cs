using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : Bateau
{
    public Cruiser()
    {
        Nom = "Cruiser";
        Longueur = 3;
        Maquette = /*Prefab*/;
        TypeOccupation = ÉtatOccupation.Cruiser;
    }
}
