using UnityEngine;
using System.Collections;

public enum RobotAIStatus {Wander, Defend, Attack, Search, Escape}

public class RobotAI : AIBaseController 
{
	RobotAIStatus robotAIStatus = RobotAIStatus.Wander;
	private float escapeCounter;
	private float searchCounter;
	private Vector2 targetPosition;

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
			}
		}
	}

	void GlobalStatus()
	{
		if(AIBaseController.humanInSight && Human.humanRef.aggressiveMode)
		{
			robotAIStatus = RobotAIStatus.Escape;
		}
	}

	void Wander()
	{
		// TODO: WANDER
		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
		else if(base.pathCompleted)
		{
			wlkToRandomPositionAround(player.basicMovementServer.characterTransform.position, 8);
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
		//TODO: Attack
		//base.ClearPath();

		if(AIBaseController.humanInSight)
		{
			if((targetPosition - (Vector2)Human.humanRef.basicMovementServer.characterTransform.position).sqrMagnitude > 2)
			{
				targetPosition = Human.humanRef.basicMovementServer.characterTransform.position;
				base.CalculatePathTo(targetPosition);
			}

		}
		else
		{
			robotAIStatus = RobotAIStatus.Search;

			searchCounter = Time.time + 5;
		}
	}

	void Search()
	{
		//TODO: BUSCAR

		if(AIBaseController.humanInSight)
		{
			robotAIStatus = RobotAIStatus.Attack;
		}
		else if(Time.time > searchCounter)
		{
			robotAIStatus = RobotAIStatus.Wander;
		}
	}

	void Escape()
	{
		//TODO: ESCAPAR

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
			robotAIStatus = RobotAIStatus.Search;
		}
	}

	void wlkToRandomPositionAround(Vector2 center, short radius)
	{
		Vector2 posicionADevolver;
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

		}while(!base.CalculatePathTo(posicionADevolver));
	}
}
