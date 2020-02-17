// GenerateurGrilleDeJeu
using DLL_Maillage;
using UnityEngine;

public class GenerateurGrilleDeJeu : MonoBehaviour
{
	public const int DimensionHorizontale = 10;

	public const int DimensionVerticale = 10;

	public Vector2 ÉtendueSurface
	{
		get;
	} = new Vector2(10f, 10f);


	public Vector2 DeltaÉtendue
	{
		get;
		private set;
	}

	public Vector3 Origine
	{
		get;
		private set;
	}

	private Mesh MaillageGrille
	{
		get;
		set;
	}

	private int NbColonnes
	{
		get;
		set;
	}

	private int NbRangées
	{
		get;
		set;
	}

	public void CréerGrilleVisuelle(object sender, DimensionGrilleEventArgs dataEvent)
	{
		CalculerDonnéesInitiales(dataEvent);
		GénérerMaillage();
		GénérerEnveloppeCollision();
	}

	protected void CalculerDonnéesInitiales(DimensionGrilleEventArgs dataEvent)
	{
		NbColonnes = dataEvent.NbColonnesGrille;
		NbRangées = dataEvent.NbRangéesGrille;
		DeltaÉtendue = new Vector2(ÉtendueSurface.x / (float)NbColonnes, ÉtendueSurface.y / (float)NbRangées);
		Origine = new Vector3((0f - ÉtendueSurface.x) / 2f, (0f - ÉtendueSurface.y) / 2f, 0f);
	}

	private void GénérerMaillage()
	{
		MaillageGrille = Maillage.GénérerMaillage(ÉtendueSurface, Origine, NbRangées, NbColonnes);
		GetComponent<MeshFilter>().mesh = MaillageGrille;
	}

	private void GénérerEnveloppeCollision()
	{
		GetComponent<MeshCollider>().sharedMesh = MaillageGrille;
	}
}
