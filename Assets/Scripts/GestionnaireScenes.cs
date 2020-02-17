using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GestionnaireScenes : MonoBehaviour
{
   List<Button> Boutons { get; set; }
   Button BtnQuitter { get; set; }
   Button BtnScnCaméra { get; set; }
   Button BtnScnCanon { get; set; }
   Button BtnScnVaisseau { get; set; }

   void Awake()
   {
      ChargerBoutons();
      AssocierFonctionsDeRappel();
      ContrôleScèneActive();
   }

   void ChargerBoutons()
   {
      GameObject pnlScènes = gameObject;
      BtnQuitter = pnlScènes.GetComponentsInChildren<Button>().First(x => x.name == "BtnQuitter");
      Boutons = pnlScènes.GetComponentsInChildren<Button>().Where(x => x.name != "BtnQuitter").ToList();
      BtnScnCaméra = Boutons.First(b => b.name == "BtnScèneCaméra");
      BtnScnCanon = Boutons.First(b => b.name == "BtnScèneCanon");
      BtnScnVaisseau = Boutons.First(b => b.name == "BtnScèneVaisseau");
   }

   void AssocierFonctionsDeRappel()
   {
      Boutons.ForEach(bouton => bouton.onClick.AddListener(() => ChargerScene(bouton.name)));
      BtnQuitter.onClick.AddListener(() => Quitter());
   }

   void ChargerScene(string nomBouton)
   {
      string nomScène;
      switch (nomBouton)
      {
         case "BtnScèneCaméra":
            nomScène = "SceneCamera";
            break;
         case "BtnScèneCanon":
            nomScène = "SceneCanon";
            break;
         default: //case "BtnScèneVaisseau":
            nomScène = "SceneVaisseau";
            break;
      }
      SceneManager.LoadScene(nomScène);
   }

   void ContrôleScèneActive()
   {
      Scene scene = SceneManager.GetActiveScene();
      switch (scene.name)
      {
         case "SceneCamera":
            BtnScnCaméra.interactable = false;
            break;
         case "SceneCanon":
            BtnScnCanon.interactable = false;
            break;
         default: //case "SceneVaisseau":
            BtnScnVaisseau.interactable = false;
            break;
      }
   }

   public static void Quitter()
   {
      #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
      #else
            Application.Quit();
      #endif
   }

}
