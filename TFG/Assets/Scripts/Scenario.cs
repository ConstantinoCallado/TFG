using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EnumTile{Wall, Empty, Piece, Weapon};

public class Scenario : MonoBehaviour 
{
	public static Scenario scenarioRef;
	//public GameObject prefabMuro;
	public GameObject prefabWeapon;
	public GameObject prefabPiece;
	public const int tamanyoMapaX = 40;
	public const int tamanyoMapaY = 24;
	public GameManager gameManager;

	public byte[,] arrayNivel = new byte[tamanyoMapaY, tamanyoMapaX] 
	{
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
		{0, 0, 3, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 2, 0, 0, 0, 0, 2, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 3, 0, 0}, 
		{0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 4, 0, 0, 1, 2, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 2, 1, 0, 0, 4, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 0, 0, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0}, 
		{0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
		{0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
		{0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0}, 
		{0, 0, 0, 1, 0, 0, 1, 1, 2, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 2, 1, 1, 0, 0, 1, 0, 0, 0}, 
		{0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 2, 0, 0, 1, 0, 0, 5, 1, 1, 5, 0, 0, 1, 0, 0, 2, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0}, 
		{1, 1, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 1, 1}, 
		{0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 5, 1, 1, 5, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 0, 0, 1, 1, 1, 1, 0, 0, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0}, 
		{0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0}, 
		{0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0}, 
		{0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 2, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0}, 
		{0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0}, 
		{0, 0, 3, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 0, 0, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 3, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
	};
	
	
	public List<Vector2> robotSpawnPoints = new List<Vector2>();
	public List<Vector2> playerSpawnPoints = new List<Vector2>();
	
	short contadorRobots = -1;

	public short partitionOfLeft;
	public short partitionOfRight;

	public void Awake()
	{
		partitionOfLeft = tamanyoMapaX/4;
		partitionOfRight = (tamanyoMapaX/4) * 3;

		contadorRobots = -1;
		scenarioRef = this;
		
		crearMuros();
	}
	
	void crearMuros()
	{
		GameObject gameObjectInstanciado;

		for(int i=0; i<tamanyoMapaY; i++)
		{
			for(int j=0; j<tamanyoMapaX; j++)
			{
				if(arrayNivel[i,j] != 0 && arrayNivel[i,j] != 1)
				{
					if(arrayNivel[i,j] == 2)
					{
						gameObjectInstanciado = GameObject.Instantiate(prefabPiece);
						++gameManager.piezasRestantes;
						gameObjectInstanciado.transform.position = new Vector3(j, i, 0);
						gameObjectInstanciado.transform.parent = transform;
					}
					else if(arrayNivel[i,j] == 3)
					{
						gameObjectInstanciado = GameObject.Instantiate(prefabWeapon);
						gameObjectInstanciado.transform.position = new Vector3(j, i, 0);
						gameObjectInstanciado.transform.parent = transform;
					}
				}
			}
		}
	}
	
	public Vector2 getRandomHumanSpawnPoint()
	{
		return playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Count)];
	}
	
	public Vector3 getRRobotSpawnPoint ()
	{
		++contadorRobots;
		
		if(contadorRobots < robotSpawnPoints.Count)
		{
			return robotSpawnPoints[contadorRobots];
		}
		else
		{
			return robotSpawnPoints[0];
		}
	}

	public bool isWalkable(Vector2 posicionAExplorar)
	{
		return(posicionAExplorar.x >= 0 && posicionAExplorar.x < Scenario.tamanyoMapaX 
		       && posicionAExplorar.y >= 0 && posicionAExplorar.y < Scenario.tamanyoMapaY
		       && arrayNivel[(int)posicionAExplorar.y, (int)posicionAExplorar.x] != 0);
	}

	public bool isWarpPosition(Vector2 position)
	{
		bool resultado = false;

		if((position.x == 0 && position.y == 12) || (position.x == tamanyoMapaX-1 && position.y == 12))
		{
			resultado = true;
		}

		return resultado;
	}
}
