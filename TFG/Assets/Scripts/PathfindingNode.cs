using UnityEngine;
using System.Collections.Generic;

public class PathfindingNode
{
	public Vector2 position;
	public short distance;
	public short heuristicaParcial;
	public short heuristicaTotal;
	public PathfindingNode parent;
	
	private static PathfindingNode nodoExtraido;
	private static PathfindingNode nodoCreado;
	private static Vector2 posicionAExplorar;
	private static Vector2 targetPosition = Vector2.zero;
	
	public PathfindingNode(Vector2 position, short distance, PathfindingNode parent, Vector2 targetPosition)
	{
		this.position = position;
		this.distance = distance;
		this.parent = parent;
		PathfindingNode.targetPosition = targetPosition;
		calcuarDistanciaManhattan();
		actualizarPesos();
	}
	
	public PathfindingNode(Vector2 position, short distance, PathfindingNode parent)
	{
		this.position = position;
		this.distance = distance;
		this.parent = parent;
		calcuarDistanciaManhattan();
		actualizarPesos();
	}
	
	public bool ExpandAll(SortedNodeList listaNodosAExplorar, Dictionary<Vector2, PathfindingNode> diccionarioNodos)
	{
		if(!diccionarioNodos.ContainsKey(position))
		{
			diccionarioNodos.Add(position, this);

			return (ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x + 1, position.y)) ||
					ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x - 1, position.y)) ||
					ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x, position.y + 1)) ||
					ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x, position.y - 1)));
		}
		return false;
	}
	
	public bool ExpandEach(SortedNodeList listaNodosAExplorar, Dictionary<Vector2, PathfindingNode> diccionarioNodos, Vector2 posicionAExplorar)
	{
		nodoExtraido = null;

		// Primero nos aseguramos de que estemos en una posicion dentro del mapa
		if(posicionAExplorar.x >= 0 && posicionAExplorar.x < Scenario.tamanyoMapaX 
		   	&& posicionAExplorar.y >= 0 && posicionAExplorar.y < Scenario.tamanyoMapaY)
		{
			diccionarioNodos.TryGetValue(posicionAExplorar, out nodoExtraido);
			
			// Si es la primera vez que alcanzamos ese nodo...
			if(nodoExtraido == null)
			{
				// Si la posicion esta libre...
				if(Scenario.scenarioRef.arrayNivel[(int)posicionAExplorar.y, (int)posicionAExplorar.x] != 0)
				{
					// Y no es el objetivo la anyadimos a la lista para explorar
					if(posicionAExplorar != targetPosition)
					{
						nodoCreado = new PathfindingNode(posicionAExplorar, (short)(distance + 1), this);
						nodoCreado.actualizarPesos();
						listaNodosAExplorar.Add(nodoCreado);
					}
					// Si la posicion es la objetivo la encabezamos en la lista y detenemos la busqueda
					else
					{
						nodoCreado = new PathfindingNode(posicionAExplorar, (short)(distance + 1), this);
						listaNodosAExplorar.AddResultAtFirst(nodoCreado);
						return true;
					}
				}
			}
			// Si hemos llegado a un caso mejor... reordenamos los punteros
			else if(distance + 1 < nodoExtraido.distance)  
			{
				nodoExtraido.distance = (short)(distance + 1);
				nodoExtraido.parent = this;
				nodoExtraido.actualizarPesos();
			}
		}

		return false;
	}

	//TODO: Modificar heuristica para permitir que el pathfinding pueda usar los warps de los extremos del mapa
	private void calcuarDistanciaManhattan()
	{
		heuristicaParcial =  (short)(Mathf.Abs((int)position.x - (int)targetPosition.x) + Mathf.Abs((int)position.y - (int)targetPosition.y));
	}

	private void actualizarPesos()
	{
		heuristicaTotal = (short)(heuristicaParcial + distance);
	}
}