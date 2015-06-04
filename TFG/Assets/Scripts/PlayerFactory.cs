using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour 
{
	public static PlayerFactory playerFactoryRef;

	public GameObject playerPrefab;
	public GameObject sightPrefab;



	public void Awake()
	{
		playerFactoryRef = this;
	}

	public Player InstanciarPlayerComun(NetworkViewID viewID, int enumPersonajeInt)
	{
		GameObject gameObjectInstanciado = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); 
		
		switch((EnumPersonaje)enumPersonajeInt)
		{
			case EnumPersonaje.Humano:
				gameObjectInstanciado.AddComponent<Human>();
				break;
				
			case EnumPersonaje.RobotRojo:
				gameObjectInstanciado.AddComponent<RedRobot>();
				break;
				
			case EnumPersonaje.RobotNaranja:
				gameObjectInstanciado.AddComponent<OrangeRobot>();
				break;
				
			case EnumPersonaje.RobotAzul:
				gameObjectInstanciado.AddComponent<BlueRobot>();
				break;
				
			case EnumPersonaje.RobotRosa:
				gameObjectInstanciado.AddComponent<PinkRobot>();
				break;
				
			case EnumPersonaje.RobotVerde:
				gameObjectInstanciado.AddComponent<GreenRobot>();
				break;
				
			case EnumPersonaje.RobotBlanco:
				gameObjectInstanciado.AddComponent<WhiteRobot>();
				break;
				
			case EnumPersonaje.RobotMorado:
				gameObjectInstanciado.AddComponent<PurpleRobot>();
				break;
		}
		
		gameObjectInstanciado.AddComponent<NetworkView>();
		
		NetworkView netViewInstanciada = gameObjectInstanciado.GetComponent<NetworkView>();

		netViewInstanciada.viewID = viewID;

		netViewInstanciada.stateSynchronization = NetworkStateSynchronization.Unreliable;

		//netViewInstanciada.observed = null;
	
		//gameObjectInstanciado.AddComponent<LocalInput>();
		
		Player jugadorInstanciado = (Player)gameObjectInstanciado.GetComponent<Player>();

		
		return jugadorInstanciado;
	}

	
	public Player InstanciarPlayerEnCliente(NetworkViewID viewID, int enumPersonajeInt)
	{
		Player jugadorInst = InstanciarPlayerComun(viewID, enumPersonajeInt);
		NetworkView netViewInstanciada = jugadorInst.gameObject.GetComponent<NetworkView>();
		jugadorInst.gameObject.AddComponent<MovementPredictionOtherClient>();
		netViewInstanciada.observed = jugadorInst.gameObject.GetComponent<MovementPredictionOtherClient>();

		if((EnumPersonaje)enumPersonajeInt == EnumPersonaje.Humano)
		{		
			jugadorInst.gameObject.AddComponent<CircleCollider2D>();
			jugadorInst.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
			jugadorInst.gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;
			
			jugadorInst.gameObject.AddComponent<Rigidbody2D>();
			Rigidbody2D rigidbodyInstanciado = jugadorInst.gameObject.GetComponent<Rigidbody2D>();
			rigidbodyInstanciado.fixedAngle = true;

			rigidbodyInstanciado.gravityScale = 0;
		}
		else
		{	
			// Le anyadimos un objeto de vision
			Sight visionInstanciada = (Sight)GameObject.Instantiate(sightPrefab).GetComponent<Sight>();
			visionInstanciada.transform.parent = jugadorInst.transform;
			visionInstanciada.transform.localPosition = Vector3.zero;
			visionInstanciada.InitializeInClient();
		}


		jugadorInst.Initialize();

		return jugadorInst;
	}
	
	public Player InstanciarPlayerEnServidor(NetworkViewID viewID, int enumPersonajeInt)
	{
		Player jugadorInst = InstanciarPlayerComun(viewID, enumPersonajeInt);
		jugadorInst.gameObject.AddComponent<BasicMovementServer>();
				
		NetworkView netViewInstanciada = jugadorInst.gameObject.GetComponent<NetworkView>();
		netViewInstanciada.observed = jugadorInst.gameObject.GetComponent<BasicMovementServer>();

		jugadorInst.gameObject.AddComponent<CircleCollider2D>();
		jugadorInst.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
		jugadorInst.gameObject.GetComponent<CircleCollider2D>().radius = 0.3f;

		jugadorInst.gameObject.AddComponent<Rigidbody2D>();
		Rigidbody2D rigidbodyInstanciado = jugadorInst.gameObject.GetComponent<Rigidbody2D>();
		rigidbodyInstanciado.fixedAngle = true;

		if((EnumPersonaje)enumPersonajeInt == EnumPersonaje.Humano)
		{		
			rigidbodyInstanciado.gravityScale = 0;
			jugadorInst.SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());
		}
		else
		{
			rigidbodyInstanciado.isKinematic = true;
			jugadorInst.SetSpawnPoint(Scenario.scenarioRef.getRRobotSpawnPoint());

			// Le anyadimos un objeto de vision
			Sight visionInstanciada = (Sight)GameObject.Instantiate(sightPrefab).GetComponent<Sight>();
			visionInstanciada.transform.parent = jugadorInst.transform;
			visionInstanciada.transform.localPosition = Vector3.zero;
			visionInstanciada.InitializeInServer();
		}

		jugadorInst.Initialize();

		jugadorInst.Respawn();

		return jugadorInst; 
	}
}
