﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIBaseController : MonoBehaviour 
{
	private Dictionary<Vector2, PathfindingNode> diccionarioNodosExplorados = new Dictionary<Vector2, PathfindingNode>();
	private SortedNodeList listaNodosAExplorar = new SortedNodeList();

	private short indexObjetivo = 0;
	protected List<Vector2> colaPosicionesObjetivo = new List<Vector2>();
	protected Player player;

	protected bool pathCompleted = true;
	protected bool pathCalculated = false;

	public bool AIEnabled = true;

	public static Vector2 humanKnownPosition;
	public static bool humanInSight;

	private void Awake()
	{
		player = gameObject.GetComponent<Player>();
		player.AIBase = this;
	}

	protected void Start()
	{
		StartCoroutine(corutinaIrASiguientePosicion());
	}

	//TODO: Comprobar muros u obstaculos por el camino
	public IEnumerator corutinaIrASiguientePosicion()
	{
		while(true)
		{
			if(!player.isDead && !player.isFreeze && !pathCompleted && AIEnabled)
			{
				if(player.basicMovementServer.targetPos == player.basicMovementServer.characterTransform.position)
				{
					if(indexObjetivo > 0 && colaPosicionesObjetivo.Count > 0)
					{
						--indexObjetivo;

						player.basicMovementServer.inputDirection = (colaPosicionesObjetivo[indexObjetivo] 
						                                             - (Vector2)player.basicMovementServer.characterTransform.position).normalized;

						//colaPosicionesObjetivo.RemoveAt(colaPosicionesObjetivo.Count-1);
					}
					else
					{
						player.basicMovementServer.inputDirection = Vector2.zero;
						pathCompleted = true;
					}
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public void AddNewWaypoint(Vector2 siguiente)
	{
		pathCompleted = false;
		colaPosicionesObjetivo.Add(siguiente);
		++indexObjetivo;
	}

	public void ClearPath()
	{
		indexObjetivo = 0;
		pathCompleted = true;
		colaPosicionesObjetivo.Clear();
		listaNodosAExplorar.Clear();
		diccionarioNodosExplorados.Clear();
	}

	protected bool CalculatePathTo(Vector2 targetPosition)
	{
		ClearPath();
		pathCalculated = false;

		PathfindingNode nodoInicial = new PathfindingNode(redondearPosicion(player.basicMovementServer.characterTransform.position), 0, null, redondearPosicion(targetPosition));

		// Si la posicion a ir esta vacia...
		if(Scenario.scenarioRef.isWalkable(targetPosition))
		{
			// Insertamos el primer nodo en la lista a explorar
			listaNodosAExplorar.Add(nodoInicial);

			// Expandimos nodos hasta que no queden mas... o hayamos encntrado el final
			while(listaNodosAExplorar.Count() > 0 && !pathCalculated)
			{
				pathCalculated = listaNodosAExplorar.First().ExpandAll(listaNodosAExplorar, diccionarioNodosExplorados);
			}

			// Si se ha llegado al final reconstruimos y almacenamos el camino
			if(pathCalculated)
			{
				PathfindingNode nodoFinal = listaNodosAExplorar.First();

				while(nodoFinal.parent != null)
				{
					AddNewWaypoint(nodoFinal.position);

					nodoFinal = nodoFinal.parent;
				}

				return true;
			}
		}

		return false;
	}

	private Vector2 redondearPosicion(Vector2 pos)
	{
		return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
	}
}
