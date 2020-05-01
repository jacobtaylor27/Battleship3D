using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

static public class GestionnaireCouleur 
{
    static public void ModifierCouleur()
    {
        List<InformationTuile> infoTuile = GameObject.Find("ListeTuiles").GetComponentsInChildren<InformationTuile>().ToList();

        if(GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Touché)
            infoTuile.FindAll(x => x.Case.Coordonnées == GestionnaireJeu.manager.CoordVisée).Find(x=>x.Case.PositionMonde == GestionnaireJeu.manager.PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/Touché");
        else if (GestionnaireJeu.manager.OccupÀCoordVisée == TypeOccupation.Manqué)
            infoTuile.FindAll(x => x.Case.Coordonnées == GestionnaireJeu.manager.CoordVisée).Find(x => x.Case.PositionMonde == GestionnaireJeu.manager.PositionVisée)
                .GetComponent<MeshRenderer>().material = (Material)Resources.Load("Material/noir");
    }
}
