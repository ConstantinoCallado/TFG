using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarFog : MonoBehaviour 
{
	public bool actualIsHumanInSight = false;
	public bool oldIsHumanInSight = false;

	[HideInInspector]
	public List<Sight> listaVisiones = new List<Sight>();
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
			actualIsHumanInSight = false;

			for(int i=0; i<listaVisiones.Count && !actualIsHumanInSight; i++)
			{
				if(listaVisiones[i].isPlayerInSight(Human.humanRef.transform.position))
				{
					actualIsHumanInSight = true;
				}
			}

			if(actualIsHumanInSight != oldIsHumanInSight)
			{
				if(actualIsHumanInSight)
				{
					Debug.Log("Humano dentro de vista");
				}
				else
				{
					Debug.Log("Humano fuera de vista");
				}

				oldIsHumanInSight = actualIsHumanInSight;
			}

			yield return new WaitForSeconds(0.1f);
		}
	}
}
