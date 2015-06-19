using UnityEngine;
using System.Collections;

public enum RobotAIStatus {Wander, Defend, Attack, Search, Escape}

public class RobotAI : AIBaseController 
{
	RobotAIStatus robotAIStatus = RobotAIStatus.Wander;
	private float escapeCounter;
	private float searchCounter;

	void Update()
	{
		Debug.Log(robotAIStatus);

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

		if(!AIBaseController.humanInSight)
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
}
