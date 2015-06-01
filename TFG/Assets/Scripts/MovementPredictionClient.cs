using UnityEngine;
using System.Collections;

public class MovementPredictionClient : BasicMovementClient 
{	
	private Vector2 posAseguradaAnterior;
	private Vector2 posPredicted;
	private Vector2 posAseguradaNueva;
	private Player refJugador;
	private bool initialized = false;
	private Transform refTransform;
	public void Awake()
	{
		base.Awake();
		refTransform = transform;
		refJugador = gameObject.GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () 
	{
		if(((Vector2)refTransform.position - posAseguradaNueva).sqrMagnitude < 2)
		{
			refTransform.position = Vector2.MoveTowards(refTransform.position, posPredicted, Time.deltaTime * refJugador.speed);
		}
		else
		{
			refTransform.position = posAseguradaNueva;
		}
	}

	public override void RecievedNewPosition(Vector3 positionRecieved)
	{
		if(initialized)
		{
			posAseguradaAnterior = posAseguradaNueva;
			posAseguradaNueva = positionRecieved;
			base.diferenciasPosiciones = posAseguradaNueva - posAseguradaAnterior;
			posPredicted = posAseguradaNueva + base.diferenciasPosiciones;


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
		else
		{
			// Al principio ponemos todas las posiciones a la recibida
			posAseguradaAnterior = posAseguradaNueva = posPredicted = positionRecieved;
			initialized = true;
		}
	}
}
