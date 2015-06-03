using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EnumTile{Wall, Empty, Piece, Weapon};

public class Scenario : MonoBehaviour 
{
	public static Scenario scenarioRef;
	public GameObject prefabMuro;
	public GameObject prefabWeapon;
	public GameObject prefabPiece;
	public const int tamanyoMapaX = 20;
	public const int tamanyoMapaY = 14;

	public byte[,] arrayNivel = new byte[tamanyoMapaY, tamanyoMapaX] { 
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 3, 1, 2, 1, 1, 0, 2, 2, 2, 2, 0, 1, 1, 2, 1, 3, 0, 0},
		{0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0},
		{0, 0, 2, 1, 2, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 2, 1, 2, 0, 0},
		{0, 0, 1, 0, 1, 0, 1, 1, 2, 1, 1, 2, 1, 1, 0, 1, 0, 1, 0, 0},
		{0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0},
		{0, 0, 2, 0, 0, 0, 2, 1, 0, 1, 1, 0, 1, 2, 0, 0, 0, 2, 0, 0},
		{1, 2, 2, 0, 0, 0, 2, 1, 0, 1, 1, 0, 1, 2, 0, 0, 0, 2, 2, 1},
		{0, 0, 2, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 2, 0, 0},
		{0, 0, 1, 0, 1, 0, 1, 1, 2, 1, 1, 2, 1, 1, 0, 1, 0, 1, 0, 0},
		{0, 0, 2, 1, 2, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 2, 1, 2, 0, 0},
		{0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0},
		{0, 0, 3, 1, 2, 1, 1, 0, 2, 2, 2, 2, 0, 1, 1, 2, 1, 3, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
	};
	
	
	public List<Vector2> robotSpawnPoints = new List<Vector2>();
	public List<Vector2> playerSpawnPoints = new List<Vector2>();
	
	short contadorRobots = -1;
	
	public void Awake()
	{
		contadorRobots = -1;
		scenarioRef = this;
		
		crearMuros();
	}
	
	void crearMuros()
	{
		for(int i=0; i<tamanyoMapaY; i++)
		{
			for(int j=0; j<tamanyoMapaX; j++)
			{
				if(arrayNivel[i,j] != 1)
				{
					GameObject gameObjectInstanciado;

					if(arrayNivel[i,j] == 0)
					{
						gameObjectInstanciado = GameObject.Instantiate(prefabMuro);
					}
					else if(arrayNivel[i,j] == 2)
					{
						gameObjectInstanciado = GameObject.Instantiate(prefabPiece);
					}
					else
					{
						gameObjectInstanciado = GameObject.Instantiate(prefabWeapon);
					}

					gameObjectInstanciado.transform.position = new Vector3(j, i, -1);
					//gameObjectInstanciado.transform.localScale = new Vector3(1, 1, 2f);
					gameObjectInstanciado.transform.parent = transform;
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
}
