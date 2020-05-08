using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GestionTirs : MonoBehaviour
{
    Coordonnées CoordVisée { get; set; }
    public Vector3 PositionVisée { get; set; }
    private Camera CamBot { get; set; }
    Vector3 Origine { get; set; }
    float Delta { get; set; }
    private KeyCode Tirer { get; set; }
    Vector3 mousePosition;
    RaycastHit hit;
    Ray ray;

    GameObject plane;
    private void Start()
    {
        Origine = GetComponent<GénérerCollidersGrille>().OrigineNPC;
        Delta = GetComponent<GénérerCollidersGrille>().Delta;

        Tirer = KeyCode.Mouse0;//click gauche

        plane = GameObject.Find("WaterFloor");
        CamBot = Camera.allCameras.ToList<Camera>().Find(x=>x.name == "NPCCam"); // Caméra Bot --> trouvée avec Debug donc à changer si on rajoute des cams.

        // Trouver gameObjectGrille et set la hauteur voulue par rapport à la grille comme étant yAxis
        float zAxis = plane.transform.position.z;
        mousePosition.z = zAxis;

        mousePosition = CamBot.ScreenToWorldPoint(Input.mousePosition);
    }
    private void Awake()
    {
        enabled = false;
    }
    void Update()
    {
        ray = CamBot.ScreenPointToRay(Input.mousePosition);
        //float distance = Mathf.Sqrt(Mathf.Pow(CamBot.transform.position.x, 2) + Mathf.Pow(CamBot.transform.position.y, 2) + Mathf.Pow(CamBot.transform.position.z, 2));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Mettre un tag pour tous les colliders et générer procéduralement les colliders
            if (hit.collider.gameObject.name == "Tuile(Clone)")
                if (Input.GetKeyDown(Tirer))
                {
                    CoordVisée = hit.collider.gameObject.GetComponent<InformationTuile>().Case.Coordonnées;
                    PositionVisée = new Vector3(Origine.x - Delta * CoordVisée.Colonne - Delta / 2, Origine.y, Origine.z + Delta * CoordVisée.Rangée + Delta / 2);
                    ExitState();
                }
        }
    }
    public void EnterState()
    {
        enabled = true;
    }
    private void ExitState()
    {
        enabled = false;
        GestionnaireJeu.manager.PositionVisée = PositionVisée;
        GestionnaireJeu.manager.CoordVisée = CoordVisée;

        GestionnaireJeu.manager.DéterminerRésultatTir();
    }
}
