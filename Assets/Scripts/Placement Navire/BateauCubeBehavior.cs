using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauCubeBehavior : MonoBehaviour
{
    //11
    public LayerMask layer;     //Layer à regarder
    RaycastHit hit;
    InformationTuile InfoTuile;

    //Grille GrilleDeJeu;

    //public void SetGrille(Grille  _GrileDeJeu)
    //{
    //    GrilleDeJeu = _GrileDeJeu;
    //}
    public bool SurTuile()
    {
        InfoTuile = GetInfoTuile(); // ajouter si tuile deja occuper
        if (InfoTuile != null)
        {
            return true;
        }
        InfoTuile = null;
        return false;

    }
    public InformationTuile GetInfoTuile() //donne l'info sur se qui se trouve sur la tuile
    {
        Ray ray = new Ray(transform.position, -transform.up);//- pour L'inverser
        if (Physics.Raycast(ray,out hit,1f,layer))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            return hit.collider.GetComponent<InformationTuile>();

        }


        return null;
    }


}
