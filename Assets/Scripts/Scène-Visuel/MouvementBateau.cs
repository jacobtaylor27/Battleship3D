using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementBateau : MonoBehaviour
{
    Rigidbody RigidbodyPrefab;
    float AngleInitial;
    Quaternion RotationInitiale;
    Vector3 PosInitiale;
    [SerializeField]
    float AmplitudeMouvement = 20f;
    [SerializeField]
    float VitesseExecution = 0.5f;

    void Start()
    {
        RigidbodyPrefab = GetComponent<Rigidbody>();
        RotationInitiale = transform.rotation;
        AngleInitial = Random.Range(-Mathf.PI, Mathf.PI);
        PosInitiale = transform.position;
    }

    void Update()
    {
        RigidbodyPrefab.MovePosition(PosInitiale + new Vector3(0, 1, 0) * AmplitudeMouvement / 100 * Mathf.Sin(VitesseExecution * Time.fixedTime + AngleInitial));
        RigidbodyPrefab.MoveRotation(RotationInitiale * Quaternion.Euler(0, 0, +AmplitudeMouvement * Mathf.Sin(VitesseExecution * Time.fixedTime + AngleInitial)));
    }
}
