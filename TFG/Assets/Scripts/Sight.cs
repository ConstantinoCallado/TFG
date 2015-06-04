using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour 
{	
	public bool isEnabled = true;
	public float radius = 3;

	public Transform transformRef;
	
	public void Start()
	{
		transformRef = transform;
		WarFog.warFogRef.listaVisiones.Add(this);
	}
	
	public bool isPlayerInSight(Vector3 playerPosition)
	{
		if(isEnabled)
		{
			return ((playerPosition - transformRef.position).magnitude < radius);
		}
		else
		{
			return false;
		}
	}
}

