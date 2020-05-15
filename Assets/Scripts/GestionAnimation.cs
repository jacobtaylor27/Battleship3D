﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float TempsAnimation = 5f;
    const float AccélérationGravitationnelle = -9.80f;

    Camera AnimationBot {  get; set; }
    Camera AnimationJoueur { get; set; }
    public Camera CamBot { get; private set; }
    public Camera CamJoueur { get; private set; }

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
    float TempsÉcoulé { get; set; }
    float[,] MatriceRotationY { get; set; }
    float[,] MatriceRotationX { get; set; }

    private void OnEnable()
    {
        if (GetComponent<ControlleurInterface>().AnimationEstActivée)
        {
            foreach (var Camera in Camera.allCameras)
            {
                Camera.enabled = false;
            }
            if (GestionnaireJeu.manager.DéterminerJoueurActif() == "Bot")
                AnimationBot.enabled = true;
            if (GestionnaireJeu.manager.DéterminerJoueurActif() == "Joueur")
                AnimationJoueur.enabled = true;
        }

        Affut = GestionnaireJeu.manager.CanonActif;

        VCanonInit = Affut.GetComponentsInChildren<Transform>()[4].position - Affut.transform.position;
        VCanonInit = new Vector3(VCanonInit.x, 0, VCanonInit.z);

        VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.transform.position;

        //Angle pour rotation autour de Y
        AngleY = Vector3.SignedAngle(VCanonInit, VCanonFinal, Vector3.up);

        UpdateMatricesRotation();
        Vector3 tempBoucheCanon = RotateVector(VCanonInit, MatriceRotationY);

        float portée = new Vector3(VCanonFinal.x - tempBoucheCanon.x, 0, VCanonFinal.z - tempBoucheCanon.z).magnitude;
        float hauteur = Mathf.Abs(VCanonFinal.y - tempBoucheCanon.y);

        CalculerVitesseEtAngleX(portée, hauteur);

        UpdateMatricesRotation();
        Vector3 tempBoucheCanon2 = RotateVector(tempBoucheCanon, MatriceRotationX);

        portée = new Vector3(VCanonFinal.x - tempBoucheCanon.x + (tempBoucheCanon.x - tempBoucheCanon2.x), 0, VCanonFinal.z - tempBoucheCanon.z + (tempBoucheCanon.z - tempBoucheCanon2.z)).magnitude;
        hauteur += Mathf.Abs(tempBoucheCanon2.y);
        CalculerVitesseEtAngleX(portée, hauteur);

        CptFrame = 0;
    }

    private void CalculerVitesseEtAngleX(float portée, float hauteur)
    {
        //Vitesse en angle
        VitesseI = Mathf.Sqrt(Mathf.Pow((portée / TempsAnimation), 2) + Mathf.Pow((GestionnaireJeu.manager.PositionVisée.y - 0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) - hauteur) / TempsAnimation, 2));

        //Angles possible
        float Angle1 = Mathf.Acos(portée / (VitesseI * TempsAnimation));
        float Angle2 = Mathf.Asin((-0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) - Affut.transform.position.y) / (VitesseI * TempsAnimation));

        //Angle pour rotation autour de X
        AngleX = Mathf.Min(Angle1, Angle2) * Mathf.Rad2Deg;
    }

    private void Awake()
    {
        List<Camera> cameras = GameObject.Find("WaterFloor").GetComponentsInChildren<Camera>().ToList();

        CamBot = cameras.Find(c => c.name == "NPCCam");
        CamJoueur = cameras.Find(c => c.name == "PlayerGridCam");
        AnimationBot = cameras.Find(c => c.name == "CamAnimationBot");
        AnimationJoueur = cameras.Find(c => c.name == "CamAnimationJoueur");

        AnimationBot.enabled = false;
        AnimationJoueur.enabled = false;
        enabled = false;
    }

    void Update()
    {
        if (GetComponent<ControlleurInterface>().AnimationEstActivée)
        {
            if (CptFrame < 60)
            {
                Affut.transform.Rotate(Vector3.up, AngleY / 60f, Space.Self);
            }
            else if (CptFrame >= 60 && CptFrame < 120)
            {
                Affut.transform.Rotate(Vector3.left, AngleX / 60f, Space.Self);
            }
            else if (CptFrame >= 120 && CptFrame < 300)
            {
                if (CptFrame == 120)
                {
                    TempsÉcoulé = 0;
                    Missile = Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4].position, Affut.GetComponentsInChildren<Transform>()[4].rotation);
                    Missile.GetComponent<Rigidbody>().AddForce(transform.TransformVector(Missile.transform.forward) * VitesseI, ForceMode.VelocityChange);
                }
            }
            else if (CptFrame >= 300 && CptFrame < 360)
            {
                Affut.transform.Rotate(Vector3.left, -AngleX / 60f, Space.Self);
            }
            else if (CptFrame >= 360 && CptFrame < 420)
            {
                Affut.transform.Rotate(Vector3.up, -AngleY / 60f, Space.Self);
            }
            else if(TempsÉcoulé >= TempsAnimation + 0.5f)
                ExitState();

            CptFrame++;
            TempsÉcoulé += Time.deltaTime;
        }
        else
            ExitState();

    }

    public void EnterState()
    {
        enabled = true;
    }

    public void ExitState()
    {
        foreach (var Camera in Camera.allCameras)
        {
            Camera.enabled = false;
        }
        CamJoueur.enabled = true;
        CamBot.enabled = true;

        enabled = false;
        GestionnaireJeu.manager.PasserAuProchainTour();
    }

    private void UpdateMatricesRotation()
    {
        MatriceRotationX = new float[3, 3] { { 1, 0, 0 }, { 0, Mathf.Cos(AngleX * Mathf.Deg2Rad), -Mathf.Sin(AngleX * Mathf.Deg2Rad) }, { 0, Mathf.Sin(AngleX * Mathf.Deg2Rad), Mathf.Cos(AngleX * Mathf.Deg2Rad) } };

        //Autour de l'axe des Y de Unity (axe des Z en maths)
        MatriceRotationY = new float[3, 3] { { Mathf.Cos(AngleY * Mathf.Deg2Rad), Mathf.Sin(AngleY * Mathf.Deg2Rad), 0 }, { Mathf.Sin(AngleY * Mathf.Deg2Rad), Mathf.Cos(AngleY * Mathf.Deg2Rad), 0 }, { 0, 0, 1 } };
    }

    private Vector3 RotateVector(Vector3 vectorToRotate, float[,] rotationMatrix)
    {
        Vector3 temp = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                temp[i] += rotationMatrix[i, j] * vectorToRotate[j];
            }
        }
        return temp;
    }
}
