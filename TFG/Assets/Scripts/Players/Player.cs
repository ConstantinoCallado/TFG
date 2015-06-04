using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotRosa, RobotVerde, RobotBlanco, RobotMorado};

public class Player : MonoBehaviour 
{
	public PlayerGraphics playerGraphics;
	public float speed = 2.75f;
	public Vector3 spawnPoint;
	public bool isFreeze = false;
	public bool isDead = false;
	public int id;
	public LocalInput localInput;
	public NetworkView networkView;
	public CircleCollider2D colliderJugador;

	public void Awake()
	{
		playerGraphics = GetComponent<PlayerGraphics>();
	}

	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
		transform.position = spawnPoint;
		gameObject.GetComponent<HoveringName>().playerRef = this;
		networkView = GetComponent<NetworkView>();
		colliderJugador = GetComponent<CircleCollider2D>();
	}
	
	public virtual void Kill()
	{
		isDead = true;
		isFreeze = true;
		playerGraphics.Kill();

		if(colliderJugador)
		{
			colliderJugador.enabled = false;
		}
	}
	
	public void SetSpawnPoint(Vector2 spawnPoint)
	{
		this.spawnPoint = spawnPoint;
	}

	// Mueve el jugador a la posicion de spawn, resucitandolo si murio
	public virtual void Respawn()
	{
		if(Network.isServer)
		{
			transform.position = spawnPoint;

			gameObject.GetComponent<BasicMovementServer>().targetPos = transform.position;
			gameObject.GetComponent<BasicMovementServer>().inputDirection = Vector3.zero;
		}

		// Ponemos su direccion anterior a 0 para que cualquier nueva direccion se pueda enviar
		if(localInput != null)
		{
			localInput.oldEnumMovimiento = 0;
			localInput.enumMovimiento = 0;
		}
		
		isDead = false;
		isFreeze = false;

		playerGraphics.EnableGraphics(true);

		if(colliderJugador)
		{
			colliderJugador.enabled = true;
		}
	}

	public virtual void ActivatePower()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}
}
