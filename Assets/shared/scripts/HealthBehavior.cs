using UnityEngine;
using System.Collections;

public class HealthBehavior : MonoBehaviour {
	
	//max amount of health
	public float health;
	
	//current amount of health
	private float current;
	
	void Start () {
		current = health;
	}
	
	//restore health by amount
	void heal(float amount){
		current += amount;
		if(current > health){
			current = health;
		}
		gameObject.SendMessage("healed");
	}
	
	//restore health fully
	void heal(){
		current = health;
		gameObject.SendMessage("healed");
	}
	
	//deplete health by amount
	void damage(float amount){
		current -= amount;
		gameObject.SendMessage("damaged");
		if(current <= 0){
			gameObject.SendMessage("dead");	
		}
	}
}
