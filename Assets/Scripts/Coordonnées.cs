public class Coordonnées
{
    public int Rangée { get; set; }
    public int Colonne { get; set; }

    public Coordonnées()
    {
        Rangée = 0;
        Colonne = 0;
    }

    public Coordonnées(int rangée, int colonne)
    {
        Rangée = rangée;
        Colonne = colonne;
    }

    public override string ToString() => $"{Rangée},{Colonne}";

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
        {
            Coordonnées coord = (Coordonnées)obj;
            return Rangée == coord.Rangée && Colonne == coord.Colonne;

        }
    }
    public override int GetHashCode()
    {
        return Rangée.GetHashCode() ^ Colonne.GetHashCode();
    }
    public static bool operator ==(Coordonnées coord1, Coordonnées coord2)
    {
        return coord1.Equals(coord2);
    }
    public static bool operator !=(Coordonnées coord1, Coordonnées coord2)
    {
        return !coord1.Equals(coord2);
    }
}
