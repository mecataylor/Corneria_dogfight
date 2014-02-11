using UnityEngine;
using System.Collections;

public class EnemySpawn : Photon.MonoBehaviour {

	public Transform[] enemy_transforms;
	public int num_of_baddies;
	
	private GameObject[] respawnLocations;
	private int respawn_locations_count;
	private int enemy_count;
	
	void Start () {
		respawnLocations = GameObject.FindGameObjectsWithTag("Respawn");
		respawn_locations_count = respawnLocations.Length;
	}

	void Update () {
		if(PhotonNetwork.isMasterClient){
			//check how many enemies there are currently
			enemy_count = GameObject.FindGameObjectsWithTag("Enemy").Length;
			Debug.Log("max: " + (enemy_count < num_of_baddies));
			if(enemy_count < num_of_baddies){
				spawn();
			}
		}
	}

	void spawn(){
		Transform enemy = randomEnemy();
		GameObject location = randomLocation();
		PhotonNetwork.InstantiateSceneObject(enemy.name, location.transform.position, Quaternion.identity, 0, new object[0]);
	}

	Transform randomEnemy(){
		int r = Random.Range(0, enemy_count);
		return enemy_transforms[r];
	}

	GameObject randomLocation(){
		int r = Random.Range(0, respawn_locations_count);
		return respawnLocations[r];
	}
}
