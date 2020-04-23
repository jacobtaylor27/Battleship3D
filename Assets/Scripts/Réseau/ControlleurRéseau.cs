using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlleurRéseau : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connection établi au serveur : " + PhotonNetwork.CloudRegion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
