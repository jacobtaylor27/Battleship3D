﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot
{
    List<Bateau> Arsenal = new List<Bateau>(); // défénir les bateaux

    public void GénérerDirectionAléatoire()
    {
        foreach (Bateau b in Arsenal)
        {
            var dir = Random.Range(0, 4);

            switch (dir)
            {
                case 0:
                    b.Direction = new Vector3(-1, 0, 0);
                    break;
                case 1:
                    b.Direction = new Vector3(0, -1, 0);
                    break;
                case 2:
                    b.Direction = new Vector3(1, 0, 0);
                    break;
                case 3:
                    b.Direction = new Vector3(0, 1, 0);
                    break;
            }
        }
    }

    public void PlacerBateaux()
    {
        foreach (Bateau b in Arsenal)
        {

        }
    }

    public void Tirer()
    {

    }
}