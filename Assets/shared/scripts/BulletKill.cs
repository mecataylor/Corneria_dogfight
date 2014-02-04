using UnityEngine;
using System.Collections;

public class BulletKill : MonoBehaviour {

	public float lifetime = 2.0f;
 
    void Awake()
    {
        Destroy(gameObject, lifetime);
    }

	void OnCollisionEnter(Collision col)
	{
		Debug.Log ("Layer: " + gameObject.layer);
		if(col.gameObject.layer == Env.environmentLayer){
			Destroy (gameObject);
		}
	}
}