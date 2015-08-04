﻿using UnityEngine;
using System.Collections;

public enum EnumMovimiento{None, Up, Right, Down, Left};

public class LocalInput : MonoBehaviour 
{
	Camera cameraRef;
	public int enumMovimiento = (int)EnumMovimiento.None;
	public int oldEnumMovimiento = (int)EnumMovimiento.None;
	NetworkView networkView;
	BasicMovementServer movementRef;
	private string methodName = "";
	Player playerRef;
	public static LocalInput localInputRef;

	#if !UNITY_STANDALONE && !UNITY_STANDALONE_OSX 
	Vector2 firstTapPosition;
	Vector2 actualTapPosition;
	Vector2 difTapPosition;
	bool dragStarted = false;
	#endif

	void Awake()
	{
		networkView = GetComponent<NetworkView>();
		cameraRef = Camera.main;
		localInputRef = this;
		playerRef = gameObject.GetComponent<Player>();

		if(Network.isServer)
		{
			movementRef = gameObject.GetComponent<BasicMovementServer>();
		}
	}

	public void ClickPower()
	{
		if(Time.time >= playerRef.skillCoolDown && !playerRef.isDead)
		{
			playerRef.skll();
		
			SkillButton.skillButtonRef.SetCoolDown(playerRef.GetCoolDownTime());
		}
		else
		{
			Debug.Log("AUN NO HA PASADO EL COOLDOWN");
		}
	}

	// Actualizamos la entrada y la enviamos al servidor... si ya estamos en el propio servidor invocamos la funcion directamente
	void Update () 
	{
		getUserInput();

		if(oldEnumMovimiento != enumMovimiento)
		{
			switch(enumMovimiento)
			{
				case (int)EnumMovimiento.Right:
					methodName = "r";
					break;

				case (int)EnumMovimiento.Left:
					methodName = "l";
					break;

				case (int)EnumMovimiento.Up:
					methodName = "u";
					break;

				case (int)EnumMovimiento.Down:
					methodName = "d";
					break;
			}
			
			if(!movementRef)
			{
				networkView.RPC(methodName, RPCMode.Server);
			}
			else
			{
				movementRef.Invoke(methodName, 0);
			}

			oldEnumMovimiento = enumMovimiento;
		}
	}

	void getUserInput()
	{
		#if UNITY_STANDALONE || UNITY_STANDALONE_OSX 
			if(Input.GetAxis("Horizontal") > 0)
			{
				enumMovimiento = (int)EnumMovimiento.Right;
			}
			else if(Input.GetAxis("Horizontal") < 0)
			{
				enumMovimiento = (int)EnumMovimiento.Left;
			}
			else if(Input.GetAxis("Vertical") > 0)
			{
				enumMovimiento = (int)EnumMovimiento.Up;
			}
			else if(Input.GetAxis("Vertical") < 0)
			{
				enumMovimiento = (int)EnumMovimiento.Down;
			}

			if(Input.GetKeyDown(KeyCode.Space))
			{
				ClickPower();
			}
		#else
			if(Input.GetButton("Fire1"))
			{
				if(dragStarted)
				{
					actualTapPosition = Input.mousePosition;
					
					difTapPosition = actualTapPosition - firstTapPosition;
					
					if(difTapPosition.magnitude > Screen.height / 15)
					{
						if(Mathf.Abs(difTapPosition.x) > Mathf.Abs(difTapPosition.y))
						{
							if(difTapPosition.x > 0)
							{
								enumMovimiento = (int)EnumMovimiento.Right;
							}
							else
							{
								enumMovimiento = (int)EnumMovimiento.Left;
							}
						}
						else
						{
							if(difTapPosition.y > 0)
							{
								enumMovimiento = (int)EnumMovimiento.Up;
							}
							else
							{
								enumMovimiento = (int)EnumMovimiento.Down;
							}
						}
					}
				}
				else
				{
					firstTapPosition = Input.mousePosition;
					dragStarted = true;
				}
			}
			else
			{
				dragStarted = false;
			}
		#endif
	}

	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PAR ACTUALIZAR MOVIMIENTO
	[RPC]
	void r()
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}
	
	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PAR ACTUALIZAR MOVIMIENTO
	[RPC]
	void l()
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}
	
	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PAR ACTUALIZAR MOVIMIENTO
	[RPC]
	void u()
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}
	
	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PAR ACTUALIZAR MOVIMIENTO
	[RPC]
	void d()
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}

	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PARA ACTIVAR UNA SKILL
	[RPC]
	void skll()
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}
}
