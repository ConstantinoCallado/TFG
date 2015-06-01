using UnityEngine;
using System.Collections;

public enum FaceDirection{None, Right, Down, Left, Up}

public class BasicMovementServer : MonoBehaviour 
{
	FaceDirection faceDirection = FaceDirection.Right;
	public Vector3 inputDirection = Vector3.zero;
	private Vector3 oldInputDirection = Vector3.zero;
	public Vector3 targetPos;
	public Player player;
	Vector3 vectorObjetivo;
	public Transform characterTransform;

	void Awake()
	{
		player = gameObject.GetComponent<Player>();
		targetPos = transform.position;
		characterTransform = transform;
	}
	
	void Update () 
	{
		// Cuando se este en un frame en el que se pueda cambiar de direccion ...
		if(puedeCambiarDireccion())
		{
			// Calculamos la posicion a donde quiere moverse el jugador

			vectorObjetivo = redondearPosicion(characterTransform.position + inputDirection);

			// Si la posicion esta libre la ponemos como objetivo
			if(Scenario.scenarioRef.arrayNivel[(int)vectorObjetivo.y, (int)vectorObjetivo.x] != 0)
			{
				targetPos = vectorObjetivo;
				if(inputDirection != oldInputDirection)
				{
					ActualizarRotacion(inputDirection);
					oldInputDirection = inputDirection;
				}
			}
			// Si no, intentamos mover el jugador siguiendo el movimiento anterior
			else
			{
				vectorObjetivo = redondearPosicion(characterTransform.position + oldInputDirection);
			
				// Si esta libre el movimiento anterior lo ponemos como objetivo
				if(Scenario.scenarioRef.arrayNivel[(int)vectorObjetivo.y, (int)vectorObjetivo.x] != 0)
				{					
					targetPos = vectorObjetivo;
				}
				// Si esta ocupada detenemos el jugador
				else
				{
					targetPos = transform.position;
				}
			}
		}

		characterTransform.position = Vector2.MoveTowards(characterTransform.position, targetPos, Time.deltaTime * player.speed);	
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


	Vector3 redondearPosicion(Vector3 position)
	{
		return new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);
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

	[RPC]
	public void inpt(int enumMovimiento)
	{
		ActualizarInput(enumMovimiento);
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
}