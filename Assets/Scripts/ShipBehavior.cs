using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {
	
	private GameObject shield;

	void Start () {
		shield = transform.Find("shield").gameObject;
	}
	
	void Update () {
		//check for shield up
		if(Input.GetButtonDown("Jump")){
			shield.SetActive(true);	
		}
	}
}
