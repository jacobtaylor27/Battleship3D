// Cellule
public class Cellule
{
	private ÉtatCellule étatActuel;

	private ÉtatCellule ÉtatFutur
	{
		get;
		set;
	}

	public ÉtatCellule ÉtatActuel
	{
		get;
		private set;
	}

	public Cellule()
	{
		ÉtatActuel = (ÉtatFutur = ÉtatCellule.MORT);
	}

	public Cellule(ÉtatCellule état)
	{
		ÉtatActuel = état;
	}

	public void DéterminerÉtatFutur(int nbVoisinsVivants)
	{
		ÉtatCellule étatFutur;
		if ((ÉtatActuel != ÉtatCellule.VIVANT || nbVoisinsVivants != 2) && nbVoisinsVivants != 3)
		{
			ÉtatCellule étatCellule2 = ÉtatFutur = ÉtatCellule.MORT;
			étatFutur = étatCellule2;
		}
		else
		{
			ÉtatCellule étatCellule2 = ÉtatFutur = ÉtatCellule.VIVANT;
			étatFutur = étatCellule2;
		}
		ÉtatFutur = étatFutur;
	}

	public bool ActualiserÉtat()
	{
		bool num = ÉtatActuel != ÉtatFutur;
		if (num)
		{
			ÉtatActuel = ÉtatFutur;
		}
		return num;
	}

	public void InverserÉtatCellule()
	{
		ÉtatCellule num;
		if (ÉtatActuel != 0)
		{
			ÉtatCellule étatCellule2 = ÉtatActuel = ÉtatCellule.MORT;
			num = étatCellule2;
		}
		else
		{
			ÉtatCellule étatCellule2 = ÉtatActuel = ÉtatCellule.VIVANT;
			num = étatCellule2;
		}
		ÉtatActuel = num;
	}

	public void Tuer()
	{
		ÉtatFutur = ÉtatCellule.MORT;
	}
}
