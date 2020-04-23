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
    GameObject CubesÀPlacer;
    Case CaseVisée;

    private void Awake()
    {
        enabled = false;
    }
    void Start()
    {
        ValeursInitiales();
        CubesÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, Input.mousePosition, Quaternion.identity);
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
        DéplacerCubes();

        if (Input.GetMouseButtonDown(0) && PeutÊtrePlacé)
        {
            Debug.Log("gauche");
            PlacerBateau(Bateaux[IndiceBateauActuel]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("droite");
            ChangerDirectionCubes();
        }
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
        CubesÀPlacer.transform.position = new Vector3(Mathf.Round(PtCollision.x), 5, Mathf.Round(PtCollision.z));
    }

    bool VérifierPlace()
    {
        foreach (MeshRenderer mesh in CubesÀPlacer.GetComponentsInChildren<MeshRenderer>())
        {
            BateauCubeBehavior bateauCube = mesh.GetComponent<BateauCubeBehavior>();

            if (!bateauCube.EstSurTuile() || bateauCube.GetInfoTuile().Case.TypeOccupation == TypeOccupation.Occupé)
            {
                foreach (MeshRenderer cube in CubesÀPlacer.GetComponentsInChildren<MeshRenderer>())
                    cube.material.color = new Color(255, 0, 0, 250);

                return false;
            }
            else
                mesh.material.color = new Color(0, 0, 0);

        }

        return true;
    }

    void ChangerDirectionCubes()
    {
        CubesÀPlacer.transform.Rotate(Vector3.up,90f);
        
        Debug.Log(DéterminerOrientation(CubesÀPlacer.transform.localEulerAngles.y).ToString());
    }

    void PlacerBateau(Bateau b)
    {

        Instantiate(b.PrefabBateau, CubesÀPlacer.transform.position, CubesÀPlacer.transform.rotation);

        //Changer occupations dans paneaujeu
        //Ajouter case occupées sur le bateau
        GestionnaireJeu.manager.PlacerBateauLogique(IndiceBateauActuel, DéterminerOrientation(CubesÀPlacer.transform.localEulerAngles.y), CaseVisée);
        //b.EstPlacé = true;

        if (!SontTousPlacés())
        {
            IndiceBateauActuel++;

            //Crée les cubes du prochain bateau
            Vector3 temp = new Vector3(CubesÀPlacer.transform.position.x, CubesÀPlacer.transform.position.y, CubesÀPlacer.transform.position.z);
            Quaternion tempR = new Quaternion(CubesÀPlacer.transform.rotation.x, CubesÀPlacer.transform.rotation.y, CubesÀPlacer.transform.rotation.z, CubesÀPlacer.transform.rotation.w);
            Destroy(CubesÀPlacer);
            CubesÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, temp, tempR);

        }
        else
        {
            Destroy(CubesÀPlacer);
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
                orientation = Vector3.back;
                break;
            case 180f:
                orientation = Vector3.left;
                break;
            case 270f:
                orientation = Vector3.forward;
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
        GestionnaireJeu.manager.PasserAuProchainTour();
    }


}
