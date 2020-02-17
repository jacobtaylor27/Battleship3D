using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurLocal : Joueur
{

    public void PlacerBateaux()
    {
        foreach (var b in Arsenal)
        {
            InstantierBateau();
        }
    }

    private void InstantierBateau()
    {

    }

    public void Tirer()
    {

    }
}
