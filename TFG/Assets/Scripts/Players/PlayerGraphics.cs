using UnityEngine;
using System.Collections;

public class PlayerGraphics : MonoBehaviour 
{
	public HumanGraphics humanGraphics;
	public RobotGraphics robotGraphics;

	public void setHuman()
	{
		humanGraphics.gameObject.SetActive(true);
		robotGraphics.gameObject.SetActive(false);
	}

	public void setRobot(Color color)
	{
		humanGraphics.gameObject.SetActive(false);
		robotGraphics.gameObject.SetActive(true);
		robotGraphics.setColor(color);
	}
}
