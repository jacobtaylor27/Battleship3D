using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Joueur
{
    public PaneauJeu PaneauJeu { get; set; }
    public PaneauTirs PaneauTirs { get; set; }
    public List<Bateau> Arsenal { get; set; }
    public bool aPerdu { get { return Arsenal.All(x => x.EstCallé); } }

    public Joueur()
    {
        PaneauJeu = new PaneauJeu();
        PaneauTirs = new PaneauTirs();
        Arsenal = new List<Bateau>()
        {
            new Destroyer(),
            new Cruiser(),
            new Carrier(),
            new Battleship(),
            new Submarine()
        };
    }

    public void SeFaireTouché(Bateau b)
    {
        Arsenal[Arsenal.FindIndex(x => x == b)].PerdreVie();
    }
}
