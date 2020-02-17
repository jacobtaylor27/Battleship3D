// Colonie
using System;
using System.Text;

public sealed class Colonie
{
	public const int NbRangéesMin = 5;

	public const int NbRangéesMax = 20;

	public const int NbColonnesMin = 5;

	public const int NbColonnesMax = 20;

	private int nbColonnesGrille;

	private int nbRangéesGrille;

	private Cellule[,] ColonieCellulaire
	{
		get;
		set;
	}

	public int NbColonnesGrille
	{
		get
		{
			return nbColonnesGrille;
		}
		private set
		{
			if (value >= 5 && value <= 20)
			{
				nbColonnesGrille = value;
			}
		}
	}

	public int NbRangéesGrille
	{
		get
		{
			return nbRangéesGrille;
		}
		private set
		{
			if (value >= 5 && value <= 20)
			{
				nbRangéesGrille = value;
			}
		}
	}

	private DimensionGrilleEventArgs DonnéesÉvènement
	{
		get;
		set;
	}

	public event EventHandler<CelluleEventArgs> CelluleModifiée;

	public event EventHandler<DimensionGrilleEventArgs> DimensionsGrilleModifiée;

	private void OnCelluleModifiée(CelluleEventArgs data)
	{
		this.CelluleModifiée?.Invoke(this, data);
	}

	private void OnDimensionsGrilleModifiée(DimensionGrilleEventArgs data)
	{
		this.DimensionsGrilleModifiée?.Invoke(this, data);
	}

	public Colonie()
	{
		DonnéesÉvènement = new DimensionGrilleEventArgs();
		this.DimensionsGrilleModifiée = null;
		this.CelluleModifiée = null;
		NbColonnesGrille = 5;
		NbRangéesGrille = 5;
		ColonieCellulaire = new Cellule[NbRangéesGrille, NbColonnesGrille];
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				ColonieCellulaire[i, j] = new Cellule(ÉtatCellule.MORT);
			}
		}
	}

	public Colonie(Colonie colonie, DimensionGrilleEventArgs dataEvent)
	{
		DonnéesÉvènement = new DimensionGrilleEventArgs();
		CréerNouvelleColonie(dataEvent);
		this.DimensionsGrilleModifiée = colonie.DimensionsGrilleModifiée;
		this.CelluleModifiée = colonie.CelluleModifiée;
		colonie.NbRangéesGrille = colonie.ColonieCellulaire.GetLength(0);
		colonie.NbColonnesGrille = colonie.ColonieCellulaire.GetLength(1);
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				ColonieCellulaire[i, j] = new Cellule((i < colonie.NbRangéesGrille && j < colonie.NbColonnesGrille) ? colonie.ColonieCellulaire[i, j].ÉtatActuel : ÉtatCellule.MORT);
				OnCelluleModifiée(new CelluleEventArgs(i, j, ColonieCellulaire[i, j].ÉtatActuel));
			}
		}
	}

	private void CréerNouvelleColonie(DimensionGrilleEventArgs dataEvent)
	{
		NbColonnesGrille = dataEvent.NbColonnesGrille;
		NbRangéesGrille = dataEvent.NbRangéesGrille;
		ColonieCellulaire = new Cellule[dataEvent.NbRangéesGrille, dataEvent.NbColonnesGrille];
	}

	public void InverserÉtatCellule(int noRangée, int noColonne)
	{
		ColonieCellulaire[noRangée, noColonne].InverserÉtatCellule();
		OnCelluleModifiée(new CelluleEventArgs(noRangée, noColonne, ColonieCellulaire[noRangée, noColonne].ÉtatActuel));
	}

	private int CalculerNbVoisinsVivants(int posRangée, int posColonne)
	{
		int num = 0;
		for (int i = posRangée - 1; i <= posRangée + 1; i++)
		{
			for (int j = posColonne - 1; j <= posColonne + 1; j++)
			{
				if (i > -1 && j > -1 && i < NbRangéesGrille && j < NbColonnesGrille)
				{
					num = (int)(num + ColonieCellulaire[i, j].ÉtatActuel);
				}
			}
		}
		return (int)(num - ColonieCellulaire[posRangée, posColonne].ÉtatActuel);
	}

	public void EffectuerMiseÀJour()
	{
		CalculerGeneration();
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				Cellule cellule = ColonieCellulaire[i, j];
				if (cellule.ActualiserÉtat())
				{
					OnCelluleModifiée(new CelluleEventArgs(i, j, cellule.ÉtatActuel));
				}
			}
		}
	}

	private void CalculerGeneration()
	{
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				ColonieCellulaire[i, j].DéterminerÉtatFutur(CalculerNbVoisinsVivants(i, j));
			}
		}
	}

	public void Vider()
	{
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				Cellule cellule = ColonieCellulaire[i, j];
				cellule.Tuer();
				if (cellule.ActualiserÉtat())
				{
					OnCelluleModifiée(new CelluleEventArgs(i, j, cellule.ÉtatActuel));
				}
			}
		}
	}

	public string SerializeToText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(NbRangéesGrille + ";" + NbColonnesGrille + "\n");
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				stringBuilder.Append((int)ColonieCellulaire[i, j].ÉtatActuel);
			}
		}
		return stringBuilder.ToString();
	}

	private Colonie(Colonie colonie, int nbRangées, int nbColonnes, string txtÉtats)
	{
		DonnéesÉvènement = new DimensionGrilleEventArgs();
		this.DimensionsGrilleModifiée = colonie.DimensionsGrilleModifiée;
		this.CelluleModifiée = colonie.CelluleModifiée;
		ModifierDimensionsGrille(nbRangées, nbColonnes, copierColonie: false);
		ColonieCellulaire = new Cellule[NbRangéesGrille, NbColonnesGrille];
		int num = 0;
		for (int i = 0; i < NbRangéesGrille; i++)
		{
			for (int j = 0; j < NbColonnesGrille; j++)
			{
				ColonieCellulaire[i, j] = new Cellule((txtÉtats[num++] != '0') ? ÉtatCellule.VIVANT : ÉtatCellule.MORT);
				OnCelluleModifiée(new CelluleEventArgs(i, j, ColonieCellulaire[i, j].ÉtatActuel));
			}
		}
	}

	public Colonie DeserializeFromText(string txtDimension, string txtÉtats)
	{
		char[] separator = new char[1]
		{
			';'
		};
		string[] array = txtDimension.Split(separator);
		int nbRangées = int.Parse(array[0]);
		int nbColonnes = int.Parse(array[1]);
		return new Colonie(this, nbRangées, nbColonnes, txtÉtats);
	}

	public void ModifierDimensionsGrille(int nbRangées, int nbColonnes, bool copierColonie)
	{
		if (nbRangées != nbRangéesGrille || nbColonnes != nbColonnesGrille)
		{
			int num = NbRangéesGrille;
			int num2 = NbColonnesGrille;
			NbRangéesGrille = nbRangées;
			NbColonnesGrille = nbColonnes;
			if (num != NbRangéesGrille || num2 != NbColonnesGrille)
			{
				DimensionGrilleEventArgs dimensionGrilleEventArgs = new DimensionGrilleEventArgs();
				dimensionGrilleEventArgs.NbRangéesGrille = NbRangéesGrille;
				dimensionGrilleEventArgs.NbColonnesGrille = NbColonnesGrille;
				dimensionGrilleEventArgs.CopierColonie = copierColonie;
				OnDimensionsGrilleModifiée(dimensionGrilleEventArgs);
			}
		}
	}
}
