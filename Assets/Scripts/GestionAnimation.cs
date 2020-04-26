using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float TempsAnimation = 6f;
    const float AccélérationGravitationnelle = -9.80f;
    const float HauteurMax = 500f;
    const float VitesseInit = 100f;

    GameObject Missile { get; set; }
    public GameObject projectile;
    GameObject[] Canons { get; set; }
    GameObject Affut { get; set; }
    Vector3 VCanonInit { get; set; }
    Vector3 VCanonFinal { get; set; }
    float AngleX { get; set; }
    float AngleY { get; set; }
    float VitesseI { get; set; }
    int CptFrame { get; set; }
    float[,] MatriceRotationY { get; set; }
    float[,] MatriceRotationX { get; set; }


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
        VCanonInit = new Vector3(VCanonInit.x, 0, VCanonInit.z);

        VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.transform.position;
        VCanonFinal = new Vector3(VCanonFinal.x, 0, VCanonFinal.z);

        //Angle pour rotation autour de Y
        AngleY = Vector3.SignedAngle(VCanonInit, VCanonFinal, Vector3.up);

        UpdateMatricesRotation();
        Vector3 tempBoucheCanon = RotateVector(VCanonInit, MatriceRotationY);
        tempBoucheCanon = new Vector3(tempBoucheCanon.x, 0, tempBoucheCanon.z);

        float angleTemp = Vector3.SignedAngle(tempBoucheCanon, VCanonFinal, Vector3.up);

        float portée = (VCanonFinal - tempBoucheCanon).magnitude;


        CalculerVitesseEtAngleX(portée);

        UpdateMatricesRotation();
        Vector3 tempBoucheCanon2 = RotateVector(tempBoucheCanon, MatriceRotationX);

        portée = new Vector3(VCanonFinal.x - tempBoucheCanon.x + (tempBoucheCanon.x - tempBoucheCanon2.x), 0,VCanonFinal.z - tempBoucheCanon.z + (tempBoucheCanon.z - tempBoucheCanon2.z)).magnitude;

        CalculerVitesseEtAngleX(portée);

        CptFrame = 0;


    }

    private void CalculerVitesseEtAngleX(float portée)
    {
        //Vitesse en angle
        VitesseI = Mathf.Sqrt(Mathf.Pow((portée / TempsAnimation), 2) + Mathf.Pow((GestionnaireJeu.manager.PositionVisée.y - 0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) - Affut.GetComponentsInChildren<Transform>()[4].position.y) / TempsAnimation, 2));

        //Angles possible
        float Angle1 = Mathf.Acos(portée / (VitesseI * TempsAnimation));
        float Angle2 = Mathf.Asin((-0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) - Affut.transform.position.y) / (VitesseI * TempsAnimation));

        //Angle pour rotation autour de X
        AngleX = Mathf.Min(Angle1, Angle2) * Mathf.Rad2Deg;
    }

    private void Awake()
    {
        enabled = false;
    }

    void Update()
    {
        //if (test < 60)
        //    Affut.transform.Rotate(-AngleX / 60f, AngleY / 60f, 0);
        //else
        //{
        //    GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4]);
        //    GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4]).GetComponent<Rigidbody>().velocity = VitesseInitiale;
        //    ExitState();
        //}

        if (CptFrame < 30)
        {
            Affut.transform.Rotate(Vector3.up, AngleY /** Mathf.Rad2Deg*/ / 30f, Space.Self);
        }
        else if(CptFrame >= 30 && CptFrame < 60)
        {
            Affut.transform.Rotate(Vector3.left, AngleX  /** Mathf.Rad2Deg*/ / 30f, Space.Self);
        }
        else if(CptFrame >= 60 && CptFrame < 240)
        {
            if (CptFrame == 60)
            {
                Missile = GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4].position, Affut.GetComponentsInChildren<Transform>()[4].rotation);
                //Missile.GetComponent<Rigidbody>().AddForce(transform.TransformVector(lol.transform.forward*Force),ForceMode.Impulse);
                //Missile.GetComponent<Rigidbody>().velocity = transform.TransformVector(lol.transform.forward * Force);
                Missile.GetComponent<Rigidbody>().AddForce(transform.TransformVector(Missile.transform.forward )* VitesseI,ForceMode.VelocityChange);
                //Missile.GetComponent<Rigidbody>().velocity = transform.TransformVector(lol.transform.forward )* VitesseI;
            }
        }
        else if (CptFrame >= 240 && CptFrame < 270)
        {
           Affut.transform.Rotate(Vector3.left, -AngleX / 30f, Space.Self);
        }
        else if (CptFrame >= 270 && CptFrame < 300)
        {
           Affut.transform.Rotate(Vector3.up, -AngleY / 30f, Space.Self);
        }
        else
            ExitState();

        CptFrame++;
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState()
    {
        enabled = false;
    }

    private void UpdateMatricesRotation()
    {
        MatriceRotationX = new float[3, 3] { { 1, 0, 0 }, { 0, Mathf.Cos(AngleX * Mathf.Deg2Rad), -Mathf.Sin(AngleX * Mathf.Deg2Rad) }, { 0, Mathf.Sin(AngleX * Mathf.Deg2Rad), Mathf.Cos(AngleX * Mathf.Deg2Rad) } };
        MatriceRotationY = new float[3, 3] { { Mathf.Cos(AngleY * Mathf.Deg2Rad), 0, Mathf.Sin(AngleY *Mathf.Deg2Rad) }, { 0, 1, 0 }, { -Mathf.Sin(AngleY * Mathf.Deg2Rad), 0, Mathf.Cos(AngleY * Mathf.Deg2Rad) } };
    }

    private Vector3 RotateVector(Vector3 vectorToRotate, float[,] rotationMatrix)
    {
        Vector3 temp = Vector3.zero;

        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                temp[i] += rotationMatrix[i, j] * vectorToRotate[j];
            }
        }

        return temp;
    }

}
