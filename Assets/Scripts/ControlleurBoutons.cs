using UnityEngine;
using UnityEngine.UI;

public class ControlleurBoutons : MonoBehaviour
{
    Vector3 VecteurAgrandi { get; set; }
    Color CouleurDefault { get; set; }
    Vector3 ScaleInitial { get; set; }

    void Start() => AssignerValeursInitiales();

    void AssignerValeursInitiales()
    {
        VecteurAgrandi = new Vector3(3.5f, 3.5f, 3.5f);
        ScaleInitial = transform.localScale;
        CouleurDefault = new Color(255, 255, 255);
    }

    public void AgrandirBouton() => transform.localScale = VecteurAgrandi;

    public void RétrécirBouton() => transform.localScale = ScaleInitial;

    public void ChangerCouleurVert() => GetComponent<Image>().color = Color.green;

    public void ChangerCouleurDefault() => GetComponent<Image>().color = CouleurDefault;
}
