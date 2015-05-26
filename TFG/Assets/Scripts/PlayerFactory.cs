using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour 
{
	public GameObject playerPrefab;
	
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
		
		Player jugadorInstanciado = (Player)gameObjectInstanciado.GetComponent<Player>();
		
		jugadorInstanciado.Initialize();
		
		//jugadorInstanciado.transform.parent = transform;
		
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
