﻿using UnityEngine;
using System.Collections;

public class PlayerFactory : MonoBehaviour 
{
	public GameObject playerPrefab;
	public static PlayerFactory playerFactoryRef;


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
		jugadorInstanciado.Initialize();
		
		return jugadorInstanciado;
	}

	
	public Player InstanciarPlayerEnCliente(NetworkViewID viewID, int enumPersonajeInt)
	{
		Player jugadorInst = InstanciarPlayerComun(viewID, enumPersonajeInt);
		NetworkView netViewInstanciada = jugadorInst.gameObject.GetComponent<NetworkView>();
		jugadorInst.gameObject.AddComponent<MovementPredictionOtherClient>();
		netViewInstanciada.observed = jugadorInst.gameObject.GetComponent<MovementPredictionOtherClient>();

		return jugadorInst;
	}
	
	public Player InstanciarPlayerEnServidor(NetworkViewID viewID, int enumPersonajeInt)
	{
		Player jugadorInst = InstanciarPlayerComun(viewID, enumPersonajeInt);
		jugadorInst.gameObject.AddComponent<BasicMovementServer>();
				
		NetworkView netViewInstanciada = jugadorInst.gameObject.GetComponent<NetworkView>();
		netViewInstanciada.observed = jugadorInst.gameObject.GetComponent<BasicMovementServer>();

		if((EnumPersonaje)enumPersonajeInt == EnumPersonaje.Humano)
		{
			jugadorInst.SetSpawnPoint(Scenario.scenarioRef.getRandomHumanSpawnPoint());
		}
		else
		{
			jugadorInst.SetSpawnPoint(Scenario.scenarioRef.getRRobotSpawnPoint());
		}
		
		return jugadorInst; 
	}
}
