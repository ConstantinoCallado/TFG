using UnityEngine;
using System.Collections;

public class HumanAI : AIBaseController 
{
	public Vector2 repulsionVector;
	private Vector2 auxVector;

	private Vector2 evadeTarget;

	void Start()
	{
		base.AIEnabled = false;
		base.Start ();
		
		StartCoroutine(startIADelayed());
	}
	
	IEnumerator startIADelayed()
	{
		yield return new WaitForSeconds(Random.Range(0f, 3f));
		base.AIEnabled = true;
	}

	void Update()
	{
		if(base.AIEnabled)
		{
			Evade();
		}
	}

	void Evade()
	{
		repulsionVector = Vector2.zero;

		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano)
			{
				auxVector = ((Vector2)base.player.basicMovementServer.characterTransform.position - (Vector2)NetworkManager.networkManagerRef.listaJugadores[i].player.transform.position);
				repulsionVector += auxVector.normalized * (1 / auxVector.magnitude);
			}
		}

		if(base.pathCompleted || repulsionVector.sqrMagnitude > 1f)
		{
			evadeTarget = base.wlkToRandomPositionAround((Vector2)base.player.basicMovementServer.characterTransform.position + (repulsionVector * 4), (short)(3/repulsionVector.magnitude));
		}
	}
}
