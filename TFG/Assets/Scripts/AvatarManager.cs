using UnityEngine;
using System.Collections.Generic;

public class AvatarManager : MonoBehaviour 
{
	public static AvatarManager avatarManager;

	public List<Sprite> listaTexturasAvatares = new List<Sprite>();

	void Awake()
	{
		avatarManager = this;
	}

	public Sprite getAvatar(EnumPersonaje enumPersonaje)
	{
		return listaTexturasAvatares[(int)(enumPersonaje)-1];
	}

	public string getPlayerClassName(EnumPersonaje enumPersonaje)
	{
		switch(enumPersonaje)
		{
		case EnumPersonaje.Humano:
			return "Humano";
			break;
			
		case EnumPersonaje.RobotAzul:
			return "Robot azul";
			break;
			
		case EnumPersonaje.RobotBlanco:
			return "Robot blanco";
			break;
			
		case EnumPersonaje.RobotMorado:
			return "Robot morado";
			break;
			
		case EnumPersonaje.RobotNaranja:
			return "Robot naranja";
			break;
			
		case EnumPersonaje.RobotRojo:
			return "Robot rojo";
			break;
			
		case EnumPersonaje.RobotRosa:
			return "Robot rosa";
			break;
			
		case EnumPersonaje.RobotVerde:
			return "Robot verde";
			break;
			
		default: return "";
		}
	}
}
