using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Bateau
{
    public string Nom { get; set; }
    public int Longueur { get; set; }
    public int Coups { get; set; }
    
    public GameObject BateauPrefab;
    
    public GameObject BateauCube;
    public List<Case> CasesOccupées { get; set; } 
    public TypeOccupation TypeOccupation { get; set; }
    public bool EstCallé { get { return Coups >= Longueur; } }
    public void PerdreVie() => Coups++;
}
