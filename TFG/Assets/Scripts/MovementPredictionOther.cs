using UnityEngine;
using System.Collections;

public class MovementPredictionOtherClient : BasicMovementClient 
{	
	private Vector2 posAseguradaAnterior;
	private Vector2 posPredicted;
	private Vector2 posAseguradaNueva;
	private Vector2 posComprobacionMuro;
	private bool initialized = false;
	private Transform refTransform;
	private float magnitudDistancia;
	float diferenciaConEntero;

	public void Awake()
	{
		base.Awake();
		refTransform = transform;
	}

	// Update is called once per frame
	void Update () 
	{
		if(!base.playerRef.isFreeze)
		{
			// Si el error de la prediccion esta dentro de un margen aceptable movemos el jugador
			if(((Vector2)refTransform.position - posAseguradaNueva).sqrMagnitude < 1f)
			{
				// Si no hay un muro donde predecimos el movimiento... movemos al jugador
				if(!hayMuroEnPrediccion())
				{
					refTransform.position = Vector2.MoveTowards(refTransform.position, posPredicted, Time.deltaTime * base.playerRef.speed);
				}
			}
			// Sino lo movemos directamente a mano
			else
			{
				refTransform.position = posAseguradaNueva;
				posPredicted = posAseguradaNueva;
			}
		}
	}

	public override void RecievedNewPosition(Vector3 positionRecieved)
	{
		if(initialized)
		{
			posAseguradaAnterior = posAseguradaNueva;
			posAseguradaNueva = positionRecieved;
			base.diferenciasPosiciones = posAseguradaNueva - posAseguradaAnterior;
			posPredicted = posAseguradaNueva + base.diferenciasPosiciones * 2;

			if(base.diferenciasPosiciones.sqrMagnitude < 5)
			{
				if(base.diferenciasPosiciones.x > 0)
				{
					transformRef.eulerAngles = new Vector3(0, 0, 180);
				}
				else if(base.diferenciasPosiciones.x < 0)
				{
					transformRef.eulerAngles = Vector3.zero;
				}
				else if(base.diferenciasPosiciones.y > 0)
				{
					transformRef.eulerAngles = new Vector3(0, 0, -90);
				}
				else if(base.diferenciasPosiciones.y < 0)
				{
					transformRef.eulerAngles = new Vector3(0, 0, 90);
				}
			}
		}
		else
		{
			// Al principio ponemos todas las posiciones a la recibida
			posAseguradaAnterior = posAseguradaNueva = posPredicted = positionRecieved;
			initialized = true;
		}
	}

	public bool hayMuroEnPrediccion()
	{
		// Calculamos la posicion donde nos desplazaremos
		posComprobacionMuro = (Vector2)refTransform.position + (((posPredicted - (Vector2)refTransform.position).normalized) / 2);

		return(Scenario.scenarioRef.arrayNivel[Mathf.RoundToInt(posComprobacionMuro.y), Mathf.RoundToInt(posComprobacionMuro.x)] == 0);
	}
}
