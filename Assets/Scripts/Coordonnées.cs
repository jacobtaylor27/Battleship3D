using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordonnées
{
    //Vérifier les niveaux de protection
    public int X
    {
        get;
        set;
    }

    public int Y
    {
        get;
        set;
    }

    public Coordonnées()
    {
        X = 0;
        Y = 0;
    }
    
    public Coordonnées(int posX, int posY)
    {
        X = posX;
        Y = posY;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            Coordonnées coord = (Coordonnées)obj;
            return coord.X == X && coord.Y == Y;
        }
    }
    public override int GetHashCode()
    {
        return X ^ Y;
    }

}
