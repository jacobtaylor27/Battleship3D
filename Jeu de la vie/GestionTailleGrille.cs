// GestionTailleGrille
using System;
using UnityEngine;
using UnityEngine.UI;

public class GestionTailleGrille : MonoBehaviour
{
	[SerializeField]
	private GameObject GameManager;

	[SerializeField]
	private GameObject ObjetÀContrôler;

	[SerializeField]
	private Text txtNbColonnes;

	[SerializeField]
	private Text txtNbRangées;

	private RectTransform PanneauÀContrôler;

	private RectTransform EspaceMaximal
	{
		get;
		set;
	}

	private Vector2 Dimension
	{
		get;
		set;
	}

	private Vector2 DeltaDimension
	{
		get;
		set;
	}

	private Vector2 BornesHorizontales
	{
		get;
		set;
	}

	private Vector2 BornesVerticales
	{
		get;
		set;
	}

	private Colonie ColonieJeu
	{
		get;
		set;
	}

	public void Awake()
	{
		PanneauÀContrôler = (RectTransform)ObjetÀContrôler.transform;
		EspaceMaximal = (RectTransform)base.transform.parent;
		Dimension = new Vector2(EspaceMaximal.rect.width, EspaceMaximal.rect.height);
		DeltaDimension = new Vector2(Dimension.x / 20f, Dimension.y / 20f);
		BornesHorizontales = new Vector2(EspaceMaximal.position.x + DeltaDimension.x * 5f, EspaceMaximal.position.x + EspaceMaximal.rect.width);
		BornesVerticales = new Vector2(EspaceMaximal.position.y - EspaceMaximal.rect.height, EspaceMaximal.position.y - DeltaDimension.x * 5f);
	}

	public void Start()
	{
		ColonieJeu = GameManager.GetComponent<Jeu>().ColonieJeu;
		ColonieJeu.DimensionsGrilleModifiée += ValiderContrôleVisuel;
		ValiderTailleGrille();
	}

	private void ValiderTailleGrille()
	{
		int result;
		int nbRangées = int.TryParse(txtNbRangées.text, out result) ? Math.Min(Math.Max(5, result), 20) : 5;
		int result2;
		int nbColonnes = int.TryParse(txtNbColonnes.text, out result2) ? Math.Min(Math.Max(5, result2), 20) : 5;
		ColonieJeu.ModifierDimensionsGrille(nbRangées, nbColonnes, copierColonie: true);
	}

	private void ValiderContrôleVisuel(object sender, DimensionGrilleEventArgs dataEvent)
	{
		txtNbRangées.text = dataEvent.NbRangéesGrille.ToString();
		txtNbColonnes.text = dataEvent.NbColonnesGrille.ToString();
		PanneauÀContrôler.sizeDelta = new Vector2((float)dataEvent.NbColonnesGrille * DeltaDimension.x, (float)dataEvent.NbRangéesGrille * DeltaDimension.y);
		base.transform.localPosition = PanneauÀContrôler.localPosition + new Vector3(PanneauÀContrôler.rect.width, 0f - PanneauÀContrôler.rect.height, 0f);
	}

	public void GérerTailleGrille()
	{
		Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector3 position = new Vector3(Math.Min(Math.Max(vector.x, BornesHorizontales.x), BornesHorizontales.y), Math.Min(Math.Max(vector.y, BornesVerticales.x), BornesVerticales.y));
		base.transform.position = position;
		PanneauÀContrôler.sizeDelta = new Vector2(base.transform.localPosition.x, 0f - base.transform.localPosition.y);
	}

	public void CalculerTailleGrille()
	{
		ColonieJeu = GameManager.GetComponent<Jeu>().ColonieJeu;
		int nbRangées = Mathf.RoundToInt(PanneauÀContrôler.rect.height / DeltaDimension.y);
		int nbColonnes = Mathf.RoundToInt(PanneauÀContrôler.rect.width / DeltaDimension.x);
		ColonieJeu.ModifierDimensionsGrille(nbRangées, nbColonnes, copierColonie: true);
		txtNbColonnes.text = nbColonnes.ToString();
		txtNbRangées.text = nbRangées.ToString();
	}

	private bool EstEntre(float valeur, Vector2 bornes)
	{
		if (valeur > bornes.x)
		{
			return valeur < bornes.y;
		}
		return false;
	}
}
