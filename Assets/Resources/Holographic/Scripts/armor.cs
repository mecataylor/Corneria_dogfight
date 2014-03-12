﻿using UnityEngine;
using System.Collections;

public class armor : MonoBehaviour
{
	//An array of Objects that stores the results of the Resources.LoadAll() method
	private Object[] objects;
	//Each returned object is converted to a Texture and stored in this array
	private Texture[] textures;
	//With this Material object, a reference to the game object Material can be stored
	private Material goMaterial;
	//An integer to advance frames
	private int frameCounter = 0;	
	private float a=1; //alpha control
	public static float armor_=100;

	private ShipBehavior SHBehavior;
	public GameObject shipRef;
	
	void Awake()
	{
		//Get a reference to the Material of the game object this script is attached to
		this.goMaterial = this.renderer.material;
		this.renderer.material.color = new Color(this.renderer.material.color.r,this.renderer.material.color.b,this.renderer.material.color.g,.65f*a);
	}

	void Start ()
	{
		//print(this.renderer.material.color);
		//Load all textures found on the Sequence folder, that is placed inside the resources folder
		this.objects = Resources.LoadAll("Holographic/output/main/armor", typeof(Texture));

		//Initialize the array of textures with the same size as the objects array
		this.textures = new Texture[objects.Length];

		//Cast each Object to Texture and store the result inside the Textures array
		for(int i=0; i < objects.Length;i++)
		{
			this.textures[i] = (Texture)this.objects[i];
		}

		SHBehavior = shipRef.GetComponent("ShipBehavior") as ShipBehavior;
	}
	
	
	void OnGUI () {
		}

	void Update ()
	{
		//armor_=sliders.armor;

		armor_ = SHBehavior.isMissileReady();
		
		if (a!=sliders.opacity){
			a= sliders.opacity;
this.renderer.material.color = new Color(this.renderer.material.color.r,this.renderer.material.color.b,this.renderer.material.color.g,.65f*a);
		}
		frameCounter=(int)Mathf.Round((100f-armor_)/5.263f);
		goMaterial.mainTexture = textures[frameCounter];

	}

  

}