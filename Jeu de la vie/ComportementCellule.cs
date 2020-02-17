// ComportementCellule
using UnityEngine;

public class ComportementCellule : MonoBehaviour
{
	private Vector3 VecteurDéplacement
	{
		get;
		set;
	}

	private bool EstEnTrainDeNaitre
	{
		get;
		set;
	}

	private bool EstEnTrainDeMourir
	{
		get;
		set;
	}

	private void Start()
	{
		base.transform.Translate(Vector3.forward);
		VecteurDéplacement = new Vector3(0f, 0f, -0.1f);
		EstEnTrainDeNaitre = true;
		EstEnTrainDeMourir = false;
	}

	private void Update()
	{
		if (EstEnTrainDeNaitre)
		{
			base.transform.Translate(VecteurDéplacement);
			EstEnTrainDeNaitre = (base.transform.localPosition.z > 0f);
		}
		if (EstEnTrainDeMourir)
		{
			base.transform.Translate(-VecteurDéplacement);
			if (base.transform.localPosition.z > 1f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Tuer()
	{
		EstEnTrainDeMourir = true;
	}
}
