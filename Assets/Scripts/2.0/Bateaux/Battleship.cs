using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship : Bateau
{
    public Battleship()
    {
        Nom = "Battleship";
        Longueur = 4;
        Maquette = GameObject.Find("Battleship");
        TypeOccupation = TypeOccupation.Battleship;
    }
}
