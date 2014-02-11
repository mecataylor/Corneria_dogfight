using UnityEngine;
using System.Collections;

public class shieldBehavior : MonoBehaviour {

	public float duration = 0.5f;
	public Transform explosion;
	
	void OnCollisionEnter(Collision col){
		Debug.Log("Getting shot");
		int layer = Env.enemyFireLayer;
		if(Env.isDogFight()){
			layer = Env.droneFireLayer;
		}
		if(col.gameObject.layer == layer){
			Destroy(col.gameObject);
			//Add an explosion at the site of the collision
			Instantiate(explosion, col.transform.position, col.transform.rotation);
		}
	}
	
	void OnEnable(){
		audio.Play();
		StartCoroutine(Disable(gameObject, duration));
	}
	
	IEnumerator Disable(GameObject target, float duration) {
		yield return new WaitForSeconds(duration);
		target.SetActive(false);
	}
}
