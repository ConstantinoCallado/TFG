using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFog : MonoBehaviour 
{
	public bool actualIsHumanInSight = false;
	public bool oldIsHumanInSight = false;
	public static bool warfogEnabled = true;
	[HideInInspector]
	//public List<Sight> listaVisiones = new List<Sight>();
	public static WarFog warFogRef;

	public void Awake()
	{
		warFogRef = this;
	
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].ownByClient && NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje == EnumPersonaje.Humano)
			{
				RemoveFOW();
				break;
			}
		}
	}
	
	public void RemoveFOW ()
	{
		GameObject.Destroy(GameObject.FindWithTag("FogOfWar"));

		WarFog.warfogEnabled = false;
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
