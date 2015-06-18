using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IAController : MonoBehaviour 
{
	private Dictionary<Vector2, PathfindingNode> diccionarioNodosExplorados = new Dictionary<Vector2, PathfindingNode>();
	private SortedNodeList listaNodosAExplorar = new SortedNodeList();

	private short indexObjetivo = 0;
	public List<Vector2> colaPosicionesObjetivo = new List<Vector2>();
	public Player player;

	public bool completed = false;


	public bool doTest = false;
	public Vector2 testPosition;



	private void Awake()
	{
		player = gameObject.GetComponent<Player>();
	}

	private void Start()
	{
		StartCoroutine(corutinaIrASiguientePosicion());
	}

	void Update()
	{
		if(doTest)
		{
			doTest = false;
			ClearPath();
			CalculatePathTo(testPosition);
		}
	}


	//TODO: Comprobar muros u obstaculos por el camino
	public IEnumerator corutinaIrASiguientePosicion()
	{
		while(true)
		{
			if(!player.isDead && !player.isFreeze && !completed)
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

	private bool CalculatePathTo(Vector2 targetPosition)
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
