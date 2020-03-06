using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionTirs : MonoBehaviour
{
    Coordonnées CoordVisée { get; set; } //devra être set selon le ray du joueur ou le tir du bot
    public Vector3 PosisitionVisée { get; set; }
    float delta { get; set; }
    Vector3 origine { get; set; }

    public void EnterState()
    {
        enabled = true;
    }
    public void ExitState()
    {

    }
    private void Start()
    {
        float delta = GetComponent<GénérerCollidersGrille>().Delta;//pas sur que ca marche de meme mais au moins j'ai le principe
        Vector3 origine = GetComponent<GénérerCollidersGrille>().Origine.position;//same here
        PosisitionVisée = new Vector3(origine.x + CoordVisée.Colonne * delta, origine.y + CoordVisée.Rangée * delta, origine.z);//pas sur si les modification doivent être faites sur x et y ou s'il y a le z qqpart
        //si nesséssaire on peut faire une fonction pour l'avoir en return c'est pas très compliqué
    }
}
