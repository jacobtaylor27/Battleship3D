using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComportementTuile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject.GetComponent<BoxCollider>());

        if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Touché)
            gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/Touché");
        else if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Manqué)
           gameObject.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");
    }
}
