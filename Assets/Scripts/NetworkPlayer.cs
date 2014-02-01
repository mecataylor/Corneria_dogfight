using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {

	private int networkID;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting = false;
	private bool shieldUp = false;

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
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			shieldUp = (bool) stream.ReceiveNext();
		}
	}

	public void resetSentVars(){
		isShooting = false;
		shieldUp = false;
	}

	public void NetworkShoot(){
		isShooting = true;
	}

	public void NetworkShield(){
		shieldUp = true;
	}

	//Local player hit someone
	public void NetworkHit(int playerID){
		photonView.RPC("iHitYou", PhotonTargets.All, playerID);
	}

	[RPC]
	void iHitYou(int playerID){
		if(networkID == playerID){
			gameObject.SendMessage("damage", Env.laserDamageAmount);
		}
	}

}
