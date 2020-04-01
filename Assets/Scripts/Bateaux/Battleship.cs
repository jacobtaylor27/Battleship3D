using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship : Bateau
{
    public Battleship()
    {
        Nom = "Battleship";
        Longueur = 5;
        BateauPrefab = GameObject.Find("Battleship");
        TypeOccupation = TypeOccupation.Occupé;
    }
}
