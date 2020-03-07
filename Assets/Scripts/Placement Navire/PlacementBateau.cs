using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.SCRIPTS;

public class PlacementBateau : MonoBehaviour
{
    public bool place; //Place mode on / off 
    bool PeutPlacé;

    //Grille GrilleDeJeu;
    //https://docs.unity3d.com/ScriptReference/LayerMask.html
    public LayerMask Layer;   //layer à regardé

    public List<BateauÀPlacer> ListeBateau = new List<BateauÀPlacer>();
    int BateauActuel = 4;//changer pour avoir le bon bateau (de 0 à 4)

    RaycastHit hit;
    Vector3 PtCollision;

    void Start()
    {
        // JB : À mettre dans EnterState()? A voir qu'est-ce que ça fait exactement
        ActiverBateau(-1);
        ActiverBateau(BateauActuel);// ou -1 //chq bateau dans placement bateau sera desactiver 

    }

    // Update is called once per frame
    void Update()
    {
        //Qd placing == true -> shoot a ray pour avoir la position
        if (place)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // "shoot" le ray a un endroit à parir du screenpoint 
            //si il y a une tuile(Cast un ray physique)
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
            {
                //if(tuile == !Tuile Adversaire)
                PtCollision = hit.point;  //"sauvegarde" le hit comme un vecteur

            }
            //place le bateau
            if (Input.GetMouseButton(0))    //Left Click
            {
                if (PeutPlacé)
                {
                    //placer bateau
                    PlacerBateau();
                }
            }
            //Rotation du bateau
            if (Input.GetMouseButtonDown(1))
            {
                //rotate le bateau
                ChangerDirectionBateau();
            }
            //place ghost
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
            PeutPlacé = VérifierPlace(); //check for other ships
            //placer bateau actuel de liste bateau
            ListeBateau[BateauActuel].BateauCube.transform.position = new Vector3(Mathf.Round(PtCollision.x), 0, Mathf.Round(PtCollision.z)); //round les valeurs pour avoir que des entiers
        }
        else
        {
            //Desactiver chq ghost (bateau)
            ActiverBateau(-1);
        }

    }
    
    private bool VérifierPlace()
    {
        foreach (Transform child in ListeBateau[BateauActuel].BateauCube.transform) // vérifie le transform pour chq bateau de la liste  
        {

            BateauCubeBehavior bateauCube = child.GetComponent<BateauCubeBehavior>();
            if (!bateauCube.SurTuile()) //si le bateau n'est pas sur une tuile
            {
                //changer couleur prefab pour signifier que le bateau ne peut pas être placer en rouge
                //https://forum.unity.com/threads/how-to-set-base-tint-color-of-material.287499/
                //rgb et alpha
                child.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 250);
                return false;
            }
            //sinon garder couler de base (blanche)
            child.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 250);


        }
        return true;
    }
    void ChangerDirectionBateau()
    {
        //https://docs.unity3d.com/ScriptReference/Transform-localEulerAngles.html        
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

    }
}
