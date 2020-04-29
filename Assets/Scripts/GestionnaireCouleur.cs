using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionnaireCouleur : MonoBehaviour
{
    static public void ModifierCouleur()
    {
        if(InformationTuile.infoTuile.GetComponent<InformationTuile>().Case.Coordonnées == GestionnaireJeu.manager.CoordVisée)
        {
            if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Touché)
                InformationTuile.infoTuile.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/TestProjectile");
            else if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Manqué)
                InformationTuile.infoTuile.GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");
        }
    }
}
