using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vagues : MonoBehaviour
{
    [SerializeField]
    float Grandeur = 0.1f;
    [SerializeField]
    float Vitesse = 1.0f;
    Vector3[] HauteuDeBase;

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (HauteuDeBase == null)
            HauteuDeBase = mesh.vertices;

        Vector3[] Sommets = new Vector3[HauteuDeBase.Length];
        
        for (int i = 0; i < Sommets.Length; i++)
        {
            Vector3 Hauteur = HauteuDeBase[i];
            Hauteur.y += Mathf.Sin(Time.time * Vitesse + HauteuDeBase[i].x + HauteuDeBase[i].y + HauteuDeBase[i].z) * Grandeur;
            Sommets[i] = Hauteur;
        }
        mesh.vertices = Sommets;
    }

}
