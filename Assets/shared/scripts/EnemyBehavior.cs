using UnityEngine;
using System.Collections;

public class EnemyBehavior : Photon.MonoBehaviour {

	public Rigidbody projectile;
	public GameObject DeathExplosion;
	public float shootFrequency;
	//The middle component of the enemy
	public Transform middle;
	//The tallest component of the enemy
	public Transform height;
	public float menaceMax = 50f;
	public float menaceSpeed = 10f;

	private bool attacking = false;
	private GameObject player;
	private float yOrig;
	private bool menacingUp;


	void Start(){
		yOrig = transform.position.y;
		menacingUp = true;
	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.layer == Env.playerFireLayer){
			gameObject.SendMessage("damage", 1);
			Destroy (col.gameObject);
		}
	}
	
	void attack(GameObject toAttack){
		if(!attacking){
			attacking = true;
			player = toAttack.transform.parent.parent.gameObject;
			int playerID = getViewID(player);
			photonView.RPC ("targetAquired", PhotonTargets.All, new int[] {playerID, photonView.viewID});
			StartCoroutine(fire());
		}
	}

	int getViewID(GameObject target){
		PhotonView playerView = player.GetComponent("PhotonView") as PhotonView;
		return playerView.viewID;
	}
	
	void Update(){
		if(PhotonNetwork.isMasterClient){
			if (attacking && player.activeInHierarchy){
				Attacking(player);
			}else{
				attacking = false;
				photonView.RPC ("voidTarget", PhotonTargets.All, photonView.viewID);
				float current_y = transform.position.y;
				if(menacingUp){
					moveY(current_y + menaceSpeed);
				}else{
					moveY(current_y - menaceSpeed);
				}
				MenacingDirection(current_y);
			}
		}
	}

	void Attacking(GameObject player){
		transform.LookAt(player.transform);
		//Lerp to a position just infront of the player
		Vector3 newposition = player.transform.position;
		newposition.z = player.transform.position.z + 100;
		newposition.y = player.transform.position.y - (height.renderer.bounds.size.y / 2);
		transform.position = Vector3.Lerp(transform.position, newposition, Time.deltaTime);
	}

	IEnumerator fire(){
		while(attacking){
			photonView.RPC ("baddieFire", PhotonTargets.All, photonView.viewID);
			shootPlasma();
			yield return new WaitForSeconds(shootFrequency);
		}
	}

	void shootPlasma(){
		Rigidbody projectileInstance = Instantiate(projectile, middle.position, transform.rotation) as Rigidbody;
		if(player){
			projectileInstance.SendMessage("setTarget", player);
		}
	}

	void MenacingDirection(float current_y){
		if(current_y < (yOrig - menaceMax)){
			menacingUp = true;
		}else if(current_y > (yOrig + menaceMax)){
			menacingUp = false;
		}
	}

	void moveY(float new_y){
		Vector3 newposition = new Vector3(transform.position.x, new_y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, newposition, Time.deltaTime);
	}
	
	//message from Health class
	void healed(){
		
	}
	
	//message from Health class
	void dead(){
		Instantiate(DeathExplosion, transform.position, transform.rotation);
		photonView.RPC("destroyBaddie", PhotonTargets.MasterClient, photonView.viewID);
	}

	[RPC]
	void destroyBaddie(int viewID){
		if(photonView.viewID == viewID){
			PhotonNetwork.Destroy(gameObject);
		}
	}

	[RPC]
	void targetAquired(int[] ids){
		if(photonView.viewID == ids[1] ){
			PhotonView target = PhotonView.Find (ids[0]);
			player = target.transform.gameObject;
			attacking = true;
			StartCoroutine(fire());
		}
	}

	[RPC]
	void voidTarget(int viewID){
		if(photonView.viewID == viewID){
			attacking = false;
		}
	}

	[RPC]
	void baddieFire(int viewID){
		if(photonView.viewID == viewID){
			shootPlasma();
		}
	}
	
	//message from Health class
	void damaged(){
		
	}
}
