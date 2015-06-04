using UnityEngine;
using System.Collections;

public class WarFog : MonoBehaviour 
{
	public bool canSightHuman = false;
	public bool oldCanSightHuman = true;

	void Update()
	{
		if(Human.humanRef)
		{
			canSightHuman = Sight.isHumanInSight;

			if(canSightHuman != oldCanSightHuman)
			{
				// TODO: Activar o desactivar la vision del jugador
				Human.humanRef.playerGraphics.EnableGraphics(canSightHuman);

				oldCanSightHuman = canSightHuman;
			}
		}
	}

	void LateUpdate () 
	{
		//TODO: Actualizar niebla de guerra
	}
}
