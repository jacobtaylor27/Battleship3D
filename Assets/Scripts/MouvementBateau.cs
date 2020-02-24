using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementBateau : MonoBehaviour
{

    //rigidbody du prefab
    private Rigidbody RigidbodyPrefab;
    
    private float InitialAngle;
    public float AmplitudeMouvement = 20f;
    public float VitesseExecution = 0.5f;
    
    Quaternion RotationInitiale;
    Vector3 PosInitiale;

    void Start()
    {
        //references initial
        RigidbodyPrefab = GetComponent<Rigidbody>();
        RotationInitiale = transform.rotation;

        InitialAngle = Random.Range(-Mathf.PI, Mathf.PI);
        PosInitiale = transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        //Simule un mouvement sinusoidale
        Quaternion Rot = RotationInitiale * Quaternion.Euler(0, 0, +AmplitudeMouvement * Mathf.Sin(VitesseExecution * Time.fixedTime + InitialAngle));
        Vector3 pos = PosInitiale + new Vector3(0, 1, 0) * AmplitudeMouvement / 100 * Mathf.Sin(VitesseExecution * Time.fixedTime + InitialAngle);
        //Applique les changements au rigidbody
        RigidbodyPrefab.MovePosition(pos);
        RigidbodyPrefab.MoveRotation(Rot);

    }
}
