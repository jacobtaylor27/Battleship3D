using Unity.Collections;
using UnityEngine;

public class MouvementBateau : MonoBehaviour
{
    const float AmplitudeMouvement = 20f;
    const float VitesseExecution = 0.5f;
    Rigidbody RigidbodyPrefab { get; set; }
    float AngleInitial { get; set; }
    Quaternion RotationInitiale { get; set; }
    Vector3 PositionInitiale { get; set; }

    void Start() => AssignerValeursInitiales();

    void AssignerValeursInitiales()
    {
        RigidbodyPrefab = GetComponent<Rigidbody>();
        RotationInitiale = transform.rotation;
        AngleInitial = Random.Range(-Mathf.PI, Mathf.PI);
        PositionInitiale = transform.position;
    }

    void Update()
    {
        RigidbodyPrefab.MovePosition(PositionInitiale + new Vector3(0, 1, 0) * AmplitudeMouvement / 100 * Mathf.Sin(VitesseExecution * Time.fixedTime + AngleInitial));
        RigidbodyPrefab.MoveRotation(RotationInitiale * Quaternion.Euler(0, 0, +AmplitudeMouvement * Mathf.Sin(VitesseExecution * Time.fixedTime + AngleInitial)));
    }
}
