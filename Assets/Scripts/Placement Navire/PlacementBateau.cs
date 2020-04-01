using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBateau : MonoBehaviour
{
    public bool place; //Place mode on / off 
    bool PeutÊtrePlacé { get; set; }
    public LayerMask Layer;

    public List<Bateau> ListeBateau = new List<Bateau>();

    int BateauActuel = 0; //Changer pour avoir le bon bateau (de 0 à 4)
    RaycastHit hit;
    Vector3 PtCollision;

    void Start()
    {
        // JB : À mettre dans EnterState()? A voir qu'est-ce que ça fait exactement
        ActiverBateau(-1);
        ActiverBateau(BateauActuel);// ou -1 //chq bateau dans placement bateau sera desactiver
        PtCollision = new Vector3();
    }

    private void Awake()
    {
        enabled = true;
    }

    void Update()
    {
        if (place)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,Layer))
            {
                PtCollision = hit.collider.gameObject.transform.position;

                //if (Layer != 8)
                //{
                //    foreach(Transform t in ListeBateau[BateauActuel].BateauCube.transform)
                //    {
                //        BateauCubeBehavior bateauCube = t.GetComponent<BateauCubeBehavior>();
                //        ListeBateau[BateauActuel].BateauCube.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0);

                //    }
                //}

            }

            if (Input.GetMouseButtonDown(0)) // Click gauche
            {
                if (PeutÊtrePlacé)
                    PlacerBateau();
            }

            if (Input.GetMouseButtonDown(1)) // Click droit
                ChangerDirectionBateau();

            PlacerBateauCube();
        }
    }

    void ActiverBateau(int num)
    {
        if (num != -1)
        {
            if (ListeBateau[num].BateauCube.activeInHierarchy)
            {
                return;
            }
        }
        //Desactive les bateaux
        for (int i = 0; i < ListeBateau.Count; i++)
        {
            ListeBateau[i].BateauCube.SetActive(false);
        }
        if (num == -1)
        {
            return;
        }
        //Activer les bateaux voulue
        ListeBateau[num].BateauCube.SetActive(true);

    }

    void PlacerBateauCube()
    {
        if (place)
        {
            //Debug.Log(PtCollision.x);
            PeutÊtrePlacé = VérifierPlace(); //check for other ships
            //placer bateau actuel de liste bateau
            ListeBateau[BateauActuel].BateauCube.transform.position = new Vector3(Mathf.Round(PtCollision.x), 5, Mathf.Round(PtCollision.z)); //round les valeurs pour avoir que des entiers
        }
        else
        {
            //Desactiver chq ghost (bateau)
            ActiverBateau(-1);
        }

    }

    bool VérifierPlace()
    {
        foreach (Transform t in ListeBateau[BateauActuel].BateauCube.transform)
        {
            BateauCubeBehavior bateauCube = t.GetComponent<BateauCubeBehavior>();

            if (!bateauCube.SurTuile())
            {
                t.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 250);
                return false;
            }

            t.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
        }

        return true;
    }
    void ChangerDirectionBateau()
    {
        ListeBateau[BateauActuel].BateauCube.transform.localEulerAngles += new Vector3(0, 90, 0); // fait tourner BateauActuel de 90 Deg selon l'axe des Y
    }

    void PlacerBateau()
    {
        //instantier un nouveau bateau à partir de la liste de Bateau 


        Vector3 PositionCoup = new Vector3(Mathf.Round(PtCollision.x), 0, Mathf.Round(PtCollision.z));//hit point
        Quaternion RotationCoup = ListeBateau[BateauActuel].BateauCube.transform.rotation;

        GameObject NouveauBateau = Instantiate(ListeBateau[BateauActuel].BateauPrefab, PositionCoup, RotationCoup);

        //Update la Grille et incrmenter le nmbre de bateau actuellement placer
        //Desactiver Place
        //Desactiver les cubes(Prefab)
        //Verifier si tout les bateaux ont été placer
    }

    public void EnterState()
    {
        enabled = true;
    }

    public void ExitState()
    {
        enabled = false;

        // S'assurer de modifier la GrilleLogique et OccupationEventArgs

        GestionnaireJeu.manager.NextPlayer();// Ou CommencerPhaseTirs();
    }
}
