using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionAnimation : MonoBehaviour
{
    const float TempsAnimation = 7f;
    const float AccélérationGravitationnelle = -9.80f;
    const float HauteurMax = 500f;

    public GameObject projectile;
    GameObject[] Canons { get; set; }
    GameObject Affut { get; set; }
    Vector3 VitesseInitiale { get; set; }
    Vector3 VCanonInit { get; set; }
    Vector3 VCanonFinal { get; set; }
    float AngleX { get; set; }
    float AngleY { get; set; }

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
        float VitesseIY = (HauteurMax - AccélérationGravitationnelle * Mathf.Pow(TempsAnimation,2) / 2) / TempsAnimation; // vitesse initiale en y;

        AngleX = Mathf.Atan(VitesseIY / VitesseIX);
        VitesseInitiale = new Vector3(VitesseIX * Mathf.Cos(AngleY), VitesseIY, VitesseIX * Mathf.Sin(AngleY));
        test = 0;



    }

    private void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (test < 60)
            Affut.transform.Rotate(-AngleX / 60f, AngleY / 60f, 0);
        else
        {
            GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4]);
            GameObject.Instantiate(projectile, Affut.GetComponentsInChildren<Transform>()[4]).GetComponent<Rigidbody>().velocity = VitesseInitiale;
            ExitState();
        }

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
