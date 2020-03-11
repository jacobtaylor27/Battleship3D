using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// Doit être fusionner avec le script CréerGrille.cs
public class GénérerCollidersGrille : MonoBehaviour
{
    // Start is called before the first frame update
    const float Dimensions = 10f;
    Transform[] CoinsNPC;
    Transform[] CoinsPlayer;
    public GameObject ColliderBox;
    public float Delta { get; set; }//public pour utiliser dans gestion tir
    float Distance { get; set; }
    public Vector3 OrigineNPC { get; set; }//xav

    float Décalage
    {
        get { return Delta / 2; }
    }

    void Start()
    {

        //Coins = GetComponentsInChildren<Transform>();// Éléments 1 à 4 sont les coins
        CoinsNPC = GameObject.FindGameObjectsWithTag("NPC").First(x => x.name == "Coins").GetComponentsInChildren<Transform>();
        CoinsPlayer = GameObject.FindGameObjectsWithTag("Player").First(x => x.name == "Coins").GetComponentsInChildren<Transform>();

        OrigineNPC = CoinsNPC[1].position; //je mets ça mais je sais pas vraiment c'est quel coins celui en bas a droite

        Distance = Mathf.Sqrt(Mathf.Pow(CoinsPlayer[1].position.x, 2) + Mathf.Pow(CoinsPlayer[2].position.x, 2));
        Delta = Distance / Dimensions;

        for (int i = 0; i < Dimensions; i++)
        {
            for (int j = 0; j < Dimensions; j++)
            {
                Instantiate(ColliderBox, new Vector3(CoinsNPC[1].position.x + -j * Delta - Décalage, CoinsNPC[1].position.y, CoinsNPC[1].position.z + Décalage + i * Delta), Quaternion.identity).GetComponent<InformationTuile>().DéfinirInformationTuile(i,j);
                Instantiate(ColliderBox, new Vector3(CoinsPlayer[1].position.x + j * Delta + Décalage, CoinsPlayer[1].position.y, CoinsPlayer[1].position.z + -i * Delta - Décalage), Quaternion.identity).GetComponent<InformationTuile>().DéfinirInformationTuile(i, j);
            }
        }

    }
}
