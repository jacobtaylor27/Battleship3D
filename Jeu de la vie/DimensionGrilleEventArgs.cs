// DimensionGrilleEventArgs
using System;

public class DimensionGrilleEventArgs : EventArgs
{
	public int NbRangéesGrille
	{
		get;
		set;
	}

	public int NbColonnesGrille
	{
		get;
		set;
	}

	public bool CopierColonie
	{
		get;
		set;
	}
}
