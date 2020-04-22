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
    private bool aPerdu { get { return Arsenal.All(x => x.EstCoulé); } }

    public event EventHandler<BateauEventArgs> BateauDétruit;

    public Joueur()
    {
        PaneauJeu = new PaneauJeu();
        PaneauTirs = new PaneauTirs();
        Arsenal = new List<Bateau>()
        {
            new Bateau(2,ChercherPrefab("Carrier(2)",0),ChercherPrefab("BateauCube2",1)),
            new Bateau(3,ChercherPrefab("Destroyer(3)",0),ChercherPrefab("BateauCube3",1)),
            new Bateau(3,ChercherPrefab("Submarine(3)",0),ChercherPrefab("BateauCube3",1)),
            new Bateau(4,ChercherPrefab("Cruiser(4)",0),ChercherPrefab("BateauCube4",1)),
            new Bateau(5,ChercherPrefab("Battleship(5)",0),ChercherPrefab("BateauCube5",1))
        };
    }

    GameObject ChercherPrefab(string nom, int val)
    {
        if (val == 0)
        {
            string chemin = "Prefabs/PrefabNavires/";
            return (GameObject)Resources.Load(chemin + nom);

        }
        else if (val == 1)
        {
            string chemin = "Prefabs/";
            return (GameObject)Resources.Load(chemin + nom);
        }
        else
            return null;
    }

    private void onBateauDétruit(BateauEventArgs dataBateau)
    {
        this.BateauDétruit?.Invoke(this, dataBateau);
    }

    public void SeFaireTouché(Bateau b)
    {
        Arsenal[Arsenal.FindIndex(x => x == b)].PerdreVie();

        if (Arsenal[Arsenal.FindIndex(x => x == b)].EstCoulé)
        {
            onBateauDétruit(new BateauEventArgs(Arsenal[Arsenal.FindIndex(x => x == b)]));

            //if(aPerdu)
            //    onPartieTerminée()

        }
    }

    public virtual void Tirer()
    {
        GestionnaireJeu.manager.TirerJoueur();
    }
}
