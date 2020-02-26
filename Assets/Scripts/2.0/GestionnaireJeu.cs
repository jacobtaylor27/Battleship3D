﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionnaireJeu : MonoBehaviour
{
    Joueur Joueur { get; set; }
    PaneauJeu BateauxJoueur { get; set; }
    PaneauTirs TirsJoueur { get; set; }
    Bot Bot { get; set; }
    PaneauJeu BateauxBot { get; set; }
    PaneauTirs TirsBot { get; set; }
    KeyCode Placer { get; set; }
    KeyCode Tourner { get; set; }
    bool Fait { get; set; }

    void Start()
    {
        // Linker les callbacks (+=)

        Joueur = new Joueur();
        Bot = new Bot();
        Placer = KeyCode.Mouse0; // CLICK GAUCHE
        Tourner = KeyCode.R;
        BateauxJoueur = new PaneauJeu();
        TirsJoueur = new PaneauTirs();
        TirsBot = new PaneauTirs();
        BateauxBot = new PaneauJeu();
    }
    private void Update()
    {
        PlacerBateauxJoueur();
        Bot.Placer();
    }
    public void PlacerBateauxJoueur()
    {
        //à mettre dans joueur(je crois)
        var positionCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var b in Joueur.Arsenal)
        {
            Fait = false;
            while (!Fait)
            {
                Instantiate(b.Maquette, new Vector3(positionCamera.x, 1f, positionCamera.z), Quaternion.identity);
                if (Input.GetKeyDown(Placer))
                {

                    Fait = true;
                }
            }
        }
    }

    private void GestionPlacement()
    {
        //Mettre un bool EnPlacement pour arrêter cette méthode après le placement
        float zAxis;
        Vector3 mousePosition;
        RaycastHit hit;
        Ray ray;
        GameObject bateau;
        GameObject Grille;

        Grille = GameObject.Find("WaterFloor");
        zAxis = Grille.transform.position.z;
        mousePosition.z = zAxis;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Changer la valeur de y pour hauteur voulue
        test = Instantiate(cube, new Vector3(mousePosition.x, 1f, mousePosition.z), Quaternion.identity);

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = Mathf.Sqrt(Mathf.Pow(Camera.main.transform.position.x, 2) + Mathf.Pow(Camera.main.transform.position.y, 2) + Mathf.Pow(Camera.main.transform.position.z, 2));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Mettre un tag pour tous les colliders et générer procéduralement les colliders
            if (hit.collider.gameObject.name == "Tuile(Clone)")
                test.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, 1f, hit.collider.gameObject.transform.position.z);
        }
    }

}