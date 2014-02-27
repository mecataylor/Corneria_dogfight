using UnityEngine;
using System.Collections;

public class BulletKill : MonoBehaviour {

	public float lifetime = 2.0f;
	public bool explode = false;
	public Transform explosion;

	void Start(){
		StartCoroutine(remove());
	}

	IEnumerator remove(){
		yield return new WaitForSeconds(lifetime);
		if(explode){
			Instantiate(explosion, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.layer == Env.environmentLayer){
			if(explode){
				Instantiate(explosion, transform.position, transform.rotation);
			}
			Destroy (gameObject);
		}
	}
}