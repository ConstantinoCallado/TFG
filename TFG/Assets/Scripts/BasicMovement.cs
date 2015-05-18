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
	float speed = 4f;
	bool isMoving = true;

	void Start()
	{
		targetPos = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		// Obtenemos la posible entrada del usuario
		getEntradaTeclado();

		// Cuando se este en un frame en el que se pueda cambiar de direccion ...
		if(puedeCambiarDireccion())
		{
			targetPos = redondearPosicion(transform.position + inputDirection);
			if(oldInputDirection != inputDirection)
			{
				rotarModelo();
			}

			oldInputDirection = inputDirection;
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

	Vector3 redondearPosicion(Vector3 position)
	{
		return new Vector3((int)position.x, (int)position.y, 0);
	}
}
