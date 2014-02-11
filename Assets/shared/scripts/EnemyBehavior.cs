using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public Rigidbody projectile;
	public float shootFrequency;
	//The middle component of the enemy
	public Transform middle;
	//The tallest component of the enemy
	public Transform height;
	
	private bool attacking = false;
	private GameObject player;
	
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
			transform.LookAt(player.transform);
			//Lerp to a position just infront of the player
			Vector3 newposition = player.transform.position;
			newposition.z = player.transform.position.z + 100;
			newposition.y = player.transform.position.y - (height.renderer.bounds.size.y / 2);
			transform.position = Vector3.Lerp(transform.position, newposition, Time.deltaTime);
		}
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
	
	//message from Health class
	void healed(){
		
	}
	
	//message from Health class
	void dead(){
		Destroy (gameObject);
	}
	
	//message from Health class
	void damaged(){
		
	}
}
