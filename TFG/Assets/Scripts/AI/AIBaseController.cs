using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIBaseController : MonoBehaviour 
{
	private Dictionary<Vector2, PathfindingNode> diccionarioNodosExplorados = new Dictionary<Vector2, PathfindingNode>();
	private SortedNodeList listaNodosAExplorar = new SortedNodeList();

	private short indexObjetivo = 0;
	public List<Vector2> colaPosicionesObjetivo = new List<Vector2>();
	protected Player player;

	public bool pathCompleted = true;
	public bool pathCalculated = false;

	private Vector2 movementVector;

	public bool AIEnabled = false;


	public static Vector2 humanKnownPositionPrev;
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

						movementVector = colaPosicionesObjetivo[indexObjetivo] - (Vector2)player.basicMovementServer.characterTransform.position;

						// Si la posicion a desplazarse esta cerca la tomamos como buena
						if(Mathf.Abs(movementVector.x) < 2)
						{
							player.basicMovementServer.inputDirection = movementVector.normalized;
						}
						// Si esta muy lejos asumimos que esta en la otra punta del mapa... por lo que damos la vuelta
						else
						{
							player.basicMovementServer.inputDirection = -movementVector.normalized;
						}

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


	private IEnumerator testPathfinding(Vector2 targetPosition)
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
				GameObject gameObjectDebug = (GameObject)Instantiate(Resources.Load("Prefabs/Debug_Node"));
				gameObjectDebug.GetComponent<DebugIANode>().textoHeu.text = "h " + listaNodosAExplorar.Peek().heuristicaParcial.ToString();
				gameObjectDebug.GetComponent<DebugIANode>().textoPeso.text = "p " + listaNodosAExplorar.Peek().distance.ToString();
				gameObjectDebug.transform.position = listaNodosAExplorar.Peek().position;

				pathCalculated = listaNodosAExplorar.First().ExpandAll(listaNodosAExplorar, diccionarioNodosExplorados);

				yield return new WaitForEndOfFrame();
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
			}
		}
	}


	protected bool CalculatePathTo(Vector2 targetPosition)
	{
		ClearPath();
		pathCalculated = false;
		
		if(targetPosition.x < 0)
		{
			targetPosition.x += Scenario.tamanyoMapaX;
		}
		else if(targetPosition.x > Scenario.tamanyoMapaX -1)
		{
			targetPosition.x -=Scenario.tamanyoMapaX;
		}
		
		if(targetPosition.y < 1)
		{
			targetPosition.y = 1;
		}
		else if(targetPosition.y > Scenario.tamanyoMapaY -2)
		{
			targetPosition.y = Scenario.tamanyoMapaY -2;
		}

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
				pathCompleted = false;
				PathfindingNode nodoFinal = listaNodosAExplorar.First();

				while(nodoFinal.parent != null)
				{
					AddNewWaypoint(nodoFinal.position);

					nodoFinal = nodoFinal.parent;
				}

				//Debug.Log(transform.gameObject.name + " ha calculado camino hasta " + targetPosition);
				return true;
			}
		}


		//Debug.Log(transform.gameObject.name + " ha fallado el camino hasta " + targetPosition);
		return false;
	}

	private Vector2 redondearPosicion(Vector2 pos)
	{
		return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
	}

	protected Vector2 wlkToRandomPositionAround(Vector2 center, short radius)
	{
		Vector2 posicionADevolver;
		int contador = 0;
		do
		{
			posicionADevolver = new Vector2((int)(center.x + Random.Range(-radius, radius)), (int)(center.y + Random.Range(-radius, radius)));

			++contador;

		}while(!CalculatePathTo(posicionADevolver) && contador < 10);
		
		return posicionADevolver;
	}
}
