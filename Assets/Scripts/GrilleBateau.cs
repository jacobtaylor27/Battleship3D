using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
using System.IO;

// Ce script va être fusionner avec PaneauJeu.cs
public class GrilleBateau
{
    public GameObject grille;
    private Vector2 dimensionsGrillePhysique = new Vector2(10,10);//grandeur de la grille physiquement
    const int dimensions = 10;//nombre de cases
    ÉtatOccupation[,] Bateaux = new ÉtatOccupation[dimensions, dimensions];
    List<Bateau> BateauxPlacés { get; set; }

    public GrilleBateau()
    {
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                Bateaux[i, j] = ÉtatOccupation.Vide;
            }
        }
    }
   

    public ÉtatOccupation this[int index1, int index2]
    {
        get { return Bateaux[index1, index2]; }
        private set { Bateaux[index1, index2] = value; }
    }

    public void AjouterBateau(Bateau bat)
    {
        Coordonnées coordOrigine = new Coordonnées();


        //Prendre position du bateau dans grille et y placer l'origine
        ConvertirPositionToCoordonnées(bat.Origine);
        //Changer état pour ÉtatBateau.Actif à l'origine
        Bateaux[coordOrigine.X,coordOrigine.Y] = ÉtatOccupation.Occupé;
        //Changer état à ÉtatActif pour toutes les cases selon bat.Direction à partir de l'origine jusqu'a longueur
        for(int i = 1; i < bat.Longueur; i++)
        {
            //S'assurer que bateau respecte les dimensions
            Bateaux[coordOrigine.X + i * (int)bat.Direction.x, coordOrigine.Y + i * (int)bat.Direction.y] = ÉtatOccupation.Occupé;
        }
    }

    public Coordonnées ConvertirPositionToCoordonnées(Vector3 origine)
    {
        Vector2 grandeurCases = new Vector2(dimensionsGrillePhysique.x / dimensions, dimensionsGrillePhysique.y / dimensions);
        float X = Math.Abs(origine.x - grille.transform.position.x);
        float Y = Math.Abs(origine.y - grille.transform.position.y);
        int posX = (int)(X / grandeurCases.x);
        int posY = (int)(Y / grandeurCases.y);

        return new Coordonnées(posX, posY);
    }

}
