using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour 
{
	public HumanGraphics humanGraphics;
	public RobotGraphics robotGraphics;
	public HoveringName hoveringName;

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
	
	public void EnableGraphics(bool status)
	{
		hoveringName.active = status;
		if(humanGraphics != null)
		{
			humanGraphics.gameObject.SetActive(status);
			if(status) humanGraphics.UnKill();
		}
		if(robotGraphics != null)
		{
			robotGraphics.gameObject.SetActive(status);
			if(status) robotGraphics.UnKill();
		}
	}

	public void Kill()
	{
		if(humanGraphics != null) humanGraphics.Kill();
		if(robotGraphics != null) robotGraphics.Kill();
	}

	public void SetAggressive(bool status)
	{
		if(humanGraphics != null) humanGraphics.SetAggressive(status);
	}
}
