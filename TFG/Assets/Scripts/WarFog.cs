using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFog : MonoBehaviour 
{
	public bool actualIsHumanInSight = false;
	public bool oldIsHumanInSight = false;
	public bool warfogEnabled = true;
	[HideInInspector]
	//public List<Sight> listaVisiones = new List<Sight>();
	public static WarFog warFogRef;

	public void Awake()
	{
		warFogRef = this;
	}

	public void Start()
	{
		if(Network.isServer)
		{
			StartCoroutine(coroutineCheckSight());
		}
	}

	public IEnumerator coroutineCheckSight()
	{
		while(true)
		{
			if(Human.humanRef.sightable.numberOfSighters > 0)
			{
				actualIsHumanInSight = true;
			}
			else
			{
				actualIsHumanInSight = false;
			}

			if(actualIsHumanInSight != oldIsHumanInSight)
			{
				if(actualIsHumanInSight)
				{
					AIBaseController.humanInSight = true;
					AIBaseController.humanKnownPositionPrev = AIBaseController.humanKnownPosition;
					AIBaseController.humanKnownPosition = Human.humanRef.transform.position;
				}
				else
				{
					AIBaseController.humanInSight = false;
				}

				oldIsHumanInSight = actualIsHumanInSight;
			}

			yield return new WaitForSeconds(0.1f);
		}
	}
}
