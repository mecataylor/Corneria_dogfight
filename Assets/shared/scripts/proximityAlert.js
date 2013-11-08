#pragma strict

public var enemies : Transform[];
public var player : GameObject;

function OnTriggerEnter (other : Collider) {
	if(other.gameObject.name == player.name){
		for(var enemy : Transform in enemies){
			if(enemy){
				enemy.SendMessage("attack", other.gameObject);
			}
		}
	}
}