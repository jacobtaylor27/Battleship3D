﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class Joueur
{
    public Paneau PaneauJeu { get; set; }
    public Paneau PaneauTirs { get; set; }
    public List<Bateau> Arsenal { get; set; }
    public int BateauxRestants { get; set; }
    bool APerdu { get { return Arsenal.All(x => x.EstCoulé); } }
    public event EventHandler<BateauEventArgs> BateauDétruit;
    public event EventHandler<BateauEventArgs> PartieTerminée;

    public Joueur()
    {
        BateauxRestants = 5;
        PaneauJeu = new Paneau();
        PaneauTirs = new Paneau();
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
            return (GameObject)Resources.Load("Prefabs/PrefabNavires/" + nom);

        else if (val == 1)
            return (GameObject)Resources.Load("Prefabs/" + nom);

        else
            return null;
    }

    void onBateauDétruit(BateauEventArgs dataBateau) => BateauDétruit?.Invoke(this, dataBateau);

    void onPartieTerminée(BateauEventArgs dataBateau) => BateauDétruit?.Invoke(this, dataBateau);

    public void SeFaireToucher(Bateau b)
    {
        Arsenal[Arsenal.FindIndex(x => x == b)].PerdreVie();
        if (Arsenal[Arsenal.FindIndex(x => x == b)].EstCoulé)
        {
            BateauxRestants--;
            onBateauDétruit(new BateauEventArgs(Arsenal[Arsenal.FindIndex(x => x == b)]));
            if (APerdu)
            {
                GestionnaireAccueil.accueil.TexteTitre.text = "Yo";
                onPartieTerminée(new BateauEventArgs(Arsenal[Arsenal.FindIndex(x => x == b)]));
                SceneManager.LoadScene("Accueil");
            }

        }
    }

    public virtual void Tirer() => GestionnaireJeu.manager.TirerJoueur();
}
