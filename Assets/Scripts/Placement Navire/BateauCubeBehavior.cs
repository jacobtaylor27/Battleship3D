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
        if (InfoTuile != null)
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
            return hit.collider.gameObject.GetComponent<InformationTuile>();
        }

        return null;
    }


}
