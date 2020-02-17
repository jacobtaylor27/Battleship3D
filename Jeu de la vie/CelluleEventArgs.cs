// CelluleEventArgs
using System;

public class CelluleEventArgs : EventArgs
{
	public int NoRangée
	{
		get;
		set;
	}

	public int NoColonne
	{
		get;
		set;
	}

	public ÉtatCellule État
	{
		get;
		set;
	}

	public CelluleEventArgs(int noRangée, int noColonne, ÉtatCellule état)
	{
		NoRangée = noRangée;
		NoColonne = noColonne;
		État = état;
	}
}
