using UnityEngine;
using System.Collections;

public class HomingMissileBehavior : MonoBehaviour {

	public float acceleration;

	private GameObject myTarget;
	private int baseVelocity;

	// Use this for initialization
	void Start () {
		baseVelocity = 100;
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(transform.forward * (baseVelocity * acceleration));
		if(myTarget){
			transform.LookAt(myTarget.transform);
		}
	}

	void setTarget(GameObject target){
		myTarget = target;
	}

	void setVelocity(int number){
		baseVelocity = number;
	}
}
