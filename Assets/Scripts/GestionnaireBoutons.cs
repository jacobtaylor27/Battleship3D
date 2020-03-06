using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionnaireBoutons : MonoBehaviour
{
    private Vector3 vecteurBoutonAgrandi { get; set; }

    private void Start() => vecteurBoutonAgrandi = new Vector3(1.5f, 1.5f, 1.5f);

    public void AgrandirBouton() => transform.localScale = vecteurBoutonAgrandi;

    public void RétrécirBouton() => transform.localScale = Vector3.one;

    public void Quitter() => Application.Quit();

}
