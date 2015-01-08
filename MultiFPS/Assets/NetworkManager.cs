using UnityEngine;
using System.Collections;
//*********************************************************************************************
//objects to be synched must have a photon view script
//photon view needs to be given something to keep track of -> e.g. a transform
//problem: the players both have cams, and wasd controlls (you can contoll both). to fix this
//disable them to begin with and individually enable them for each player -- look in spawnMyPlayer
//*********************************************************************************************
public class NetworkManager : MonoBehaviour {
	
	public GameObject standbyCamera;
	public SpawnSpot[] spawnspots;
	
	// Use this for initialization
	void Start () {
		spawnspots = GameObject.FindObjectsOfType <SpawnSpot> ();
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
		SpawnSpot mySpawnSpot = spawnspots [Random.Range (0, spawnspots.Length)];
		//use photon instantiate so it is created on all network computers not just yours
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate ("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		//enable scripts and cameras
		((MonoBehaviour)myPlayerGO.GetComponent ("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent ("CharacterMotor")).enabled = true;
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
		//we want to use player cam not main
		standbyCamera.SetActive(false);
	}
}