using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Computer : Joueur
{
    // Classe random pris de : http://stackoverflow.com/a/18267477/106356
    System.Random RNG = new System.Random(Guid.NewGuid().GetHashCode());

    public void PlacerBateaux()
    {
        foreach (var b in Arsenal)
        {
            bool estDisponible = true;
            while (estDisponible)
            {
                var colonneInitiale = RNG.Next(1, 11);
                var rangéeInitiale = RNG.Next(1, 11);
                var rangéeFinale = rangéeInitiale;
                var colonneFinale = colonneInitiale;
                var orientation = RNG.Next(0, 2);
                var paneauxUtilisés = PaneauJeu.Cases.Range(rangéeInitiale, colonneInitiale, rangéeFinale, colonneFinale);

                if (orientation == 0)
                {
                    for (int i = 1; 1 < b.Longueur; i++)
                        rangéeFinale++;
                }
                else
                {
                    for (int i = 1; i < b.Longueur; i++)
                        colonneFinale++;
                }

                if (rangéeFinale > 10 || colonneFinale > 10)
                    estDisponible = true;

                if (paneauxUtilisés.Any(x => x.EstOccupé))
                    estDisponible = true;

                foreach (var p in paneauxUtilisés)
                    p.TypeOccupation = b.TypeOccupation;

                estDisponible = false;
            }
        }
    }

    private Coordonnées RechercherTir(List<Coordonnées> coupsVoisins) => coupsVoisins[RNG.Next(coupsVoisins.Count)];

    private Coordonnées TirAléatoire()
    {

    }

    public Coordonnées Tirer()
    {
        var coupsVoisins = PaneauTirs.ChercherVoisinsDeTouché();
        Coordonnées coordonnées;

        if (coupsVoisins.Any())
            coordonnées = RechercherTir(coupsVoisins);
        else
            coordonnées = TirAléatoire();

    }
}
