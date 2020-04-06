using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBateau : MonoBehaviour
{
    [SerializeField]
    bool place; // Booléen accessible dans l'inspecteur permet de placer les bateaux si la valeur = vrai 
    [SerializeField]
    LayerMask Layer;
    [SerializeField]
    List<Bateau> ListeBateau = new List<Bateau>();
    bool peutÊtrePlacé;
    int BateauActuel = 0; //Changer pour avoir le bon bateau (de 0 à 4)
    RaycastHit hit;
    Vector3 PtCollision;
    Transform transformBateauActuel;

    void Start()
    {
        transformBateauActuel = ListeBateau[BateauActuel].BateauCube.transform;
        // JB : À mettre dans EnterState()? A voir qu'est-ce que ça fait exactement
        ActiverBateau(-1);
        ActiverBateau(BateauActuel);// ou -1 //chq bateau dans placement bateau sera desactiver
        PtCollision = new Vector3();
    }

    void Awake() => enabled = true;

    void Update()
    {
        if (place)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
                PtCollision = hit.collider.gameObject.transform.position;


            if (Input.GetMouseButtonDown(0)) // Click gauche
                if (peutÊtrePlacé)
                    PlacerBateau(ListeBateau[BateauActuel]);

            if (Input.GetMouseButtonDown(1)) // Click droit
                ChangerDirectionBateau();

            PlacerBateauCube();
        }
    }

    void ActiverBateau(int num)
    {
        if (num != -1)
            if (ListeBateau[num].BateauCube.activeInHierarchy)
                return;

        //Desactive les bateaux
        for (int i = 0; i < ListeBateau.Count; i++)
            ListeBateau[i].BateauCube.SetActive(false);

        if (num == -1)
            return;

        //Activer les bateaux voulue
        ListeBateau[num].BateauCube.SetActive(true);

    }

    void PlacerBateauCube()
    {
        if (place)
            //peutÊtrePlacé = VérifierPlace(); //check for other ships
            //placer bateau actuel de liste bateau
            transformBateauActuel.position = new Vector3(Mathf.Round(PtCollision.x), 5, Mathf.Round(PtCollision.z)); //round les valeurs pour avoir que des entiers

        else
            //Desactiver chq ghost (bateau)
            ActiverBateau(-1);

    }

    bool VérifierPlace()
    {
        foreach (Transform t in transformBateauActuel)
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

    void ChangerDirectionBateau() => transformBateauActuel.localEulerAngles += new Vector3(0, 90, 0);

    void PlacerBateau()
    {
        //instantier un nouveau bateau à partir de la liste de Bateau 

        Vector3 PositionRaycast = new Vector3(Mathf.Round(PtCollision.x), 0, Mathf.Round(PtCollision.z));
        Quaternion RotationCoup = transformBateauActuel.rotation;

        Instantiate(ListeBateau[BateauActuel].BateauPrefab, PositionRaycast, RotationCoup);

        //Update la Grille et incrmenter le nmbre de bateau actuellement placer
        //Desactiver Place
        //Desactiver les cubes(Prefab)
        //Verifier si tout les bateaux ont été placer
    }

    public void PlacerBateau(Bateau bateau)
    {
        bateau.BateauPrefab.transform.position = new Vector3(Mathf.Round(PtCollision.x), 0, Mathf.Round(PtCollision.y));
        bateau.BateauPrefab.transform.rotation = transformBateauActuel.rotation;

        Instantiate(bateau.BateauPrefab, bateau.BateauPrefab.transform.position, bateau.BateauPrefab.transform.rotation);
    }

    bool SontTousPlacés()
    {
        bool temp = false;

        foreach (Bateau b in ListeBateau)
        {
            if (b.EstPlacé)
                temp = true;
            else
            {
                return false;
            }

        }
        return temp;
    }

    public void EnterState() => enabled = true;

    public void ExitState()
    {
        enabled = false;

        // S'assurer de modifier la GrilleLogique et OccupationEventArgs

        GestionnaireJeu.manager.NextPlayer();// Ou CommencerPhaseTirs();
    }
}
