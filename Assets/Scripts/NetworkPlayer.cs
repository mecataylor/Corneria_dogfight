using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	private int networkID;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting = false;
	private bool shieldUp = false;
	private int died = -1;
	private int sendHit = -1;
	private int receivedHit = -1;

	void Start(){
		networkID = photonView.viewID;
		if (!photonView.isMine){
			gameObject.SendMessage("myNetViewID", networkID);
		}
	}
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			rigidbody.velocity = correctPlayerVel;
			if(isShooting){
				gameObject.SendMessage("netShoot");
			}
			if(shieldUp){
				gameObject.SendMessage("netShieldUp");
			}
			if(died == photonView.viewID){
				gameObject.SendMessage("netDied");
			}
		}else{
			if(receivedHit == photonView.viewID){
				gameObject.SendMessage("damage", Env.laserDamageAmount);
			}
		}
		resetReceivedVars();
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//We own this player: send the others our data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (rigidbody.velocity);
			stream.SendNext (isShooting);
			stream.SendNext (shieldUp);
			stream.SendNext (sendHit);
			stream.SendNext (died);
			//now reset all the sent variables
			resetSentVars();
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			shieldUp = (bool) stream.ReceiveNext();
			receivedHit = (int) stream.ReceiveNext();
			died = (int) stream.ReceiveNext();
		}
	}

	public void resetSentVars(){
		isShooting = false;
		shieldUp = false;
		sendHit = -1;
		died = -1;
	}

	public void resetReceivedVars(){
		receivedHit = -1;
	}

	public void NetworkShoot(){
		isShooting = true;
	}

	public void NetworkShield(){
		shieldUp = true;
	}

	//I died
	public void dead(){
		died = photonView.viewID;
	}

	//I hit someone
	public void NetworkHit(int playerID){
		sendHit = playerID;
	}
}
