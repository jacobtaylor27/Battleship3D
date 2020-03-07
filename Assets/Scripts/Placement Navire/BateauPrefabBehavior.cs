using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauPrefabBehavior : MonoBehaviour
{
    public int LongueurBateau;
    int NbCoups;
    private void Start()
    {
        NbCoups = LongueurBateau;
    }
    bool EstCoulé()
    {
        return LongueurBateau <= 0;

    }
    public bool EstTouché()
    {
        return (NbCoups < LongueurBateau && NbCoups > 0);
    }
    public void PrendreDommage()
    {
        if (EstCoulé())
        {
            // À Faire
            // Send le message au Gamemanager
            // MeshRenderer -- Dévoiler le Bateau 
        }


    }

}
