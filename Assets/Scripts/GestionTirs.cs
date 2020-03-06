using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionTirs : MonoBehaviour
{
    Coordonnées CoordVisée { get; set; }
    public Vector3 PositionVisée { get; set; }
    float delta { get; set; }
    Vector3 origine { get; set; }

    KeyCode Tirer { get; set; }

    float zAxis;
    Vector3 mousePosition;

    RaycastHit hit;
    Ray ray;

    GameObject plane;
    public void EnterState()
    {
        enabled = true;
    }
    public void ExitState()
    {

    }
    private void Start()
    {
        Tirer = KeyCode.Mouse0;//click gauche

        float delta = GetComponent<GénérerCollidersGrille>().Delta;//pas sur que ca marche de meme mais au moins j'ai le principe
        Vector3 origine = GetComponent<GénérerCollidersGrille>().OrigineNPC;//same here


        plane = GameObject.Find("WaterFloor");
        // Trouver gameObjectGrille et set la hauteur voulue par rapport à la grille comme étant yAxis
        zAxis = plane.transform.position.z;
        mousePosition.z = zAxis;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = Mathf.Sqrt(Mathf.Pow(Camera.main.transform.position.x, 2) + Mathf.Pow(Camera.main.transform.position.y, 2) + Mathf.Pow(Camera.main.transform.position.z, 2));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Mettre un tag pour tous les colliders et générer procéduralement les colliders
            if (hit.collider.gameObject.name == "Tuile(Clone)")
                if (Input.GetKeyDown(Tirer))
                {
                    PositionVisée = hit.transform.position;
                    CoordVisée = hit.collider.gameObject.GetComponent<InformationTuile>().coordGrille;
                }
                   
        }
    }
}
