using UnityEngine;
using System.Collections;

public enum RobotAIStatus {Wander, Patrol, Attack, Search, Escape, Scatter}

public class RobotAI : AIBaseController 
{
	RobotAIStatus robotAIStatus = RobotAIStatus.Wander;
	RobotAIStatus statusWhenLastPosition = RobotAIStatus.Wander;
	private float patrolCounter;
	private Vector2 patrolPosition;
	private float escapeCounter;
	private Vector2 targetPosition;


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
		//Debug.Log(robotAIStatus);

		if(AIEnabled)
		{
			GlobalStatus();

			switch(robotAIStatus)
			{
				case RobotAIStatus.Wander:
					Wander();
				break;

				case RobotAIStatus.Patrol:
					Patrol();
				break;

				case RobotAIStatus.Attack:
					Attack();
				break;

				case RobotAIStatus.Search:
					Search();
				break;
					
				case RobotAIStatus.Escape:
					Escape();
				break;

				case RobotAIStatus.Scatter:
					Scatter();
				break;
				
			}
		}
	}

	void GlobalStatus()
	{
		if(AIBaseController.humanInSight && Human.humanRef.aggressiveMode)
		{
			base.ClearPath();
			robotAIStatus = RobotAIStatus.Escape;
		}
	}

	void Wander()
	{
		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
		else if(base.pathCompleted)
		{
			wlkToRandomPositionAround(player.basicMovementServer.characterTransform.position, 8);

			statusWhenLastPosition = robotAIStatus;
		}
	}

	void Patrol()
	{
		if(statusWhenLastPosition != RobotAIStatus.Patrol)
		{
			statusWhenLastPosition = RobotAIStatus.Patrol;

			patrolCounter = Time.time + 10;
		}
		else
		{
			if(Time.time < patrolCounter)
			{
				if(base.pathCompleted)
				{
					wlkToRandomPositionAround(patrolPosition, 3);
				}
			}
			else
			{
				robotAIStatus = RobotAIStatus.Wander;
			}
		}

		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
	}

	void Attack()
	{
		if(AIBaseController.humanInSight)
		{
			if((targetPosition - (Vector2)Human.humanRef.basicMovementServer.characterTransform.position).sqrMagnitude > 0.9f)
			{
				targetPosition = Human.humanRef.basicMovementServer.characterTransform.position;
				base.CalculatePathTo(targetPosition);

				statusWhenLastPosition = robotAIStatus;
			}
		}
		else
		{
			robotAIStatus = RobotAIStatus.Search;
			wlkToRandomPositionAround(AIBaseController.humanKnownPosition +
			                          (AIBaseController.humanKnownPosition - (Vector2)player.basicMovementServer.characterTransform.position).normalized * 6,
			                          3);

			statusWhenLastPosition = robotAIStatus;
		}
	}

	void Search()
	{
		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
		else if(base.pathCompleted)
		{

			robotAIStatus = RobotAIStatus.Scatter;
		}
	}

	void Escape()
	{
		if(base.pathCompleted)
		{
			wlkToRandomPositionAround((Vector2)player.basicMovementServer.characterTransform.position +
			                          ((Vector2)player.basicMovementServer.characterTransform.position - AIBaseController.humanKnownPosition).normalized * 4,
			                          	3);

			statusWhenLastPosition = robotAIStatus;
		}

		if(AIBaseController.humanInSight)
		{	
			if(Human.humanRef.aggressiveMode) 
			{
				escapeCounter = Time.time + 5;
			}
			else
			{
				robotAIStatus = RobotAIStatus.Attack;
			}
		}
		else if(Time.time > escapeCounter)
		{
			robotAIStatus = RobotAIStatus.Wander;
		}
	}

	void Scatter()
	{
		if(statusWhenLastPosition != RobotAIStatus.Scatter)
		{
			int contadorEsquina = 0;
	
			statusWhenLastPosition = RobotAIStatus.Scatter;

			for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length && i != base.player.id; i++)
			{
				if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano)
				{
					++contadorEsquina;
				}
			}

			switch(contadorEsquina)
			{
				case 0:
					wlkToRandomPositionAround(new Vector2(4,10), 0);
					break;
				
				case 1:
					wlkToRandomPositionAround(new Vector2(4,3), 0);
					break;

				case 2:
					wlkToRandomPositionAround(new Vector2(15,10), 0);
					break;
				
				case 3:
					wlkToRandomPositionAround(new Vector2(15,3), 0);
					break;
			}
		}
		else
		{
			if(base.pathCompleted)
			{
				robotAIStatus = RobotAIStatus.Wander;
			}
		}
	}
}
