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

			//Debug.Log("Expandiendo nodo " + position);

			// Si estamos en una posicion normal anyadimos los 4 vecinos al conjunto de nodos a explorar
			if(!Scenario.scenarioRef.isWarpPosition(position))
			{
				return (ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x + 1, position.y)) ||
				        ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x - 1, position.y)) ||
				        ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x, position.y + 1)) ||
				        ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(position.x, position.y - 1)));
			}
			// Si nos encontramos en un posible nodo de portal (los 2 de los extremos) recalculamos el nodo resultante por el otro extremo
			else
			{
				if(position.x == 0)
				{
					return (ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(Scenario.tamanyoMapaX-1, position.y)) ||
					       	ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(1, position.y)));
				}
				else
				{
					return (ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(0, position.y)) ||
					        ExpandEach(listaNodosAExplorar, diccionarioNodos, new Vector2(Scenario.tamanyoMapaX-2, position.y)));				
				}
			}
		}
		return false;
	}

	public bool ExpandEach(SortedNodeList listaNodosAExplorar, Dictionary<Vector2, PathfindingNode> diccionarioNodos, Vector2 posicionAExplorar)
	{
		nodoExtraido = null;
		
		// Primero nos aseguramos de que estemos en una posicion dentro del mapa
		if(Scenario.scenarioRef.isWalkable(posicionAExplorar))
		{
			diccionarioNodos.TryGetValue(posicionAExplorar, out nodoExtraido);
			
			// Si es la primera vez que alcanzamos ese nodo...
			if(nodoExtraido == null)
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
	
	private void calcuarDistanciaManhattan()
	{	
		heuristicaParcial = (short)Mathf.Abs((int)position.y - (int)targetPosition.y);

		// Si estamos muy a la izquierda del mapa trucamos la heuristica para permitir pasar por el portal de la izquierda
		if(position.x < Scenario.scenarioRef.partitionOfLeft && targetPosition.x > Scenario.scenarioRef.partitionOfRight)
		{
			heuristicaParcial += (short)(position.x + (Scenario.tamanyoMapaX - targetPosition.x));
		}
		// Si estamos muy a la derecha del mapa trucamos la heuristica para permitir pasar por el portal de la derecha
		else if(position.x > Scenario.scenarioRef.partitionOfRight && targetPosition.x < Scenario.scenarioRef.partitionOfLeft)
		{
			heuristicaParcial += (short)(targetPosition.x + (Scenario.tamanyoMapaX - position.x));
		}
		else
		{
			heuristicaParcial += (short)(Mathf.Abs((int)position.x - (int)targetPosition.x));
		}
	}


	private void actualizarPesos()
	{
		heuristicaTotal = (short)(heuristicaParcial + distance);
	}
}