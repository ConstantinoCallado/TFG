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
		getEntradaTeclado();
		#else
		//getEntradaBordeMovil();
		getEntradaRelativaMovil();
		#endif
	}
	
	void getEntradaTeclado()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			basicMovement.inputDirection = Vector3.up;
		}
		else if(Input.GetAxis("Vertical") < 0)
		{
			basicMovement.inputDirection = Vector3.down;
		}
		else if(Input.GetAxis("Horizontal") > 0)
		{
			basicMovement.inputDirection = Vector3.right;
		}
		else if(Input.GetAxis("Horizontal") < 0)
		{
			basicMovement.inputDirection = Vector3.left;
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
			Vector2 toqueJugadorRelativo = (Vector2)(Input.mousePosition - cameraRef.WorldToScreenPoint(transform.position));
			
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
