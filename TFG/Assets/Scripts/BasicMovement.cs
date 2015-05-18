using UnityEngine;
using System.Collections;

public enum FaceDirection{None, Right, Down, Left, Up}

public class BasicMovement : MonoBehaviour 
{
	public Transform graphicsTransform;
	FaceDirection faceDirection = FaceDirection.Right;
	private Vector3 inputDirection = Vector3.zero;
	private Vector3 oldInputDirection = Vector3.zero;
	Vector3 targetPos;
	float speed = 3.5f;
	bool isMoving = true;

	void Start()
	{
		targetPos = transform.position;
	}
	
	void Update () 
	{
		// Obtenemos la posible entrada del usuario
		getUserInput();

		// Cuando se este en un frame en el que se pueda cambiar de direccion ...
		if(puedeCambiarDireccion())
		{
			// Calculamos la posicion a donde quiere moverse el jugador
			Vector3 vectorObjetivo = redondearPosicion(transform.position + inputDirection);

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
				vectorObjetivo = redondearPosicion(transform.position + oldInputDirection);
			
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

		transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);	
	}

	// Se podra cambiar de direccion si se ha alcanzado la objetivo o si la direccion es la contraria a la actual
	public bool puedeCambiarDireccion()
	{
		if(transform.position == targetPos || inputDirection == -oldInputDirection)
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


	void getUserInput()
	{
		#if UNITY_STANDALONE
			getEntradaTeclado();
		#else
			getEntradaMovil();
		#endif
	}

	void getEntradaTeclado()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			isMoving = true;
			inputDirection = Vector3.up;
		}
		else if(Input.GetAxis("Vertical") < 0)
		{
			isMoving = true;
			inputDirection = Vector3.down;
		}
		else if(Input.GetAxis("Horizontal") > 0)
		{
			isMoving = true;
			inputDirection = Vector3.right;
		}
		else if(Input.GetAxis("Horizontal") < 0)
		{
			isMoving = true;
			inputDirection = Vector3.left;
		}
	}

	void getEntradaMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			Vector2 toqueJugador = (Vector2)Input.mousePosition;

			if(toqueJugador.x > Screen.width * 0.7f)
			{
				isMoving = true;
				inputDirection = Vector3.right;
			}
			else if(toqueJugador.x < Screen.width * 0.3f)
			{
				isMoving = true;
				inputDirection = Vector3.left;
			}
			else if(toqueJugador.y > Screen.height * 0.7f)
			{
				isMoving = true;
				inputDirection = Vector3.up;
			}
			else if(toqueJugador.y < Screen.height * 0.3f)
			{
				isMoving = true;
				inputDirection = Vector3.down;
			}

		}
	}

	Vector3 redondearPosicion(Vector3 position)
	{
		return new Vector3(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), 0);
	}
}
