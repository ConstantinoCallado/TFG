using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotRosa, RobotVerde, RobotBlanco, RobotMorado};

//[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	//	public BasicMovement basicMovementRef;
	public PlayerGraphics playerGraphics;
	public float speed = 3.5f;
	public Vector3 spawnPoint;
	public NetworkViewID viewID;
	
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
}
