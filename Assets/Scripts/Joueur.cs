using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class Joueur
{
    public PaneauJeu PaneauJeu { get; set; }
    public PaneauTirs PaneauTirs { get; set; }
    public List<Bateau> Arsenal { get; set; }
    public bool aPerdu { get { return Arsenal.All(x => x.EstCallé); } }
    
    public event EventHandler<BateauEventArgs> BateauDétruit;

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
    public TypeOccupation DéterminerRésultatTir(Coordonnées emplacementCase)
    {
        TypeOccupation valeurRetour;

        if (PaneauJeu.Cases.Find(x => x.Coordonnées.Equals(emplacementCase)).TypeOccupation == TypeOccupation.Occupé)
            valeurRetour = TypeOccupation.Touché;

        else
            valeurRetour = TypeOccupation.Manqué;

        return valeurRetour;
    }

    public void SeFaireTouché(Bateau b)
    {
        Arsenal[Arsenal.FindIndex(x => x == b)].PerdreVie();
    }
}
