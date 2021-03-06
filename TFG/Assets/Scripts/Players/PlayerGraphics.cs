﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGraphics : MonoBehaviour 
{
	public HumanGraphics humanGraphics;
	public RobotGraphics robotGraphics;
	public HoveringName hoveringName;
	public List<Sprite> listaIconos = new List<Sprite>();

	public GameObject prefabIndicador;


	public void setHuman()
	{
		humanGraphics.gameObject.SetActive(true);
		Destroy(robotGraphics.gameObject);
	}

	public void setRobot(Color color)
	{
		Destroy(humanGraphics.gameObject);
		robotGraphics.gameObject.SetActive(true);
		robotGraphics.setColor(color);
	}

	public void SetTransparent (float param)
	{
		if(robotGraphics != null)
		{
			robotGraphics.SetTransparent(param);
		}
	}
	
	public void EnableGraphics(bool status)
	{
		hoveringName.active = status;

		if(humanGraphics != null)
		{
			humanGraphics.gameObject.SetActive(status);
		}
		if(robotGraphics != null)
		{
			robotGraphics.gameObject.SetActive(status);
		}
	}

	public void Kill()
	{
		if(humanGraphics != null) humanGraphics.Kill();
		if(robotGraphics != null) robotGraphics.Kill();
	}

	public void Unkill ()
	{
		if(humanGraphics != null) humanGraphics.UnKill();
		if(robotGraphics != null) robotGraphics.UnKill();
	}

	public void SetAggressive(bool status)
	{
		if(humanGraphics != null) humanGraphics.SetAggressive(status);
	}
}
