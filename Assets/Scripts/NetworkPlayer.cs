using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour {
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector3 correctPlayerVel;
	private bool isShooting = false;
	private bool shieldUp = false;
	private bool died = false;
	private bool gotAHit = false;
	private int hitID = -1;
	
	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			rigidbody.velocity = correctPlayerVel;
			if(isShooting){
				transform.SendMessage("netShoot");
			}else{
				transform.SendMessage("netStopShoot");
			}
			if(shieldUp){
				gameObject.SendMessage("netShieldUp");
			}
			if(died){
				transform.SendMessage("netDied");
			}
			if(gotAHit && hitID == photonView.viewID){
				transform.SendMessage("damage", 1);
			}
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isWriting){
			//We own this player: send the others our data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (rigidbody.velocity);
			stream.SendNext (isShooting);
			stream.SendNext (shieldUp);
			stream.SendNext (gotAHit);
			stream.SendNext (hitID);
			stream.SendNext (died);
			//now reset all the sent variables
			resetVars();
		}
		else{
			//Network Player, receive data
			correctPlayerPos = (Vector3) stream.ReceiveNext();
			correctPlayerRot = (Quaternion) stream.ReceiveNext();
			correctPlayerVel = (Vector3) stream.ReceiveNext();
			isShooting = (bool) stream.ReceiveNext();
			shieldUp = (bool) stream.ReceiveNext();
			died = (bool) stream.ReceiveNext();
			gotAHit = (bool) stream.ReceiveNext();
			hitID = (int) stream.ReceiveNext();
		}
	}

	public void resetVars(){
		isShooting = false;
		shieldUp = false;
		gotAHit = false;
		hitID = -1;
		died = false;
	}

	public void NetworkShoot(){
		isShooting = true;
	}

	public void NetworkShield(){
		shieldUp = true;
	}

	public void dead(){
		died = true;
	}

	public void NetworkHit(int playerID){
		hitID = playerID;
		gotAHit = true;
	}
}
