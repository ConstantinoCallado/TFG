﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Scenario : MonoBehaviour 
{
	public static Scenario scenarioRef;
	public GameObject prefabMuro;
	int tamanyoMapa = 14;
	public byte[,] arrayNivel = new byte[14, 14] { 
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0},
		{0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0},
		{0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0},
		{0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
		{0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0},
		{0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0},
		{0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0},
		{0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0},
		{0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
		{0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0},
		{0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0},
		{0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
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
		for(int i=0; i<tamanyoMapa; i++)
		{
			for(int j=0; j<tamanyoMapa; j++)
			{
				if(arrayNivel[i,j] == 0)
				{
					GameObject cube = GameObject.Instantiate(prefabMuro);
					cube.transform.position = new Vector3(j, i, -1);
					cube.transform.localScale = new Vector3(1, 1, 2f);
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
