using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour 
{
	public GameObject playerPrefab;
	public static PlayerFactory playerFactoryRef;


	public void Awake()
	{
		playerFactoryRef = this;
	}

	public Player InstanciarPlayerEnCliente(NetworkViewID viewID, int enumPersonajeInt)
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

		gameObjectInstanciado.GetComponent<NetworkView>().viewID = viewID;

		gameObjectInstanciado.GetComponent<NetworkView>().stateSynchronization = NetworkStateSynchronization.Unreliable;

		Player jugadorInstanciado = (Player)gameObjectInstanciado.GetComponent<Player>();
		jugadorInstanciado.Initialize();

		return jugadorInstanciado;
	}
	
	public Player InstanciarPlayerEnServidor(NetworkViewID viewID, int enumPersonajeInt)
	{
		Player jugadorInstanciado = InstanciarPlayerEnCliente(viewID, enumPersonajeInt);
		
		if((EnumPersonaje)enumPersonajeInt == EnumPersonaje.Humano)
		{
			jugadorInstanciado.SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());
		}
		else
		{
			jugadorInstanciado.SetSpawnPoint(Scenario.scenarioRef.getRRobotSpawnPoint());
		}
		
		return jugadorInstanciado; 
	}
}
