using UnityEngine;
using System.Collections;

public class Sightable : MonoBehaviour 
{
	public short numberOfSighters = 0;
	public GameObject gameObjectGraphics;

	public virtual void Awake()
	{
		gameObjectGraphics.SetActive(false);
	}

	public virtual void sightInRange()
	{
		++numberOfSighters;

		if(numberOfSighters > 0)
		{
			gameObjectGraphics.SetActive(true);
		}
	}

	public virtual void sightOutOfRange()
	{
		--numberOfSighters;

		if(numberOfSighters == 0)
		{
			gameObjectGraphics.SetActive(false);
		}
	}
}
