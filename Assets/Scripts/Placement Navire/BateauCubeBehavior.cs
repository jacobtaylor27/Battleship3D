using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BateauCubeBehavior : MonoBehaviour
{
    public LayerMask Layer;
    RaycastHit hit;
    InformationTuile InfoTuile;

    public bool EstSurTuile()
    {
        InfoTuile = GetInfoTuile();
        if (InfoTuile != null && InfoTuile.Case.TypeOccupation == TypeOccupation.Vide)
        {
            return true;
        }
        InfoTuile = null;
        return false;
    }

    public InformationTuile GetInfoTuile()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, 20f, Layer))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            return hit.collider.GetComponent<InformationTuile>();
        }

        return null;
    }


}
