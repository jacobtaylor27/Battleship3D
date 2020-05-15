using UnityEngine;

public class InformationTuile : MonoBehaviour
{
    public Case Case { get; private set; }
    public SpriteRenderer spriteR;
    public Sprite[] sprites;

    public void SurlignierTuile(int numTuile) => spriteR.sprite = sprites[numTuile];

    public void DéfinirInformationTuile(Case newCase) => Case = newCase;

    // Affiche le réticule
    void OnMouseOver() => SurlignierTuile(1);

    // Affiche le périmètre
    void OnMouseExit() => SurlignierTuile(0); 
}
