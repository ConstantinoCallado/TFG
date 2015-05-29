using UnityEngine;
using System.Collections;

public enum FaceDirection{None, Right, Down, Left, Up}

public class BasicMovement : MonoBehaviour 
{
	FaceDirection faceDirection = FaceDirection.Right;
	public Vector3 inputDirection = Vector3.zero;
	private Vector3 oldInputDirection = Vector3.zero;
	Vector3 targetPos;
	public Player player;
	Vector3 vectorObjetivo;
	public Transform characterTransform;
	public int playerNumber = 1;

	private float floatPositionX;
	private float floatPositionY;

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
	
	void ActualizarInput(int enumMovimiento)
	{
		switch((EnumMovimiento) enumMovimiento)
		{
			case EnumMovimiento.Right:
			{
				inputDirection = Vector3.right;
			}
			break;

			case EnumMovimiento.Left:
			{
				inputDirection = Vector3.left;
			}
			break;

			case EnumMovimiento.Up:
			{
				inputDirection = Vector3.up;	
			}
			break;

			case EnumMovimiento.Down:
			{
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
			Debug.Log("SENDING");
			floatPositionX = characterTransform.position.x;
			floatPositionY = characterTransform.position.y;

			stream.Serialize (ref floatPositionX);
			stream.Serialize (ref floatPositionY);
		} 
	}
}