using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BasicMovement))]
public class LocalInput : MonoBehaviour 
{
	Camera cameraRef;
	BasicMovement basicMovement;

	void Awake()
	{
		basicMovement = GetComponent<BasicMovement>();
		cameraRef = Camera.main;
	}

	// Update is called once per frame
	void Update () 
	{
		getUserInput();
	}

	void getUserInput()
	{
		#if UNITY_STANDALONE
		if(basicMovement.playerNumber == 1)
		{
			getEntradaTecladoWASD();
		}
		else
		{
			getEntradaTecladoFlechas();
		}
		#else
		//getEntradaBordeMovil();
		getEntradaRelativaMovil();
		#endif
	}
	
	void getEntradaTecladoWASD()
	{
		if(Input.GetKey(KeyCode.D))
		{
			basicMovement.inputDirection = Vector3.right;
		}
		else if(Input.GetKey(KeyCode.A))
		{
			basicMovement.inputDirection = Vector3.left;
		}
		else if(Input.GetKey(KeyCode.W))
		{
			basicMovement.inputDirection = Vector3.up;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			basicMovement.inputDirection = Vector3.down;
		}
	}

	void getEntradaTecladoFlechas()
	{
		if(Input.GetKey(KeyCode.RightArrow))
		{
			basicMovement.inputDirection = Vector3.right;
		}
		else if(Input.GetKey(KeyCode.LeftArrow))
		{
			basicMovement.inputDirection = Vector3.left;
		}
		else if(Input.GetKey(KeyCode.UpArrow))
		{
			basicMovement.inputDirection = Vector3.up;
		}
		else if(Input.GetKey(KeyCode.DownArrow))
		{
			basicMovement.inputDirection = Vector3.down;
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
				basicMovement.inputDirection = Vector3.right;
			}
			else if(toqueJugador.x < Screen.width * 0.3f)
			{
				basicMovement.inputDirection = Vector3.left;
			}
			else if(toqueJugador.y > Screen.height * 0.7f)
			{
				basicMovement.inputDirection = Vector3.up;
			}
			else if(toqueJugador.y < Screen.height * 0.3f)
			{
				basicMovement.inputDirection = Vector3.down;
			}
		}
	}
	
	// La direccion a moverse sera calculada teniendo en cuenta donde ha tocado el jugador respecto al personaje
	void getEntradaRelativaMovil()
	{
		if(Input.GetButton("Fire1"))
		{
			Vector2 toqueJugadorRelativo = (Vector2)(Input.mousePosition - cameraRef.WorldToScreenPoint(basicMovement.characterTransform.position));
			
			if(Mathf.Abs(toqueJugadorRelativo.x) > Mathf.Abs(toqueJugadorRelativo.y))
			{
				if(toqueJugadorRelativo.x > 0)
				{
					basicMovement.inputDirection = Vector3.right;
				}
				else
				{
					basicMovement.inputDirection = Vector3.left;
				}
			}
			else
			{
				if(toqueJugadorRelativo.y > 0)
				{
					basicMovement.inputDirection = Vector3.up;
				}
				else
				{
					basicMovement.inputDirection = Vector3.down;
				}
			}
		}
	}
}
