using UnityEngine;
using System.Collections;

public class BlueRobot : Robot
{
	Color colorRobot = new Color(0.16f, 0.58f, 1f);
	public int teleportDistance = 3;
	private Vector2 targetPosition;
	private bool continuar = true;

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot azul");
	}

	public override void ActivatePower()
	{
		Debug.Log("Activando poder azul");

		playerGraphics.robotGraphics.particulasFlash.Emit(10);

		continuar = true;

		transform.position = BasicMovementServer.redondearPosicion(transform.position);

		// Buscamos una posicion que este libre
		for(int i=teleportDistance; i>0 && continuar; i--)
		{
			targetPosition = (Vector2)transform.position + (Vector2)(-transform.right.normalized * i);
			Debug.Log("intentando teletransportarse " + targetPosition);

			if(Scenario.scenarioRef.isWalkable(targetPosition))
			{
				Debug.Log("teleport a " + targetPosition);
				transform.position = targetPosition;
				
				if(basicMovementServer)
				{
					basicMovementServer.targetPos = targetPosition;
				}

				continuar = false;
			}
		}
		playerGraphics.robotGraphics.particulasFlash.startSpeed *= -1;
		playerGraphics.robotGraphics.particulasFlash.Emit(10);
		playerGraphics.robotGraphics.particulasFlash.startSpeed *= -1;
	}

	public override Color GetColor()
	{
		return colorRobot;
	}

	public override string getColorString()
	{
		return "blue";
	}
}
