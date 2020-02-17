using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Bateau
{
    public Carrier()
    {
        Nom = "Carrier";
        Longueur = 5;
        TypeOccupation = ÉtatOccupation.Carrier;
    }
}
