using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GénérerCollidersGrille : MonoBehaviour
{
    const float Dimensions = 10f;
    Transform[] CoinsNPC;
    Transform[] CoinsPlayer;
    public GameObject ColliderBox;
    public float Delta { get; set; }
    float Distance { get; set; }
    public Vector3 OrigineNPC { get; set; }
    float Décalage { get { return Delta / 2; } }

    void Start()
    {

        CoinsNPC = GameObject.FindGameObjectsWithTag("NPC").First(x => x.name == "Coins").GetComponentsInChildren<Transform>();
        CoinsPlayer = GameObject.FindGameObjectsWithTag("Player").First(x => x.name == "Coins").GetComponentsInChildren<Transform>();

        OrigineNPC = CoinsNPC[1].position;

        Distance = Mathf.Sqrt(Mathf.Pow(CoinsPlayer[1].position.x, 2) + Mathf.Pow(CoinsPlayer[2].position.x, 2));
        Delta = Distance / Dimensions;

        for (int i = 0; i < Dimensions; i++)
        {
            for (int j = 0; j < Dimensions; j++)
            {
                Instantiate(ColliderBox, new Vector3(CoinsNPC[1].position.x + -j * Delta - Décalage, CoinsNPC[1].position.y, CoinsNPC[1].position.z + Décalage + i * Delta), Quaternion.identity).GetComponent<InformationTuile>().DéfinirInformationTuile(GestionnaireJeu.manager.JoueurActif.PaneauJeu.TrouverCase(new Coordonnées(i, j)));
                Instantiate(ColliderBox, new Vector3(CoinsPlayer[1].position.x + j * Delta + Décalage, CoinsPlayer[1].position.y, CoinsPlayer[1].position.z + -i * Delta - Décalage), Quaternion.identity).GetComponent<InformationTuile>().DéfinirInformationTuile(GestionnaireJeu.manager.AutreJoueur.PaneauJeu.TrouverCase(new Coordonnées(i, j)));
            }
        }

    }
}
