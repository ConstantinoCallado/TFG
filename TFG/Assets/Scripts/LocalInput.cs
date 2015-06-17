using UnityEngine;
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

		if(Network.isServer)
		{
			movementRef = gameObject.GetComponent<BasicMovementServer>();
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
		getEntradaTecladoEjes();
		#else
		//getEntradaRelativaMovil();
		getEntradaDragMovil();
		#endif
	}

	void getEntradaTecladoEjes()
	{
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
	}

	// La direccion a moverse sera calculada teniendo en cuenta donde ha tocado el jugador respecto al personaje
	void getEntradaRelativaMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			actualTapPosition = (Vector2)(Input.mousePosition - cameraRef.WorldToScreenPoint(transform.position));
			
			if(Mathf.Abs(actualTapPosition.x) > Mathf.Abs(actualTapPosition.y))
			{
				if(actualTapPosition.x > 0)
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
				if(actualTapPosition.y > 0)
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


	// La direccion a moverse sera calculada teniendo en cuenta el gesto de tocar y arrastrar
	void getEntradaDragMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			if(dragStarted)
			{
				actualTapPosition = Input.mousePosition;

				difTapPosition = actualTapPosition - firstTapPosition;

				if(difTapPosition.magnitude > Screen.height / 10)
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
}
