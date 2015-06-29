using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sight : MonoBehaviour 
{	
	public bool isEnabled = true;
	public float radius = 3;
	public Transform transformRef;
	public GameObject apertureFOW;
	public MeshRenderer meshRing;
	public HashSet<Sightable> sightablesInRange = new HashSet<Sightable>();

	public CircleCollider2D sightCollider;


	public void LateUpdate()
	{
		if(WarFog.warfogEnabled)
		{
			meshRing.gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		Sightable sightableObject = other.GetComponent<Sightable>();
		sightableObject.sightInRange();
		sightablesInRange.Add(sightableObject);
	}
	
	public void OnTriggerExit2D(Collider2D other)
	{
		Sightable sightableObject = other.GetComponent<Sightable>();
		sightableObject.sightOutOfRange();
		sightablesInRange.Remove(sightableObject);
	}

	public void SetSight(float radius, Color color)
	{
		this.radius = radius;
		apertureFOW.transform.localScale = new Vector3(radius * 2.4f, radius * 2.4f, 1);
		meshRing.transform.localScale = new Vector3(radius, radius, 0);
		meshRing.material.color = color;

		sightCollider.radius = radius;

		EnableSight(true);
	}	                     

	public void EnableSight(bool param)
	{
		meshRing.gameObject.SetActive(param);

		apertureFOW.SetActive(param);
		isEnabled = param;
		sightCollider.enabled = param;

		// Si desactivamos el objeto de vision notificamos a todos los objetos que esten en rango que ya nos los vemos
		if(!param)
		{
			foreach (Sightable sightable in sightablesInRange)
			{
				if(sightable)
				{
					sightable.sightOutOfRange();
				}
			}

			sightablesInRange.Clear();
		}
	}

	public void OnDestroy()
	{
		foreach (Sightable sightable in sightablesInRange)
		{
			if(sightable)
			{
				sightable.sightOutOfRange();
			}
		}
		
		sightablesInRange.Clear();
	}
}