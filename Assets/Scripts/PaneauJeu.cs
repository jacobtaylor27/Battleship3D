using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaneauJeu
{
    public List<Case> Cases { get; set; }

    public PaneauJeu()
    {
        Cases = new List<Case>();
        for (int i = 0; i <= 10; i++)
        {
            for (int j = 0; j <= 10; j++)
            {
                Cases.Add(new Case(i, j));
            }
        }
    }
}
