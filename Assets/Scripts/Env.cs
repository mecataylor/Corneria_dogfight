using UnityEngine;
using System.Collections;

public class Env : MonoBehaviour {


	public static bool OnAMac(){
		return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXWebPlayer;
	}
}
