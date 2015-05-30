using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotRosa, RobotVerde, RobotBlanco, RobotMorado};

//[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	//	public BasicMovement basicMovementRef;
	public PlayerGraphics playerGraphics;
	public float speed = 3.25f;
	public Vector3 spawnPoint;

	private float floatPositionX;
	private float floatPositionY;

	private Vector3 posicionVieja = Vector3.zero;
	private Vector3 diferenciasPosiciones;

	public void Awake()
	{
		//		basicMovementRef = GetComponent<BasicMovement>();
		playerGraphics = GetComponent<PlayerGraphics>();
	}
	/*
	public void Start()
	{
		//Initialize();
	}
*/
	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
		transform.position = spawnPoint;
	}
	
	public virtual void Kill()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}
	
	public void SetSpawnPoint(Vector2 spawnPoint)
	{
		this.spawnPoint = spawnPoint;
		transform.position = spawnPoint;
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) 
	{
		// Sending
		if (stream.isReading) 
		{
			Debug.Log("ASDAS");
			stream.Serialize (ref floatPositionX);
			stream.Serialize (ref floatPositionY);
		
			transform.position = new Vector3(floatPositionX, floatPositionY, 0);

			diferenciasPosiciones = transform.position - posicionVieja;

			if(diferenciasPosiciones.x > 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 180);
			}
			else if(diferenciasPosiciones.x < 0)
			{
				transform.eulerAngles = Vector3.zero;
			}
			else if(diferenciasPosiciones.y > 0)
			{
				transform.eulerAngles = new Vector3(0, 0, -90);
			}
			else if(diferenciasPosiciones.y < 0)
			{
				transform.eulerAngles = new Vector3(0, 0, 90);
			}

			posicionVieja = transform.position;	
		} 
	}
}
