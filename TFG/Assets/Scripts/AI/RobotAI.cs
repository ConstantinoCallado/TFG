using UnityEngine;
using System.Collections;

public enum RobotAIStatus {Wander, Patrol, Attack, Search, Escape, Scatter}

public class RobotAI : AIBaseController 
{
	public RobotAIStatus robotAIStatus = RobotAIStatus.Wander;
	RobotAIStatus statusWhenLastPosition = RobotAIStatus.Wander;
	private float patrolCounter;
	private Vector2 patrolPosition;
	private float escapeCounter;
	public Vector2 targetPosition;
	private Vector2 selfPosition;

	void Start()
	{
		base.AIEnabled = false;
		base.Start ();

		StartCoroutine(startIADelayed());
	}

	public IEnumerator corutinaRandomSkill()
	{
		yield return new WaitForSeconds(1);
		while(true)
		{
			if(this.enabled && robotAIStatus != RobotAIStatus.Wander && Random.Range(0, 100) > 75)
			{
				player.skll();
			}
			
			yield return new WaitForSeconds(1.5f);
		}
	}

	public IEnumerator corutinaRandomSkillSpammer()
	{
		yield return new WaitForSeconds(1);
		while(true)
		{
			if(this.enabled && Random.Range(0, 100) > 85)
			{
				player.skll();
			}
			
			yield return new WaitForSeconds(1.5f);
		}
	}


	IEnumerator startIADelayed()
	{
		yield return new WaitForSeconds(Random.Range(0f, 3f));
		base.AIEnabled = true;

		if(NetworkManager.networkManagerRef.listaJugadores[base.player.id].enumPersonaje == EnumPersonaje.RobotNaranja ||
		   NetworkManager.networkManagerRef.listaJugadores[base.player.id].enumPersonaje == EnumPersonaje.RobotVerde)
		{
			StartCoroutine(corutinaRandomSkillSpammer());
		}
		else
		{
			StartCoroutine(corutinaRandomSkill());
		}
	}

	void LateUpdate()
	{
		if(Mathf.Abs(selfPosition.x - transform.position.x) > 3)
		{
			base.ClearPath();
			targetPosition = new Vector2(999,999);
		}
	}
	
	void Update()
	{
		if(AIEnabled)
		{
			selfPosition = transform.position;

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

			PostAI();
		}
	}

	void PostAI()
	{
		if(robotAIStatus != RobotAIStatus.Attack)
		{
			targetPosition = new Vector2(999,999);
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
			if((targetPosition - (Vector2)Human.humanRef.transform.position).magnitude > 0.75f)
			{
				//Debug.Log("Recalculando posicion");
				targetPosition = BasicMovementServer.redondearPosicion(Human.humanRef.transform.position);

				if(!base.CalculatePathTo(targetPosition))
				{
					targetPosition = new Vector2(999,999);
				}

				statusWhenLastPosition = robotAIStatus;
			}
		}
		else
		{
			robotAIStatus = RobotAIStatus.Search;
			//base.CalculatePathTo(AIBaseController.humanKnownPosition);
			wlkToRandomPositionAround(AIBaseController.humanKnownPosition, 8);

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
		//TODO: Recalcula el camino con demasiada frecuencia, subir el umbral (o algo asi)
		if(base.pathCompleted)
		{
			if(((Vector2)player.basicMovementServer.characterTransform.position - (Vector2)Human.humanRef.transform.position).magnitude < 18)
			{
				wlkToRandomPositionAround((Vector2)player.basicMovementServer.characterTransform.position +
				                          ((Vector2)player.basicMovementServer.characterTransform.position - AIBaseController.humanKnownPosition).normalized * 6,
				                          	4);
			}
			else
			{
				wlkToRandomPositionAround((Vector2)player.basicMovementServer.characterTransform.position, 4);
			}

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
