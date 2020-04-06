using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float TempsAnimation = 3f;
    const float AccélérationGravitationnelle = -9.80f;
    const float HauteurMax = 500f;

    GameObject lol { get; set; }
    public GameObject projectile;
    GameObject[] Canons { get; set; }
    GameObject Affut { get; set; }
    float Force { get; set; }
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

        VCanonFinal = GestionnaireJeu.manager.PositionVisée - Affut.transform.position;
        VCanonFinal = new Vector3(VCanonFinal.x, 0, VCanonFinal.z);
        AngleY = Vector3.SignedAngle(VCanonInit, VCanonFinal, Vector3.up);

        float VitesseIX = VCanonFinal.magnitude / TempsAnimation; // vitesse initiale en x
        float VitesseIY = (HauteurMax - AccélérationGravitationnelle * Mathf.Pow(TempsAnimation, 2) / 2) / TempsAnimation; // vitesse initiale en y;

        AngleX = Mathf.Rad2Deg * Mathf.Atan(VitesseIY / VitesseIX);
        //VitesseInitiale = new Vector3(VitesseIX * Mathf.Sin(AngleY * Mathf.Deg2Rad), VitesseIY, VitesseIX * Mathf.Cos(AngleY * Mathf.Deg2Rad));
        //VitesseInitiale = Mathf.Sqrt(Mathf.Pow(VitesseIX,2) + Mathf.Pow(VitesseIY,2));
        test = 0;

        //Force = (projectile.GetComponent<Rigidbody>().mass * VitesseIY / 0.2f)*Mathf.Cos(AngleX*Mathf.Deg2Rad);
        Force = projectile.GetComponent<Rigidbody>().mass *-((VitesseIY+AccélérationGravitationnelle*TempsAnimation)- VitesseIY) * Mathf.Sin(AngleX * Mathf.Deg2Rad);
        //VitesseI = Mathf.Sqrt((VCanonFinal).magnitude*-AccélérationGravitationnelle/Mathf.Sin(2*AngleX*Mathf.Deg2Rad));
        VitesseI = new Vector3(0, HauteurMax / TempsAnimation*0.3f, VCanonFinal.magnitude / TempsAnimation*0.3f).magnitude;
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
                lol.GetComponent<Rigidbody>().AddForce(transform.TransformVector(lol.transform.forward * VitesseI),ForceMode.Impulse);
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
