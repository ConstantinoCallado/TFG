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

	public void DisableGraphics()
	{
		hoveringName.active = false;
		if(humanGraphics != null) humanGraphics.gameObject.SetActive(false);
		if(robotGraphics != null) robotGraphics.gameObject.SetActive(false);
	}
	
	public void EnableGraphics()
	{
		hoveringName.active = true;
		if(humanGraphics != null) humanGraphics.gameObject.SetActive(true);
		if(robotGraphics != null) robotGraphics.gameObject.SetActive(true);
	}
}
