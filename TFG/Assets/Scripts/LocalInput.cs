using UnityEngine;
using System.Collections;

public enum EnumMovimiento{None, Up, Right, Down, Left};

public class LocalInput : MonoBehaviour 
{
	Camera cameraRef;
	int enumMovimiento = (int)EnumMovimiento.None;
	int oldEnumMovimiento = (int)EnumMovimiento.None;
	NetworkView networkView;
	BasicMovement movementRef;

	void Awake()
	{
		networkView = GetComponent<NetworkView>();
		cameraRef = Camera.main;

		if(Network.isServer)
		{
			movementRef = gameObject.GetComponent<BasicMovement>();
		}
	}

	// Actualizamos la entrada y la enviamos al servidor... si ya estamos en el propio servidor invocamos la funcion directamente
	void Update () 
	{
		getUserInput();

		if(oldEnumMovimiento != enumMovimiento)
		{
			if(!movementRef)
			{
				networkView.RPC("inpt", RPCMode.Server, enumMovimiento);
			}
			else
			{
				movementRef.inpt(enumMovimiento);
			}

			oldEnumMovimiento = enumMovimiento;
		}
	}

	void getUserInput()
	{
		#if UNITY_STANDALONE
		getEntradaTecladoWASD();
		#else
		//getEntradaBordeMovil();
		getEntradaRelativaMovil();
		#endif
	}
	
	void getEntradaTecladoWASD()
	{
		if(Input.GetKey(KeyCode.D))
		{
			enumMovimiento = (int)EnumMovimiento.Right;
		}
		else if(Input.GetKey(KeyCode.A))
		{
			enumMovimiento =(int)EnumMovimiento.Left;
		}
		else if(Input.GetKey(KeyCode.W))
		{
			enumMovimiento = (int)EnumMovimiento.Up;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			enumMovimiento = (int)EnumMovimiento.Down;
		}
	}

	void getEntradaTecladoFlechas()
	{
		if(Input.GetKey(KeyCode.RightArrow))
		{
			enumMovimiento = (int)EnumMovimiento.Right;
		}
		else if(Input.GetKey(KeyCode.LeftArrow))
		{
			enumMovimiento = (int)EnumMovimiento.Left;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			enumMovimiento = (int)EnumMovimiento.Up;
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			enumMovimiento = (int)EnumMovimiento.Down;
		}
	}

	
	// La direccion a moverse sera calculada teniendo en cuenta en que borde de la pantalla toque el jugador
	void getEntradaBordeMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			Vector2 toqueJugador = (Vector2)Input.mousePosition;
			
			if(toqueJugador.x > Screen.width * 0.7f)
			{
				enumMovimiento = (int)EnumMovimiento.Right;
			}
			else if(toqueJugador.x < Screen.width * 0.3f)
			{
				enumMovimiento = (int)EnumMovimiento.Left;
			}
			else if(toqueJugador.y > Screen.height * 0.7f)
			{
				enumMovimiento = (int)EnumMovimiento.Up;
			}
			else if(toqueJugador.y < Screen.height * 0.3f)
			{
				enumMovimiento = (int)EnumMovimiento.Down;
			}
		}
	}
	
	// La direccion a moverse sera calculada teniendo en cuenta donde ha tocado el jugador respecto al personaje
	void getEntradaRelativaMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			Vector2 toqueJugadorRelativo = (Vector2)(Input.mousePosition - cameraRef.WorldToScreenPoint(transform.position));
			
			if(Mathf.Abs(toqueJugadorRelativo.x) > Mathf.Abs(toqueJugadorRelativo.y))
			{
				if(toqueJugadorRelativo.x > 0)
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
				if(toqueJugadorRelativo.y > 0)
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


	// PLANTILLA DE FUNCION QUE SE ENVIARA AL SERVIDOR PAR ACTUALIZAR MOVIMIENTO
	[RPC]
	void inpt(int enumMovimiento)
	{
		Debug.Log("LA FUNCION SE IMPLEMENTA EN EL SERVIDOR");
	}
}
