using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AIBaseController : MonoBehaviour 
{
	private Dictionary<Vector2, PathfindingNode> diccionarioNodosExplorados = new Dictionary<Vector2, PathfindingNode>();
	private SortedNodeList listaNodosAExplorar = new SortedNodeList();

	private short indexObjetivo = 0;
	protected List<Vector2> colaPosicionesObjetivo = new List<Vector2>();
	protected Player player;

	protected bool completed = false;

	public bool AIEnabled = true;

	public static Vector2 humanKnownPosition;
	public static bool humanInSight;

	private void Awake()
	{
		player = gameObject.GetComponent<Player>();
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
			if(!player.isDead && !player.isFreeze && !completed && AIEnabled)
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
						completed = true;
					}
				}
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public void AddNewWaypoint(Vector2 siguiente)
	{
		completed = false;
		colaPosicionesObjetivo.Add(siguiente);
		++indexObjetivo;
	}

	public void ClearPath()
	{
		indexObjetivo = 0;
		completed = true;
		colaPosicionesObjetivo.Clear();
		listaNodosAExplorar.Clear();
		diccionarioNodosExplorados.Clear();
	}

	protected bool CalculatePathTo(Vector2 targetPosition)
	{
		ClearPath();
		completed = false;
	
		PathfindingNode nodoInicial = new PathfindingNode(redondearPosicion(player.basicMovementServer.characterTransform.position), 0, null, redondearPosicion(targetPosition));

		listaNodosAExplorar.Add(nodoInicial);

		while(listaNodosAExplorar.Count() > 0 && !completed)
		{
			completed = listaNodosAExplorar.First().ExpandAll(listaNodosAExplorar, diccionarioNodosExplorados);
		}

		if(completed)
		{
			PathfindingNode nodoFinal = listaNodosAExplorar.First();

			while(nodoFinal.parent != null)
			{
				AddNewWaypoint(nodoFinal.position);

				nodoFinal = nodoFinal.parent;
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	private Vector2 redondearPosicion(Vector2 pos)
	{
		return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
	}
}
