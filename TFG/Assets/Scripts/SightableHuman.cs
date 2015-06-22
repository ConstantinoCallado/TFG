using UnityEngine;
using System.Collections;

public class SightableHuman : Sightable 
{
	public Human humanRef;
	private bool isInSight = false;
	

	void LateUpdate()
	{
		if(WarFog.warfogEnabled)
		{
			if(numberOfSighters > 0)
			{
				if(!isInSight)
				{
					isInSight = true;
					humanRef.playerGraphics.EnableGraphics(true);
				}
			}
			else
			{
				if(isInSight)
				{
					isInSight = false;
					humanRef.playerGraphics.EnableGraphics(false);
				}
			}
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
