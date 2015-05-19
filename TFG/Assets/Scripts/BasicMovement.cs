using UnityEngine;
using System.Collections;

public enum FaceDirection{None, Right, Down, Left, Up}

public class BasicMovement : MonoBehaviour 
{
	public Transform graphicsTransform;
	FaceDirection faceDirection = FaceDirection.Right;
	public Vector3 inputDirection = Vector3.zero;
	private Vector3 oldInputDirection = Vector3.zero;
	Vector3 targetPos;
	public float speed = 3.5f;
	Vector3 vectorObjetivo;
	Transform characterTransform;

	void Awake()
	{
		targetPos = transform.position;
		characterTransform = transform;
	}
	
	void Update () 
	{
		// Cuando se este en un frame en el que se pueda cambiar de direccion ...
		if(puedeCambiarDireccion())
		{
			// Calculamos la posicion a donde quiere moverse el jugador
			if(inputDirection == oldInputDirection)
			{
				vectorObjetivo = characterTransform.position + inputDirection;
			}
			else
			{
				vectorObjetivo = redondearPosicion(characterTransform.position + inputDirection);
			}

			// Si la posicion esta libre la ponemos como objetivo
			if(Scenario.scenarioRef.arrayNivel[(int)vectorObjetivo.y, (int)vectorObjetivo.x] != 0)
			{
				targetPos = vectorObjetivo;
				if(inputDirection != oldInputDirection)
				{
					rotarModelo();
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

		characterTransform.position = Vector2.MoveTowards(characterTransform.position, targetPos, Time.deltaTime * speed);	
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

	void rotarModelo()
	{
		if(inputDirection == Vector3.right)
		{
			graphicsTransform.eulerAngles = Vector3.zero;
		}
		else if(inputDirection == Vector3.left)
		{
			graphicsTransform.eulerAngles = new Vector3(0, 0, 180);
		}
		else if(inputDirection == Vector3.up)
		{
			graphicsTransform.eulerAngles = new Vector3(0, 0, 90);
		}
		else if(inputDirection == Vector3.down)
		{
			graphicsTransform.eulerAngles = new Vector3(0, 0, -90);
		}
	}




	Vector3 redondearPosicion(Vector3 position)
	{
		return new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);
	}
}
