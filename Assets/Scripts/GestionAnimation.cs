using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float tempsAnimation = 2f;
    GameObject Canon { get; set; }
    Vector3 VitesseInitiale { get; set; }
    Vector3 RotationCanon { get; set; }
    Vector3 VCanonPosition { get; set; }
    float Angle { get; set; }

    float[,] MatriceRotation;


    void Start()
    {
        GameObject[] tempCanons = GameObject.FindGameObjectsWithTag("Canon");

        if (GestionnaireJeu.manager.Tour % 2 != 0)
            Canon = tempCanons[0];
        else
            Canon = tempCanons[1];

        Debug.Log(Canon.GetComponentsInChildren<Transform>()[1].gameObject.name);

        VCanonPosition = GestionnaireJeu.manager.PositionVisée - Canon.transform.position;
        Angle = Vector3.SignedAngle(Canon.transform.position, VCanonPosition, Vector3.up);

        //Rotation pour mettre le canon et la position sur le même axe;


    }

    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState()
    {

    }
    private void CréerMatriceRotation()
    {
        MatriceRotation = new float[2,2] { { Mathf.Cos(Angle), Mathf.Sin(Angle) }, { -Mathf.Sin(Angle), Mathf.Cos(Angle) } };
    }
    private void MultiplierMatrices()
    {
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++) { }
                   //Canon.transform.position = new Vector3(, Canon.transform.position.y, z);//Changer Canon.transform.position pour la boule.
            }
        }
    }
}
