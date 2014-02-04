using UnityEngine;
using System.Collections;

public class InfoPanels : MonoBehaviour {

	public Transform computer;
	public Material computer_flash_color;
	public float computer_flash_time = 0.5f;

	private Material computer_color;

	void Start(){
		computer_color = computer.renderer.material;
	}

	void hit()
	{
		computer.renderer.material = computer_flash_color;
		StartCoroutine(resetComputerColor(computer));
	}

	IEnumerator resetComputerColor(Transform comp){
		yield return new WaitForSeconds(computer_flash_time);
		comp.renderer.material = computer_color;
	}
}
