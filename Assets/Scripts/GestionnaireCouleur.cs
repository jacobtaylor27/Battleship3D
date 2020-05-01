using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GestionnaireCouleur 
{
    static public void ModifierCouleur()
    {
        List<InformationTuile> infoTuile = new List<InformationTuile>(GameObject.Find("ListeTuile").GetComponentsInChildren<InformationTuile>());

        if(GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Occupé)
            infoTuile.FindAll(x => x.Case.Coordonnées == GestionnaireJeu.manager.CoordVisée).Find(x=>x.Case.PositionMonde == GestionnaireJeu.manager.PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material) Resources.Load("Material/TestProjectile");
        else if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Manqué)
            infoTuile.FindAll(x => x.Case.Coordonnées == GestionnaireJeu.manager.CoordVisée).Find(x => x.Case.PositionMonde == GestionnaireJeu.manager.PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");

    }

    




}
