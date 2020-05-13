//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class ControlleurAccueil : MonoBehaviour
//{
//    Button BoutonJouer { get; set; }
//    Button BoutonQuitter { get; set; }
//    public TextMeshProUGUI TexteBoutonJouer { get; set; }

//    void Start()
//    {
//        DéfinirValeursParDéfaut();
//        AssignerCallbacks();
//    }

//    public void DéfinirValeursParDéfaut()
//    {
//        BoutonJouer = GameObject.Find("CanvasAccueil").GetComponentsInChildren<Button>().First(x => x.name == "BtnJouer");
//        BoutonQuitter = GameObject.Find("CanvasAccueil").GetComponentsInChildren<Button>().First(x => x.name == "BtnQuitter");

//        TexteBoutonJouer = BoutonJouer.GetComponentInChildren<TextMeshProUGUI>();
//    }

//    void AssignerCallbacks()
//    {
//        BoutonJouer.onClick.AddListener(Jouer);
//        BoutonQuitter.onClick.AddListener(Quitter);
//    }

//    void Quitter()
//    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_WEBPLAYER
//            Application.OpenURL(webplayerQuitURL);
//#else
//            Application.Quit();
//#endif
//    }

//    void Jouer() => SceneManager.LoadScene("GameScene");
//}
