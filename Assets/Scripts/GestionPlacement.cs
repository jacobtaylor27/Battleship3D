using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionPlacement : MonoBehaviour
{
    float zAxis;
    Vector3 mousePosition;

    RaycastHit hit;
    Ray ray;

    public GameObject cube;
    GameObject test;
    GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.Find("WaterFloor");
        // Trouver gameObjectGrille et set la hauteur voulue par rapport à la grille comme étant yAxis
        zAxis = plane.transform.position.z;
        mousePosition.z = zAxis;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = Camera.current.ScreenToWorldPoint(Input.mousePosition);
        //Changer la valeur de y pour hauteur voulue
        test = Instantiate(cube,new Vector3(mousePosition.x,1f,mousePosition.z),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = Mathf.Sqrt(Mathf.Pow(Camera.main.transform.position.x, 2) + Mathf.Pow(Camera.main.transform.position.y, 2) + Mathf.Pow(Camera.main.transform.position.z, 2));
        if (Physics.Raycast(ray,out hit, Mathf.Infinity)){
            //Mettre un tag pour tous les colliders et générer procéduralement les colliders
            if (hit.collider.gameObject.name == "Tuile(Clone)")
                test.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, 1f, hit.collider.gameObject.transform.position.z);
        }
    }
}
