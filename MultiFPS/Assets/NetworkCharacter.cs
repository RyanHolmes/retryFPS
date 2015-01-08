using UnityEngine;
using System.Collections;

//photon view on character is tracking this script -- notice inheritence
//this is tracked instead of tranform to smooth animations
public class NetworkCharacter : Photon.MonoBehaviour {
	
	public Vector3 realPosition = Vector3.zero;
	public Quaternion realRotation = Quaternion.identity;
	
	// Use this for initialization
	void Start () {}	

	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			//Do nothing -> its mine
		} 
		else {
			//the purpose of this script -> blending by 10%
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
		}
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			//our player! send our position to network
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else{
			//someone elses player. need to receive position (as of a few 
			//millseconds ago) and update our version o fhtat player
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}