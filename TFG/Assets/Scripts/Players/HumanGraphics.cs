using UnityEngine;
using System.Collections;

public class HumanGraphics : MonoBehaviour 
{
	public Color colorAgresivo;

	public Renderer[] renderersDeColores;

	public void SetAggressive (bool status)
	{
		if(status)
		{
			setColor(colorAgresivo);
		}
		else
		{
			setColor(Color.white);
		}
	}

	public void setColor(Color color)
	{
		for(int i=0; i< renderersDeColores.Length; i++)
		{
			renderersDeColores[i].material.color = color;
		}
	}

	public void Kill()
	{
		gameObject.transform.localScale = new Vector3(1.2f, 1.1f, 0.1f);
	}

	public void UnKill()
	{
		gameObject.transform.localScale = Vector3.one;
	}
}
