using UnityEngine;
using System.Collections;

public class Sightable : MonoBehaviour 
{
	public short numberOfSighters = 0;
	public GameObject gameObjectGraphics;

	public virtual void Start()
	{
		if(WarFog.warFogRef.warfogEnabled)
		{
			gameObjectGraphics.SetActive(false);
		}
	}

	public virtual void sightInRange()
	{
		++numberOfSighters;

		if(numberOfSighters > 0 && WarFog.warFogRef.warfogEnabled)
		{
			gameObjectGraphics.SetActive(true);
		}
	}

	public virtual void sightOutOfRange()
	{
		--numberOfSighters;

		if(numberOfSighters == 0 && WarFog.warFogRef.warfogEnabled)
		{
			gameObjectGraphics.SetActive(false);
		}
	}
}
