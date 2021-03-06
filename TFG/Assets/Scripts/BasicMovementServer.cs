﻿using UnityEngine;
using System.Collections;

public enum FaceDirection{None, Right, Down, Left, Up}

public class BasicMovementServer : MonoBehaviour 
{
	FaceDirection faceDirection = FaceDirection.Right;
	public Vector2 inputDirection = Vector2.zero;
	private Vector2 oldInputDirection = Vector2.zero;
	public Vector3 targetPos;
	public Player player;
	Vector2 vectorObjetivoTemp;
	public Transform characterTransform;

	void Awake()
	{
		player = gameObject.GetComponent<Player>();
		targetPos = transform.position;
		characterTransform = transform;
	}
	
	void Update () 
	{
		if(!player.isFreeze)
		{
			// Cuando se este en un frame en el que se pueda cambiar de direccion ...
			if(puedeCambiarDireccion() && inputDirection != Vector2.zero)
			{
				// Calculamos la posicion a donde quiere moverse el jugador

				vectorObjetivoTemp = redondearPosicion((Vector2)characterTransform.position + inputDirection);

				// Si la posicion esta libre la ponemos como objetivo
				if((int)vectorObjetivoTemp.x < 0 || (int)vectorObjetivoTemp.x > Scenario.tamanyoMapaX -2 || Scenario.scenarioRef.arrayNivel[(int)vectorObjetivoTemp.y, (int)vectorObjetivoTemp.x] != 0)
				{
					targetPos = vectorObjetivoTemp;

					if(inputDirection != oldInputDirection)
					{
						ActualizarRotacion(inputDirection);
						oldInputDirection = inputDirection;
					}
				}
				// Si no, intentamos mover el jugador siguiendo el movimiento anterior
				else
				{
					vectorObjetivoTemp = redondearPosicion((Vector2)characterTransform.position + oldInputDirection);
				
					// Si esta libre el movimiento anterior lo ponemos como objetivo
					if(Scenario.scenarioRef.arrayNivel[(int)vectorObjetivoTemp.y, (int)vectorObjetivoTemp.x] != 0)
					{					
						targetPos = vectorObjetivoTemp;
					}
					// Si esta ocupada detenemos el jugador
					else
					{
						targetPos = transform.position;
					}
				}
			}

			// Desplazamos el jugador
			characterTransform.position = Vector2.MoveTowards(characterTransform.position, targetPos, Time.deltaTime * player.speed);


			// Si ha salido por los margenes del mapa, lo reintroducimos por el otro lado
			if(characterTransform.position.x < 0.1f)
			{
				characterTransform.position = new Vector2(Scenario.tamanyoMapaX-2f , characterTransform.position.y); 
				targetPos = characterTransform.position;
			}
			else if(characterTransform.position.x > Scenario.tamanyoMapaX - 1.1f)
			{
				characterTransform.position = new Vector2(1f , characterTransform.position.y); 
				targetPos = characterTransform.position;
			}
		}
	}

	// Se podra cambiar de direccion si se ha alcanzado la objetivo o si la direccion es la contraria a la actual
	public bool puedeCambiarDireccion()
	{
		if(characterTransform.position == targetPos || inputDirection == -oldInputDirection)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	public static Vector2 redondearPosicion(Vector2 position)
	{
		return new Vector2(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
	}

	public static Vector2 redondearPosicionMasProxima(Vector2 position)
	{
		return new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
	}
	
	void ActualizarRotacion(Vector3 vectorInput)
	{
		if(vectorInput ==  Vector3.right)
		{
			transform.eulerAngles = new Vector3(0, 0, 180);
		}
		else if(vectorInput ==  Vector3.left)
		{
			transform.eulerAngles = Vector3.zero;
		}
		else if(vectorInput ==  Vector3.up)
		{
			transform.eulerAngles = new Vector3(0, 0, -90);
		}
		else if(vectorInput ==  Vector3.down)
		{
			transform.eulerAngles = new Vector3(0, 0, 90);
		}
	}

	void ActualizarInput(int enumMovimiento)
	{
		switch((EnumMovimiento) enumMovimiento)
		{
			case EnumMovimiento.Right:
			{
				//transform.eulerAngles = new Vector3(0, 0, 180);
				inputDirection = Vector3.right;
			}
			break;

			case EnumMovimiento.Left:
			{
				//transform.eulerAngles = Vector3.zero;
				inputDirection = Vector3.left;
			}
			break;

			case EnumMovimiento.Up:
			{
				//transform.eulerAngles = new Vector3(0, 0, -90);
				inputDirection = Vector3.up;
			}
			break;

			case EnumMovimiento.Down:
			{
				//transform.eulerAngles = new Vector3(0, 0, 90);
				inputDirection = Vector3.down;
			}
			break;

			case EnumMovimiento.None:
			{
				inputDirection = Vector3.zero;
			}
			break;
		}
	}
	

	// RPCS para inputs del jugador, se hacen 4 funciones sin parametros para ahorrar ancho de banda
	[RPC]
	public void r()
	{
		ActualizarInput((int)EnumMovimiento.Right);
	}
	
	[RPC]
	public void l()
	{
		ActualizarInput((int)EnumMovimiento.Left);
	}

	[RPC]
	public void u()
	{
		ActualizarInput((int)EnumMovimiento.Up);
	}

	[RPC]
	public void d()
	{
		ActualizarInput((int)EnumMovimiento.Down);
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) 
	{
		// Sending
		if (stream.isWriting) 
		{
			//Serialize2Float(stream, transform.position);
			Serialize1Int(stream, transform.position);
		} 
	}

	void Serialize2Float(BitStream stream, Vector3 position)
	{
		float floatPositionX = position.x;
		float floatPositionY = position.y;

		stream.Serialize (ref floatPositionX);
		stream.Serialize (ref floatPositionY);
	}

	void Serialize1Int(BitStream stream, Vector3 position)
	{
		// Trabajaremos con una precision de 2 decimales
		short packageX = (short)(position.x * 100);
		short packageY = (short)(position.y * 100);
	
		// Acumulamos la posicionX en los 16 bits de la izq, y la Y en los 16 de la derecha
		int positionPackage = packageY + (packageX << 16);

		stream.Serialize (ref positionPackage);
	}

	
	[RPC]
	public void skll()
	{
		player.skll();
	}
}