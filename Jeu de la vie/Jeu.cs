// Jeu
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Jeu : MonoBehaviour
{
	[SerializeField]
	private string NomFichierSortie;

	[SerializeField]
	private string NomFichierEntrée;

	private const string CheminAccèsData = "Assets/Resources/Data/";

	private GameObject GrilleDeJeu
	{
		get;
		set;
	}

	private GenerateurGrilleDeJeu ScriptGrilleDeJeu
	{
		get;
		set;
	}

	private float IntervalleMAJ
	{
		get;
		set;
	}

	private float TempsÉcouléDepuisMAJ
	{
		get;
		set;
	}

	private bool EstSimulationActivée
	{
		get;
		set;
	}

	private GameObject[,] ColonieVisuelle
	{
		get;
		set;
	}

	private GameObject CelluleVisuelle
	{
		get;
		set;
	}

	private Vector3 OrigineCellule
	{
		get;
		set;
	}

	private float ÉchelleCellule
	{
		get;
		set;
	}

	public Colonie ColonieJeu
	{
		get;
		private set;
	}

	private void Awake()
	{
		IntervalleMAJ = 0.5f;
		CelluleVisuelle = (GameObject)Resources.Load("Prefabs/Cellule");
	}

	private void Start()
	{
		ColonieJeu = new Colonie();
		GrilleDeJeu = GameObject.Find("Grille");
		ScriptGrilleDeJeu = GrilleDeJeu.GetComponent<GenerateurGrilleDeJeu>();
		ColonieJeu.DimensionsGrilleModifiée += ScriptGrilleDeJeu.CréerGrilleVisuelle;
		ColonieJeu.DimensionsGrilleModifiée += CréerColonieVisuelle;
		ColonieJeu.DimensionsGrilleModifiée += CréerColonieLogique;
		ColonieJeu.CelluleModifiée += ModifierGrilleVisuelle;
	}

	public void Update()
	{
		if (EstSimulationActivée)
		{
			float deltaTime = Time.deltaTime;
			TempsÉcouléDepuisMAJ += deltaTime;
			if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
			{
				ColonieJeu.EffectuerMiseÀJour();
				TempsÉcouléDepuisMAJ -= IntervalleMAJ;
			}
		}
	}

	private void CréerColonieLogique(object sender, DimensionGrilleEventArgs dataEvent)
	{
		if (dataEvent.CopierColonie)
		{
			ColonieJeu = new Colonie(ColonieJeu, dataEvent);
		}
	}

	private void CréerColonieVisuelle(object sender, DimensionGrilleEventArgs dataEvent)
	{
		if (ColonieVisuelle != null)
		{
			DétruireColonieVisuelle();
		}
		ColonieVisuelle = new GameObject[dataEvent.NbRangéesGrille, dataEvent.NbColonnesGrille];
		Vector3 vector = ScriptGrilleDeJeu.DeltaÉtendue / 2f;
		OrigineCellule = new Vector3(ScriptGrilleDeJeu.Origine.x + vector.x, 0f - ScriptGrilleDeJeu.Origine.y - vector.y) + GrilleDeJeu.transform.localPosition;
		ÉchelleCellule = Mathf.Min(vector.x, vector.y) * 0.7f;
	}

	private void DétruireColonieVisuelle()
	{
		for (int i = 0; i < ColonieVisuelle.GetLength(0); i++)
		{
			for (int j = 0; j < ColonieVisuelle.GetLength(1); j++)
			{
				if (ColonieVisuelle[i, j] != null)
				{
					Object.Destroy(ColonieVisuelle[i, j]);
				}
			}
		}
	}

	private void ModifierGrilleVisuelle(object sender, CelluleEventArgs dataEvent)
	{
		if (dataEvent.État == ÉtatCellule.VIVANT && ColonieVisuelle[dataEvent.NoRangée, dataEvent.NoColonne] == null)
		{
			Vector3 vector = ScriptGrilleDeJeu.DeltaÉtendue;
			Vector3 position = OrigineCellule + new Vector3(vector.x * (float)dataEvent.NoColonne, (0f - vector.y) * (float)dataEvent.NoRangée, 0f);
			ColonieVisuelle[dataEvent.NoRangée, dataEvent.NoColonne] = Object.Instantiate(CelluleVisuelle, position, Quaternion.identity);
			ColonieVisuelle[dataEvent.NoRangée, dataEvent.NoColonne].transform.localScale = new Vector3(ÉchelleCellule, ÉchelleCellule, ÉchelleCellule);
		}
		else if (dataEvent.État == ÉtatCellule.MORT && ColonieVisuelle[dataEvent.NoRangée, dataEvent.NoColonne] != null)
		{
			ColonieVisuelle[dataEvent.NoRangée, dataEvent.NoColonne].GetComponent<ComportementCellule>().Tuer();
		}
	}

	public void ModifierCellule(int rangée, int colonne)
	{
		if (!EstSimulationActivée)
		{
			ColonieJeu.InverserÉtatCellule(rangée, colonne);
		}
	}

	public void EffacerSimulation()
	{
		ColonieJeu.Vider();
	}

	public void ActiverSimulation()
	{
		EstSimulationActivée = !EstSimulationActivée;
	}

	public void SauvegarderSimulation()
	{
		StreamWriter streamWriter = new StreamWriter("Assets/Resources/Data/" + NomFichierSortie);
		streamWriter.WriteLine(ColonieJeu.SerializeToText());
		streamWriter.Close();
	}

	public void ChargerSimulation()
	{
		List<string> list = LireDonnéesFichier(NomFichierEntrée);
		ColonieJeu = ColonieJeu.DeserializeFromText(list[0], list[1]);
	}

	private List<string> LireDonnéesFichier(string nomFichier)
	{
		(new char[1])[0] = '\t';
		List<string> list = new List<string>();
		StreamReader streamReader = new StreamReader("Assets/Resources/Data/" + NomFichierEntrée);
		while (!streamReader.EndOfStream)
		{
			list.Add(streamReader.ReadLine());
		}
		streamReader.Close();
		return list;
	}

	public void Quitter()
	{
		Application.Quit();
	}
}
