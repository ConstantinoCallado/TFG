using UnityEngine;
using System.Collections;

public class HumanGraphics : MonoBehaviour 
{
	public Color colorAgresivo;

	public Renderer[] renderersDeColores;
	public Animator humanAnimator;

	public Vector2 oldPosition;
	public Vector2 newPosition;

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
		gameObject.transform.localScale = new Vector3(1.2f, 0.1f, 1.1f);
		humanAnimator.enabled = false;
	}

	public void UnKill()
	{
		gameObject.transform.localScale = Vector3.one;
		humanAnimator.enabled = true;
	}

	void Update()
	{
		newPosition = transform.position;
		if(newPosition != oldPosition)
		{
			Debug.Log("CORRER");
			humanAnimator.SetBool("run", true);
		}
		else
		{
			Debug.Log("QUIETO");
			humanAnimator.SetBool("run", false);
		}

		oldPosition = newPosition;
	}
}
