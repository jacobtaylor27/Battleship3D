// GestionBoutons
using UnityEngine;
using UnityEngine.UI;

public class GestionBoutons : MonoBehaviour
{
	private Vector3 vecteurBoutonAgrandi
	{
		get;
		set;
	}

	private void Start()
	{
		vecteurBoutonAgrandi = new Vector3(1.5f, 1.5f, 1f);
	}

	public void AgrandirBouton()
	{
		base.transform.localScale = vecteurBoutonAgrandi;
	}

	public void RétrécirBouton()
	{
		base.transform.localScale = Vector3.one;
	}

	public void ChangerNomBoutonActivation()
	{
		Text componentInChildren = GetComponentInChildren<Text>();
		componentInChildren.text = ((componentInChildren.text == "Démarrer") ? "Pause" : "Démarrer");
	}
}
