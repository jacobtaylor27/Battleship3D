// InteractionGrille
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionGrille : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private GenerateurGrilleDeJeu ScriptGrille
	{
		get;
		set;
	}

	private Colonie ColonieJeu
	{
		get;
		set;
	}

	private Jeu ScriptParent
	{
		get;
		set;
	}

	public void Awake()
	{
		ScriptGrille = GetComponent<GenerateurGrilleDeJeu>();
	}

	public void Start()
	{
		ColonieJeu = base.gameObject.GetComponentInParent<Jeu>().ColonieJeu;
		ScriptParent = base.transform.parent.gameObject.GetComponent<Jeu>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ColonieJeu = base.gameObject.GetComponentInParent<Jeu>().ColonieJeu;
		Vector3 pointGrille = ObtenirPointGrille(eventData);
		Tuple<int, int> tuple = ObtenirCoordonnéeGrille(pointGrille);
		ScriptParent.ModifierCellule(tuple.Item1, tuple.Item2);
	}

	private Tuple<int, int> ObtenirCoordonnéeGrille(Vector3 pointGrille)
	{
		int item = (int)((pointGrille.x - ScriptGrille.Origine.x) / ScriptGrille.DeltaÉtendue.x % (float)ColonieJeu.NbColonnesGrille);
		return new Tuple<int, int>((int)(Mathf.Abs(pointGrille.y + ScriptGrille.Origine.y) / ScriptGrille.DeltaÉtendue.y % (float)ColonieJeu.NbRangéesGrille), item);
	}

	private Vector3 ObtenirPointGrille(PointerEventData eventData)
	{
		Vector3 position = eventData.pressPosition;
		position.z = Mathf.Abs(Camera.main.transform.position.z);
		return Camera.main.ScreenToWorldPoint(position) - base.transform.localPosition;
	}
}
