using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float TempsAnimation = 6f;
    const float AccélérationGravitationnelle = -9.80f;
    const float HauteurMax = 500f;
    const float VitesseInit = 100f;

    GameObject lol { get; set; }
    public GameObject projectile;
    GameObject[] Canons { get; set; }
    GameObject Affut { get; set; }
    Vector3 VCanonInit { get; set; }
    Vector3 VCanonFinal { get; set; }
    float AngleX { get; set; }
    float AngleY { get; set; }

    float VitesseI { get; set; }
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
        VCanonInit = new Vector3(VCanonInit.x, 0, VCanonInit.z);

        //VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.transform.position;
        VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.GetComponentsInChildren<Transform>()[4].position;
        VCanonFinal = new Vector3(VCanonFinal.x, 0, VCanonFinal.z);

        //Angle pour rotation autour de Y
        AngleY = Vector3.SignedAngle(VCanonInit, VCanonFinal, Vector3.up);

        //Vitesse en angle
        VitesseI = Mathf.Sqrt(Mathf.Pow((VCanonFinal.magnitude / TempsAnimation),2) + Mathf.Pow((GestionnaireJeu.manager.PositionVisée.y - 0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation,2) - Affut.GetComponentsInChildren<Transform>()[4].position.y) / TempsAnimation,2));

        //Angles possible
        float Angle1 = Mathf.Acos(VCanonFinal.magnitude / (VitesseI * TempsAnimation));
        float Angle2 = Mathf.Asin((-0.5f * AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) - Affut.transform.position.y) / (VitesseI * TempsAnimation));

        //Angle pour rotation autour de X
        AngleX = Mathf.Min(Angle1, Angle2) * Mathf.Rad2Deg;

        test = 0;


    }

    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
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

        if (test < 30)
        {
            Affut.transform.Rotate(Vector3.up, AngleY / 30f, Space.Self);
        }
        else if(test >= 30 && test < 60)
        {
            Affut.transform.Rotate(Vector3.left, AngleX / 30f, Space.Self);
        }
        else if(test >= 60 && test < 240)
        {
            if (test == 60)
            {
                lol = GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4].position, Affut.GetComponentsInChildren<Transform>()[4].rotation);
                //lol.GetComponent<Rigidbody>().AddForce(transform.TransformVector(lol.transform.forward*Force),ForceMode.Impulse);
                //lol.GetComponent<Rigidbody>().velocity = transform.TransformVector(lol.transform.forward * Force);
                lol.GetComponent<Rigidbody>().AddForce(transform.TransformVector(lol.transform.forward )* VitesseI,ForceMode.VelocityChange);
                //lol.GetComponent<Rigidbody>().velocity = transform.TransformVector(lol.transform.forward )* VitesseI;
            }
        }
        else if (test >= 240 && test < 270)
        {
           Affut.transform.Rotate(Vector3.left, -AngleX / 30f, Space.Self);
        }
        else if (test >= 270 && test < 300)
        {
           Affut.transform.Rotate(Vector3.up, -AngleY / 30f, Space.Self);
        }
        else
            ExitState();


        test++;
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


}
