using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotRosa, RobotVerde, RobotBlanco, RobotMorado};

//[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	//	public BasicMovement basicMovementRef;
	public PlayerGraphics playerGraphics;
	public float speed = 2.75f;
	public Vector3 spawnPoint;
	public bool isFreeze = false;
	public bool isDead = false;

	public void Awake()
	{
		//		basicMovementRef = GetComponent<BasicMovement>();
		playerGraphics = GetComponent<PlayerGraphics>();
	}

	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
		transform.position = spawnPoint;
	}
	
	public virtual void Kill()
	{
		isDead = true;
		isFreeze = true;
		playerGraphics.DisableGraphics();
	}
	
	public void SetSpawnPoint(Vector2 spawnPoint)
	{
		this.spawnPoint = spawnPoint;
	}

	public void Respawn()
	{
		isDead = false;
		isFreeze = false;
		transform.position = spawnPoint;
		playerGraphics.EnableGraphics();
		gameObject.GetComponent<BasicMovementServer>().targetPos = transform.position;
		gameObject.GetComponent<BasicMovementServer>().inputDirection = Vector3.zero;
	}
}
