using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementBateau : MonoBehaviour, IPointerClickHandler
{
    int Layer = 9;
    List<Bateau> Bateaux;
    bool PeutÊtrePlacé;
    int IndiceBateauActuel;
    RaycastHit hit;
    Vector3 PtCollision;
    Ray ray;
    GameObject CubeÀPlacer;
    Case CaseVisée;

    private void Awake()
    {
        enabled = false;
    }
    void Start()
    {
        ValeursInitiales();
        CubeÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, Input.mousePosition, Quaternion.identity);
    }

    void ValeursInitiales()
    {
        Bateaux = GestionnaireJeu.manager.JoueurActif.Arsenal;
        IndiceBateauActuel = 0;
        PeutÊtrePlacé = true;
        PtCollision = new Vector3();
    }

    void Update()
    {
        Camera cameraJoueur = Camera.allCameras[1]; // vérfier indice
        ray = cameraJoueur.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity,LayerMask.GetMask(new string[] { "Tuile" })) && hit.collider.gameObject.layer == Layer)
        {
            PtCollision = hit.collider.gameObject.transform.position;
            CaseVisée = hit.collider.gameObject.GetComponent<InformationTuile>().Case;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("gauche");
            PlacerBateau(Bateaux[IndiceBateauActuel]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("droite");
            ChangerDirectionCubes();
        }
        DéplacerCubes();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("pointer");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("right");
            ChangerDirectionCubes();
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("gauche");
            if (PeutÊtrePlacé)
            {
                PlacerBateau(Bateaux[IndiceBateauActuel]);
            }
        }
    }


    void DéplacerCubes()
    {
        PeutÊtrePlacé = VérifierPlace();
        CubeÀPlacer.transform.position = new Vector3(Mathf.Round(PtCollision.x), 5, Mathf.Round(PtCollision.z));
    }

    bool VérifierPlace()
    {
        foreach (MeshRenderer mesh in CubeÀPlacer.GetComponentsInChildren<MeshRenderer>())
        {
            BateauCubeBehavior bateauCube = mesh.GetComponent<BateauCubeBehavior>();

            if (!bateauCube.EstSurTuile() || bateauCube.GetInfoTuile().Case.TypeOccupation == TypeOccupation.Occupé)
            {
                mesh.material.color = new Color(255, 0, 0, 250);
                return false;
            }

            mesh.material.color = new Color(0, 0, 0);
        }

        return true;
    }

    void ChangerDirectionCubes()
    {
        if (CubeÀPlacer.transform.localEulerAngles == new Vector3(0, 360, 0))
            CubeÀPlacer.transform.localEulerAngles = Vector3.zero;

        CubeÀPlacer.transform.localEulerAngles += new Vector3(0, 90, 0);
    }

    void PlacerBateau(Bateau b)
    {

        Instantiate(b.PrefabBateau, CubeÀPlacer.transform.position, CubeÀPlacer.transform.rotation);

        //Changer occupations dans paneaujeu
        //Ajouter case occupées sur le bateau
        GestionnaireJeu.manager.UpdateOccupation(IndiceBateauActuel, DéterminerOrientation(CubeÀPlacer.transform.localEulerAngles.y), CaseVisée);
        b.EstPlacé = true;

        if (!SontTousPlacés())
        {
            IndiceBateauActuel++;

            //Crée les cubes du prochain bateau
            Vector3 temp = new Vector3(CubeÀPlacer.transform.position.x, CubeÀPlacer.transform.position.y, CubeÀPlacer.transform.position.z);
            Quaternion tempR = new Quaternion(CubeÀPlacer.transform.rotation.x, CubeÀPlacer.transform.rotation.y, CubeÀPlacer.transform.rotation.z, CubeÀPlacer.transform.rotation.w);
            Destroy(CubeÀPlacer);
            CubeÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, temp, tempR);

        }
        else
        {
            Destroy(CubeÀPlacer);
            ExitState();
        }


    }

    private Vector3 DéterminerOrientation(float eulerAngleY)
    {
        Vector3 orientation = Vector3.zero;

        switch (eulerAngleY)
        {
            case 0f:
                orientation = Vector3.right;
                break;
            case 90f:
                orientation = Vector3.forward;
                break;
            case 180f:
                orientation = Vector3.left;
                break;
            case 270f:
                orientation = Vector3.back;
                break;
        }

        return orientation;
    }

    bool SontTousPlacés()
    {
        return Bateaux.TrueForAll(x => x.EstPlacé);
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
