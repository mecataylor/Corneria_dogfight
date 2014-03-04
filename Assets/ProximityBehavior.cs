using UnityEngine;
using System.Collections;

public class ProximityBehavior : MonoBehaviour {

	void OnTriggerEnter (Collider col) {
		if(col.gameObject.layer == Env.playerLayer){
			transform.parent.gameObject.SendMessage("attack", col.gameObject);
		}
	}
}
