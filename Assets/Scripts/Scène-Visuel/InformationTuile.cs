using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationTuile : MonoBehaviour
{
    //Position de la tuile (en X et Z car Y = 0)
    public int PositionX, PositionZ;
    bool Touché;

    public SpriteRenderer spriteTuile;
    //tableau qui contient chq sprite qui seront utiliser (Ex : sprite touché, coulé, raté, etc. )
    public Sprite[] TabTuiles;
    //0 = périmètre de la tuile (sprite qui fait le tour de la tuile);
    //1 = réticule (crosshair)
    //2 = raté (eau)
    //3 = touché ()
    public void SurlignierTuile(int numTuile)
    {
        spriteTuile.sprite = TabTuiles[numTuile];
    }
    //fct qui recois les pos de chq tuile 
    public void DéfinirInformationTuile(int X, int Z)
    {
        PositionX = X;
        PositionZ = Z;
        

    } 

    void OnMouseOver()
    {
        SurlignierTuile(1);      //(monre le réticule)
    }
    void OnMouseExit()
    {
        SurlignierTuile(0);      //(montre le périmètre)
    }
}
