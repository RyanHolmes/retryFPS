using UnityEngine;
using System.Collections;
//*********************************************************************************************
//objects to be synched must have a photon view script
//*********************************************************************************************
public class NetworkManager : MonoBehaviour {
	
	public Camera standbyCamera;
	
	// Use this for initialization
	void Start () {
		Connect ();
	}
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings( "MultiFPS v1.0.0" );
	}
	
	void OnGUI() {
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
	}
	
	void OnJoinedLobby() {
		Debug.Log ("Joined Lobby");
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed() {
		Debug.Log ("Failed to join room, one will be made");
		PhotonNetwork.CreateRoom( null );
	}
	
	void OnJoinedRoom() {
		Debug.Log ("Joined room");	
		SpawnMyPlayer();
	}
	
	void SpawnMyPlayer() {
		//use photon instantiate so it is created on all network computers not just yours
		PhotonNetwork.Instantiate ("PlayerController", Vector3.zero, Quaternion.identity, 0);
	}
}