using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GénérerCollidersGrille : MonoBehaviour
{
    // Start is called before the first frame update
    const float Dimensions = 10f;
    Transform[] Coins;
    public GameObject ColliderBox;
    float Delta { get; set; }
    float Distance { get; set; }
    float Décalage
    {
        get { return Delta / 2; }
    }

    void Start()
    {
        Coins = GetComponentsInChildren<Transform>();// Éléments 1 à 4 sont les coins

        Distance = Mathf.Sqrt(Mathf.Pow(Coins[1].position.x, 2) + Mathf.Pow(Coins[2].position.x, 2));
        Delta = Distance / Dimensions;

        for(int i = 0; i < Dimensions; i++)
        {
            for(int j = 0; j < Dimensions; j++)
            {
                if (tag == "NPC")//Vérifier les tag dans la scène du projet final
                {
                    Instantiate(ColliderBox, new Vector3(Coins[1].position.x + -j * Delta - Décalage, Coins[1].position.y, Coins[1].position.z + Décalage + i * Delta), Quaternion.identity);
                }
                else
                {
                    Instantiate(ColliderBox, new Vector3(Coins[1].position.x + j * Delta + Décalage, Coins[1].position.y, Coins[1].position.z + -i * Delta - Décalage), Quaternion.identity);
                }
            }
        } 

    }
}
