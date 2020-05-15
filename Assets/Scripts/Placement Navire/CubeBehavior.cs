using UnityEngine;

public class CubeBehavior : MonoBehaviour
{
    LayerMask Layer;
    RaycastHit hit;

    void Start() => Layer = LayerMask.GetMask("Tuile");

    public bool EstSurTuile() => (ChercherInformationsTuile() != null) ? true : false;

    public InformationTuile ChercherInformationsTuile()
    {
        if (Physics.Raycast(new Ray(transform.position, -transform.up), out hit, 20f, Layer))
            return hit.collider.GetComponent<InformationTuile>();
        else
            return null;
    }
}
