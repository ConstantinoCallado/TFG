using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotVerde, RobotBlanco, RobotMorado};

public class Player : MonoBehaviour 
{
	public PlayerGraphics playerGraphics;
	public float speed = 3f;
	public Vector3 spawnPoint;
	public bool isFreeze = false;
	public bool isDead = false;
	public int id;
	public LocalInput localInput;
	public NetworkView networkView;
	public CircleCollider2D colliderJugador;
	public BasicMovementServer basicMovementServer;
	public AIBaseController AIBase;

	public float skillCoolDown;

	public void Awake()
	{
		playerGraphics = GetComponent<PlayerGraphics>();
	}

	public void Update()
	{
		if(Network.isServer)
		{
			if(AIBase)
			{
				AIBase.enabled = !NetworkManager.networkManagerRef.listaJugadores[id].activePlayer;
			}
		}
	}

	public virtual void Initialize()
	{
		//Debug.Log("inicializado Player");
		transform.position = spawnPoint;
		gameObject.GetComponent<HoveringName>().playerRef = this;
		networkView = GetComponent<NetworkView>();
		colliderJugador = GetComponent<CircleCollider2D>();
		basicMovementServer = GetComponent<BasicMovementServer>();
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

		playerGraphics.Unkill();

		if(colliderJugador)
		{
			colliderJugador.enabled = true;
		}
	}

	public virtual float GetCoolDownTime()
	{
		return 20;
	}

	public virtual void ActivatePower()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}

	public void EnableIA (bool b)
	{
		throw new System.NotImplementedException ();
	}
	
	public void skll()
	{
		if(Time.time >= skillCoolDown && !isDead)
		{
			skillCoolDown = Time.time + GetCoolDownTime();
			// Activamos la habilidad en el servidor
			ActivatePower();
			
			// La "broadcasteamos" a todos los clientes
			NetworkManager.networkManagerRef.BroadcastSkill(id);
		}
		else
		{
			//Debug.Log("AUN NO HA PASADO EL COOLDOWN");
		}
	}

	public void ponerIndicadorEn(Vector2 posicion, EnumTipoInidcador tipoIndicador)
	{
		GameObject indicadorInstanciado = (GameObject)GameObject.Instantiate(playerGraphics.prefabIndicador);
		indicadorInstanciado.GetComponent<IndicadorMapa>().Inicializar(posicion, GetColor(), tipoIndicador);
	}

	public virtual Color GetColor()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
		return Color.green;
	}
}
