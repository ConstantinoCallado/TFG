using UnityEngine;
using System.Collections.Generic;

public class SortedNodeList
{
	private List<PathfindingNode> laBuenaLista = new List<PathfindingNode>();
	bool insertCompleted = false;

	public void Clear()
	{
		laBuenaLista.Clear();
	}

	public void Add(PathfindingNode element)
	{
		insertCompleted = false;

		for(short i=0; i<laBuenaLista.Count && !insertCompleted; i++)
		{
			if(element.heuristicaTotal < laBuenaLista[i].heuristicaTotal)
			{
				laBuenaLista.Insert(i, element);
				insertCompleted = true;
			}
		}

		if(!insertCompleted)
		{
			laBuenaLista.Add(element);
		}
	}

	public short Count()
	{
		return (short)laBuenaLista.Count;
	}

	public PathfindingNode Peek()
	{
		return laBuenaLista[0];
	}

	public PathfindingNode First()
	{
		PathfindingNode nodoADevolver = laBuenaLista[0];

		laBuenaLista.RemoveAt(0);

		return nodoADevolver;
	}

	public void AddResultAtFirst(PathfindingNode element)
	{
		laBuenaLista.Insert(0, element);
	}

	public void Print ()
	{
		Debug.Log("TAMANYO LISTA " + Count());
		
		for(int i=0; i<Count(); i++)
		{
			Debug.Log("sacado " +laBuenaLista[i].distance);
		}
	}
}
