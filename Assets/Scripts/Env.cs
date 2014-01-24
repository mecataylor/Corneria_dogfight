using UnityEngine;
using System.Collections;

public class Env : MonoBehaviour {

	public static int laserDamageAmount = 1;
	public static int enemyFireLayer = 12;
	public static int playerFireLayer = 9;

	public static bool OnAMac(){
		return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXWebPlayer;
	}

	public static bool isDogFight(){
		//decide this later
		return true;
	}
}
