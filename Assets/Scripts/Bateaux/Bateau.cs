using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bateau
{
    //public string Nom { get; set; }
    public TypeOccupation TypeOccupation { get; set; }
    public int Longueur { get; set; }
    public int Coups { get; set; }
    public GameObject PrefabBateau;
    public GameObject PrefabCube;
    public List<Case> CasesOccupées { get; set; }
    public bool EstCoulé { get { return Coups >= Longueur; } }
    public bool EstPlacé { get; set; }
    public void PerdreVie() => Coups++;

    public Bateau(int n, GameObject prefab, GameObject cube)
    {
        Longueur = n;
        PrefabBateau = prefab;
        PrefabCube = cube;
        EstPlacé = false;
        CasesOccupées = new List<Case>(n);
    }
}
