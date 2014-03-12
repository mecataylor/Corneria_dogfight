using UnityEngine;
using System.Collections;

public class PlasmaBehavior : MonoBehaviour {

	public float duration = 3f;
	
	private GameObject target = null;
	private float turnSpeed = 1000;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up + Vector3.right, turnSpeed * Time.deltaTime);
		if(target){
			transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
		}
	}

	void setTarget(GameObject toTarget){
		target = toTarget;
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.layer == Env.playerFireLayer){
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
		if(col.gameObject.layer == Env.environmentLayer){
			Destroy (gameObject);
		}
	}
}
