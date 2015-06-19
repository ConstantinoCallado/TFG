using UnityEngine;
using System.Collections;

public enum RobotAIStatus {Wander, Defend, Attack, Search, Escape, Scatter}

public class RobotAI : AIBaseController 
{
	RobotAIStatus robotAIStatus = RobotAIStatus.Wander;
	RobotAIStatus statusWhenLastPosition = RobotAIStatus.Wander;
	private float escapeCounter;
	private Vector2 targetPosition;

	//TODO: Empezar IA con retraso

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

				case RobotAIStatus.Defend:
					Defend();
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

	void Defend()
	{
		//TODO: Patrullar una posicion

		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
	}

	void Attack()
	{
		if(AIBaseController.humanInSight)
		{
			if((targetPosition - (Vector2)Human.humanRef.basicMovementServer.characterTransform.position).sqrMagnitude > 2)
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

	//TODO: Testear esta mierda
	void Scatter()
	{
		int contadorEsquina = 0;
		if(statusWhenLastPosition != RobotAIStatus.Scatter)
		{
			statusWhenLastPosition = RobotAIStatus.Scatter;

			for(int i=0; i < NetworkManager.networkManagerRef.listaJugadores.Length && i != base.player.id; i++)
			{
				if(NetworkManager.networkManagerRef.listaJugadores[i].enumPersonaje != EnumPersonaje.Humano)
				{
					++contadorEsquina;
				}
			}


			Debug.Log("VOy a la esquina " + contadorEsquina);

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

	void wlkToRandomPositionAround(Vector2 center, short radius)
	{
		Vector2 posicionADevolver;
		int contador = 0;
		do
		{
			posicionADevolver = new Vector2((int)(center.x + Random.Range(-radius, radius)), (int)(center.y + Random.Range(-radius, radius)));

			if(posicionADevolver.x < 0)
			{
				posicionADevolver.x = Scenario.tamanyoMapaX + posicionADevolver.x;
			}
			else if(posicionADevolver.x > Scenario.tamanyoMapaX)
			{
				posicionADevolver.x = posicionADevolver.x - Scenario.tamanyoMapaX;
			}

			++contador;
		}while(!base.CalculatePathTo(posicionADevolver) && contador < 5);
	}
}
