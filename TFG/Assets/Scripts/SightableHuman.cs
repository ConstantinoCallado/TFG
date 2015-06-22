using UnityEngine;
using System.Collections;

public class SightableHuman : Sightable 
{
	public Human humanRef;

	//TODO: Si es el humano el que esta jugando: debe ver todo el mapa
	public override void Awake()
	{
	}

	void LateUpdate()
	{
		if(numberOfSighters > 0)
		{
			humanRef.playerGraphics.EnableGraphics(true);
		}
		else
		{
			humanRef.playerGraphics.EnableGraphics(false);
		}

	}

	public override void sightInRange()
	{
		++numberOfSighters;		
	}
	
	public override void sightOutOfRange()
	{
		--numberOfSighters;
	}
}
