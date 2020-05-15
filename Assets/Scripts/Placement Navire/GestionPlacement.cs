using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestionPlacement : MonoBehaviour
{
    int Layer { get; set; }
    List<Bateau> Bateaux { get; set; }
    bool PeutÊtrePlacé { get; set; }
    int IndiceBateauActuel { get; set; }
    Vector3 PtCollision { get; set; }
    GameObject CubesÀPlacer { get; set; }
    Case CaseVisée { get; set; }
    Camera CaméraJoueur { get; set; }
    RaycastHit hit;

    void Awake() => enabled = false;

    void Start() => AssignerValeursInitiales();

    void AssignerValeursInitiales()
    {
        Layer = 9;
        CaméraJoueur = Camera.allCameras.ToList().Find(x => x.name == "PlayerGridCam");
        Bateaux = GestionnaireJeu.manager.JoueurActif.Arsenal;
        IndiceBateauActuel = 0;
        PeutÊtrePlacé = true;
        PtCollision = GestionnaireJeu.manager.JoueurActif.PaneauJeu.Cases[0].PositionMonde;
        CaseVisée = GestionnaireJeu.manager.JoueurActif.PaneauJeu.Cases[0];
        CubesÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, PtCollision, Quaternion.identity);
    }

    void Update()
    {
        // Modifier les cases visées par le joueurs en fonctions d'un raycast
        if (Physics.Raycast(CaméraJoueur.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask(new string[] { "Tuile" })) && hit.collider.gameObject.layer == Layer)
        {
            PtCollision = hit.collider.gameObject.transform.position;
            CaseVisée = hit.collider.gameObject.GetComponent<InformationTuile>().Case;
        }
        DéplacerCubes();

        // Placer le bateau avec un clique gauche de la souris
        if (Input.GetMouseButtonDown(0) && PeutÊtrePlacé)
            PlacerBateau(Bateaux[IndiceBateauActuel]);

        // Rotation de 90 degrées du bateau avec un clique droit de la souris
        if (Input.GetMouseButtonDown(1))
            ChangerDirectionCubes();
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
            CubeBehavior bateauCube = mesh.GetComponent<CubeBehavior>();

            if (!bateauCube.EstSurTuile() || bateauCube.ChercherInformationsTuile().Case.TypeOccupation == TypeOccupation.Occupé)
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

    void ChangerDirectionCubes() => CubesÀPlacer.transform.Rotate(Vector3.up, 90f);

    void PlacerBateau(Bateau b)
    {
        // Instancier le bateau à la position et rotation des cubes
        Instantiate(b.PrefabBateau, CubesÀPlacer.transform.position, CubesÀPlacer.transform.rotation);

        // Placer le bateau sur le panneau de jeu du joueur (grille logique)
        GestionnaireJeu.manager.PlacerBateauLogique(IndiceBateauActuel, DéterminerOrientation(CubesÀPlacer.transform.localEulerAngles.y), CaseVisée);

        if (!SontTousPlacés())
        {
            IndiceBateauActuel++;

            // Définir une position et une rotation temporaire pour le prochain bateau
            Vector3 positionTemporaire = new Vector3(CubesÀPlacer.transform.position.x, CubesÀPlacer.transform.position.y, CubesÀPlacer.transform.position.z);
            Quaternion rotationTemporaire = new Quaternion(CubesÀPlacer.transform.rotation.x, CubesÀPlacer.transform.rotation.y, CubesÀPlacer.transform.rotation.z, CubesÀPlacer.transform.rotation.w);

            // Détruire cubes du bateau actuel
            Destroy(CubesÀPlacer);

            // Instancier les cube du prochain bateau dans la liste
            CubesÀPlacer = Instantiate(Bateaux[IndiceBateauActuel].PrefabCube, positionTemporaire, rotationTemporaire);
        }
        else
        {
            Destroy(CubesÀPlacer);
            ExitState();
        }
    }

    Vector3 DéterminerOrientation(float eulerAngleY)
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

    bool SontTousPlacés() => Bateaux.TrueForAll(x => x.EstPlacé);

    public void EnterState() => enabled = true;

    void ExitState()
    {
        enabled = false;
        GestionnaireJeu.manager.PasserAuProchainTour();
    }
}
