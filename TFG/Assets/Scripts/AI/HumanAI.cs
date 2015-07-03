using UnityEngine;
using System.Collections;

public enum HumanAIStatus {Wander, Attack, Evade}

public class HumanAI : AIBaseController 
{
	public Vector2 repulsionVector;
	private Vector2 auxVector;

	private Vector2 evadeTarget;

	public Vector2 closestEnemyPosition;

	public Vector2 targetPosition;
	public Vector2 oldTargetPosition;


	public HumanAIStatus humanAIStatus = HumanAIStatus.Wander;

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
			Basic();

			switch(humanAIStatus)
			{
				case HumanAIStatus.Wander:
					Wander();
				break;

				case HumanAIStatus.Evade:
					Evade();
				break;

				case HumanAIStatus.Attack:
					Attack();
				break;
			}
			Evade();
		}
	}

	void Basic()
	{
		closestEnemyPosition = new Vector2(100,100);

		// Recorremos todos los jugadores que sean robots y vemos cual es el mas cercano
		for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano && !NetworkManager.networkManagerRef.listaJugadores[i].player.isDead)
			{
				if((base.player.basicMovementServer.characterTransform.position - 
				    NetworkManager.networkManagerRef.listaJugadores[i].player.basicMovementServer.characterTransform.position).sqrMagnitude < closestEnemyPosition.sqrMagnitude)
				{
					closestEnemyPosition = NetworkManager.networkManagerRef.listaJugadores[i].player.basicMovementServer.characterTransform.position;
				}
			}
		}
	}

	void Evade()
	{
		if(!Human.humanRef.aggressiveMode)
		{
			// Si hay enemigos cercanos huimos de ellos
			if((closestEnemyPosition - (Vector2)base.player.basicMovementServer.characterTransform.position).magnitude < 6)
			{
				repulsionVector = Vector2.zero;

				for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length; i++)
				{
					if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano && !NetworkManager.networkManagerRef.listaJugadores[i].player.isDead)
					{
						auxVector = ((Vector2)base.player.basicMovementServer.characterTransform.position - (Vector2)NetworkManager.networkManagerRef.listaJugadores[i].player.transform.position);
						repulsionVector += auxVector.normalized * (1 / auxVector.magnitude);
					}
				}

				if(base.pathCompleted || repulsionVector.sqrMagnitude > 1f)
				{
					evadeTarget = base.wlkToRandomPositionAround((Vector2)base.player.basicMovementServer.characterTransform.position + (repulsionVector.normalized * 6), 3);
				}
			}
			else
			{
				humanAIStatus = HumanAIStatus.Wander;
			}
		}
		else
		{
			humanAIStatus = HumanAIStatus.Attack;
		}
	}

	void Attack()
	{
		// Mientras este en modo agresivo se movera hacia los enemigos
		if(Human.humanRef.aggressiveMode && GameManager.gameManager.robotsAlive > 0)
		{
			if(((Vector2)base.player.basicMovementServer.characterTransform.position - closestEnemyPosition).sqrMagnitude > 4)
			{
				// Si la posicion objetivo difiere de la anterior recalculamos el camino
				if((closestEnemyPosition - oldTargetPosition).sqrMagnitude > 2f)
				{
					base.CalculatePathTo(closestEnemyPosition);
					oldTargetPosition = closestEnemyPosition;
				}
			}
			else
			{
				// Si la posicion objetivo difiere de la anterior recalculamos el camino
				if((closestEnemyPosition - oldTargetPosition).sqrMagnitude > 0.5f)
				{
					base.CalculatePathTo(closestEnemyPosition);
					oldTargetPosition = closestEnemyPosition;
				}
			}
		}
		else
		{
			humanAIStatus = HumanAIStatus.Wander;
		}
	}

	void Wander()
	{
		if(!Human.humanRef.aggressiveMode)
		{
			// Comprobar enemigos cercanos
			if(!AIBaseController.humanInSight)
			{
				if(base.pathCompleted)
				{
					wlkToRandomPositionAround(player.basicMovementServer.characterTransform.position, 8);
				}
			}
			else
			{
				humanAIStatus = HumanAIStatus.Evade;
				base.ClearPath();
			}
		}
		else if(GameManager.gameManager.robotsAlive > 0)
		{
			humanAIStatus = HumanAIStatus.Attack;
		}
	}
}
