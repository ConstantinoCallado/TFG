using UnityEngine;
using System.Collections;

public class RobotGraphics : MonoBehaviour 
{
	public Renderer[] renderersDeColores;

	public void setColor(Color color)
	{
		Debug.Log("seteando color " + color);
		for(int i=0; i< renderersDeColores.Length; i++)
		{
			renderersDeColores[i].material.color = color;
		}
	}
}
