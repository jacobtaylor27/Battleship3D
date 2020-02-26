using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ce script va être fusionner avec GénérerCollidersGrille.cs
public class CréeGrille : MonoBehaviour
{
    //script qui a pour but de générer les 100 tuiles (chaque tuiles étant un préfab) qui forme la grille du joueur (10 par 10)
    public bool remplir;

    public GameObject PrefabTuile;

    List<GameObject> ListeTuiles = new List<GameObject>();

    //remplir la liste de tuile
    //source https://docs.unity3d.com/ScriptReference/Gizmos.html
    void OnDrawGizmos()
    {
        if (remplir == true && PrefabTuile != null)
        {
            ViderListeTuiles();
            CréeTuile();
        }
    }
    //"Détruit" les tuiles existantes (être certains qu'il n'y a pas d'ancienne tuiles dans la list)
    public void ViderListeTuiles()
    {
        for (int i = 0; i < ListeTuiles.Count; i++)
        {
            DestroyImmediate(ListeTuiles[i]);
        }
        ListeTuiles.Clear();
    }
    //Cree les tuiles aux coordonnés
    public void CréeTuile()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //hauteur = 0 
                Vector3 PositionTuile = new Vector3(transform.position.x + i, 0, transform.position.z + j);
                //instantier les tuiles au position au transform du parent (Playerfield P1 par exemple qui est de 0,0,0)

                GameObject tuile = Instantiate(PrefabTuile, PositionTuile, Quaternion.identity,transform);
                //afin de définir la pos des sprites 
                tuile.GetComponent<InformationTuile>().DéfinirInformationTuile(i, j);
                //ajouter la tuile dans le tableau
                ListeTuiles.Add(tuile);
            }

        }
    }
}
