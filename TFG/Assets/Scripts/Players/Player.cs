using UnityEngine;
using System.Collections;

public enum EnumPersonaje {Ninguno, Humano, RobotRojo, RobotNaranja, RobotAzul, RobotRosa, RobotVerde, RobotBlanco, RobotMorado};

[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	public BasicMovement basicMovementRef;
	public PlayerGraphics playerGraphics;

	public void Awake()
	{
		basicMovementRef = GetComponent<BasicMovement>();
		playerGraphics = GetComponent<PlayerGraphics>();
	}

	public void Start()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
	}

	public virtual void Kill()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}
}
