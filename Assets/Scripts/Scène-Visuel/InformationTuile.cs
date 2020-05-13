using UnityEngine;

public class InformationTuile : MonoBehaviour
{
    public Case Case { get; private set; }
    public SpriteRenderer spriteR;
    public Sprite[] sprites;

    public void SurlignierTuile(int numTuile) => spriteR.sprite = sprites[numTuile];

    public void DéfinirInformationTuile(Case newCase) => Case = newCase;

    void OnMouseOver() => SurlignierTuile(1); // Affiche le réticule

    void OnMouseExit() => SurlignierTuile(0); // Affiche le périmètre

    void OnMouseDown() => Debug.Log(Case.ToString()); // Pour tests seulement


}
