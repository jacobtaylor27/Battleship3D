using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementBateau : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    bool ActiverPlacement;


    int Layer = 8;
    List<Bateau> Bateaux;
    bool PeutÊtrePlacé;
    int IndiceBateauActuel;
    RaycastHit hit;
    Vector3 PtCollision;
    Ray ray;
    GameObject CubeÀPlacer;

    void Start()
    {
        ValeursInitiales();
        CubeÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, Input.mousePosition, Quaternion.identity);
        //ActiverBateau(-1);
        //ActiverBateau(IndiceBateauActuel);
        //Debug.Log(Camera.allCameras[0].name);
        //Debug.Log(Camera.allCameras[1].name);
        //Debug.Log(Camera.allCameras[2].name);
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
        if (ActiverPlacement)
        {
            Camera cameraJoueur = Camera.allCameras[1]; // vérfier indice
            ray = cameraJoueur.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.gameObject.layer == Layer)
            {
                PtCollision = hit.collider.gameObject.transform.position;
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

    void ActiverBateau(int num)
    {
        if (num != -1)
            if (Bateaux[num].PrefabCube.activeInHierarchy)
                return;

        //Desactive les bateaux
        for (int i = 0; i < Bateaux.Count; i++)
            Bateaux[i].PrefabCube.SetActive(false);

        if (num == -1)
            return;

        //Activer les bateaux voulue
        Bateaux[num].PrefabCube.SetActive(true);

    }

    void DéplacerCubes(/*Bateau b*/)
    {
        PeutÊtrePlacé = VérifierPlace();
        CubeÀPlacer.transform.position = new Vector3(Mathf.Round(PtCollision.x), 5, Mathf.Round(PtCollision.z));
    }

    bool VérifierPlace()
    {
        foreach (MeshRenderer mesh in CubeÀPlacer.GetComponentsInChildren<MeshRenderer>())
        {
            BateauCubeBehavior bateauCube = mesh.GetComponent<BateauCubeBehavior>();

            if (!bateauCube.SurTuile())
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
        CubeÀPlacer.transform.localEulerAngles += new Vector3(0, 90, 0);
    }

    void PlacerBateau(Bateau b)
    {
        Instantiate(b.PrefabBateau, CubeÀPlacer.transform.position, CubeÀPlacer.transform.rotation);
        b.EstPlacé = true;
        IndiceBateauActuel++;
        Vector3 temp = new Vector3(CubeÀPlacer.transform.position.x, CubeÀPlacer.transform.position.y, CubeÀPlacer.transform.position.z);
        Quaternion tempR = new Quaternion(CubeÀPlacer.transform.rotation.x, CubeÀPlacer.transform.rotation.y, CubeÀPlacer.transform.rotation.z, CubeÀPlacer.transform.rotation.w);
        Destroy(CubeÀPlacer);
        CubeÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, temp, tempR);

        if (SontTousPlacés())
            ExitState();
    }

    bool SontTousPlacés()
    {
        return Bateaux.TrueForAll(x => x.EstPlacé == true);
    }

    public void EnterState()
    {
        enabled = true;
    }

    public void ExitState()
    {
        enabled = false;
        GestionnaireJeu.manager.NextPlayer();
    }


}
