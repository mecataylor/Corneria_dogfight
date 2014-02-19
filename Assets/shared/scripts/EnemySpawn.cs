using UnityEngine;
using System.Collections;

public class EnemySpawn : Photon.MonoBehaviour {

	public Transform[] enemy_transforms;
	public int num_of_baddies;
	public int randomDistanceFromSpawn = 20;
	
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
			if(enemy_count < num_of_baddies){
				spawn();
			}
		}
	}

	void spawn(){
		Transform enemy = randomEnemy();
		GameObject location = randomLocation();
		Vector3 newPosition = addRandomAmounts(location.transform.position);
		PhotonNetwork.InstantiateSceneObject(enemy.name, newPosition, Quaternion.identity, 0, new object[0]);
	}

	Transform randomEnemy(){
		int r = Random.Range(0, enemy_count);
		return enemy_transforms[r];
	}

	GameObject randomLocation(){
		int r = Random.Range(0, respawn_locations_count);
		return respawnLocations[r];
	}

	Vector3 addRandomAmounts(Vector3 position){
		float x, y, z;
		int randX = Random.Range(20, randomDistanceFromSpawn);
		int randY = Random.Range(20, randomDistanceFromSpawn);
		int randZ = Random.Range(20, randomDistanceFromSpawn);
		return new Vector3(position.x + randX, position.y + randY, position.z + randZ);
	}
}
