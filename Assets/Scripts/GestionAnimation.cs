using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float tempsAnimation = 2f;
    GameObject[] Canons { get; set; }
    GameObject Affut { get; set; }
    Vector3 VitesseInitiale { get; set; }
    Vector3 PositionCanon { get; set; }
    Vector3 VCanonInit { get; set; }
    Vector3 VCanonFinal { get; set; }
    float AngleX { get; set; }
    float AngleY { get; set; }
    float[,] MatriceRotationX { get; set; }
    float[,] MatriceRotationY { get; set; }
    int test { get; set; }


    private void OnEnable()
    {
        Canons = GameObject.FindGameObjectsWithTag("Canon");
        Debug.Log(GestionnaireJeu.manager.JoueurActif);
        Debug.Log(GestionnaireJeu.manager.Tour);

        if (GestionnaireJeu.manager.Tour % 2 == 0)
            Affut = Canons[0].GetComponentsInChildren<Transform>()[1].gameObject;
        else
            Affut = Canons[1].GetComponentsInChildren<Transform>()[1].gameObject;

        VCanonInit = Affut.GetComponentsInChildren<Transform>()[4].position - Affut.transform.position;

        VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.transform.position;
        VCanonFinal = new Vector3(VCanonFinal.x, 0, VCanonFinal.z);
        AngleY = Mathf.Deg2Rad * Vector3.Angle(VCanonInit, VCanonFinal);

        CréerMatricesRotation();
        FaireRotationVecteur();


    }

    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (test == 4)
            ExitState();
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState()
    {
        enabled = false;
        GestionnaireJeu.manager.NextPlayer();
    }
    private void CréerMatricesRotation()
    {
        MatriceRotationX = new float[3, 3] { { 1, 0, 0 }, { 0, Mathf.Cos(AngleX), Mathf.Sin(AngleX) }, { 0, -Mathf.Sin(AngleX), Mathf.Cos(AngleX) } };
        MatriceRotationY = new float[3, 3] { { Mathf.Cos(AngleY), 0, -Mathf.Sin(AngleY) }, { 0, 1, 0 }, { Mathf.Sin(AngleY), 0, Mathf.Cos(AngleY) } };
    }
    private void FaireRotationVecteur()
    {
        Vector3 tempVect = VCanonInit;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tempVect[i] += tempVect[i] * MatriceRotationY[j, i];
            }
        }

        Affut.GetComponentsInChildren<Transform>()[4].position = tempVect;
        //affut.transform.rotation = new quaternion(matricetransformée[1], affut.transform.rotation.y, matricetransformée[0], affut.transform.rotation.w);
        //Affut.transform.localEulerAngles = new Vector3(90,0,0);
        test = 4;

    }
}
