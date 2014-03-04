using UnityEngine;
using System.Collections;

public class EnemyBehavior : Photon.MonoBehaviour {

	public Rigidbody projectile;
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
		attacking = true;
		player = toAttack;
		StartCoroutine(fire());
	}
	
	void Update(){
		if (attacking && player){
			Attacking(player);
		}else{
			float current_y = transform.position.y;
			if(menacingUp){
				moveY(current_y + menaceSpeed);
			}else{
				moveY(current_y - menaceSpeed);
			}
			MenacingDirection(current_y);
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
			Rigidbody projectileInstance = Instantiate(projectile, middle.position, transform.rotation) as Rigidbody;
			if(player){
				projectileInstance.SendMessage("setTarget", player);
			}
			yield return new WaitForSeconds(shootFrequency);
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
		PhotonNetwork.Destroy(gameObject);
	}
	
	//message from Health class
	void damaged(){
		
	}
}
