using UnityEngine;
using System.Collections;

public class MovementPredictionClient : BasicMovementClient 
{	
	private Vector3 posAnterior;
	private Vector3 velPredicted;
	private Vector3 posNueva;
	private Player refJugador;
	private Vector3 posPredicted;
	private bool initialized = false;

	public void Awake()
	{
		base.Awake();

		refJugador = gameObject.GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () 
	{
		// Si la diferencia entre posiciones es pequeña procedemos a mover el jugador segun la prediccion
		if((base.transformRef.position - posNueva).sqrMagnitude < 2 && initialized)
		{
			posPredicted = base.transformRef.position + velPredicted;

			if(Scenario.scenarioRef.arrayNivel[(int)(posPredicted.y + 0.5f), (int)(posPredicted.x + 0.5f)] != 0)
			//if(true)
			{
				base.transformRef.position = Vector2.MoveTowards(base.transformRef.position, 
				                                                 base.transformRef.position + velPredicted, Time.deltaTime * refJugador.speed);
			}
			else
			{
				Debug.Log("COLISION ENFRENTE");
			}
		}
		// Sino lo movemos directamente sin prediccion
		else
		{
			Debug.Log("Posicion demasiado lejana, movemos el jugador sin prediccion");
			base.transformRef.position = posNueva;
		}
	}

	public override void RecievedNewPosition(Vector3 positionRecieved)
	{
		if(initialized)
		{
			posAnterior = posNueva;
			posNueva = positionRecieved;
			base.transformRef.position = positionRecieved;
			velPredicted = (posNueva - posAnterior).normalized;
		}
		else
		{
			posAnterior = posNueva = positionRecieved;
		}

		initialized = true;
	}
}
