using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour 
{
	byte oldItemInPos;
	Vector2 position;

	public void TurnOn(float time, Vector2 pos)
	{
		position = pos;
		transform.position = position;
		if(Network.isServer)
		{
			BlockPosition();
		}
		else
		{
			Invoke("BlockPosition", 0.2f);
		}

		Invoke("TurnOff", time);
	}

	public void BlockPosition()
	{
		oldItemInPos = Scenario.scenarioRef.arrayNivel[(int)position.y, (int)position.x];
		Scenario.scenarioRef.arrayNivel[(int)position.y, (int)position.x] = 0;
	}

	public void TurnOff()
	{
		Scenario.scenarioRef.arrayNivel[(int)position.y, (int)position.x] = oldItemInPos;
		Destroy(gameObject);
	}
}
