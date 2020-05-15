using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementMissile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        StartCoroutine(Routine());
    }
    IEnumerator Routine()
    {
        yield return new WaitForSecondsRealtime(1);//je veux faire attendre 1 seconde pour voir le changement de couleur, ça marche pas
    }
}
