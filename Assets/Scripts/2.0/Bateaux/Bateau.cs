using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bateau
{
    public string Nom { get; set; }
    public int Longueur { get; set; }
    public int Coups { get; set; }
    public GameObject Maquette { get; set; }
    public TypeOccupation TypeOccupation { get; set; }
    public bool EstCallé { get { return Coups >= Longueur; } }
}
