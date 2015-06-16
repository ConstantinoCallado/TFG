using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour 
{	
	public bool isEnabled = true;
	public float radius = 3;
	public Transform transformRef;
	public GameObject apertureFOW;

	public void Start()
	{
		WarFog.warFogRef.listaVisiones.Add(this);
	}
	
	public bool isPlayerInSight(Vector3 playerPosition)
	{
		if(isEnabled)
		{
			return ((playerPosition - transformRef.position).magnitude < (radius + 0.5f));
		}
		else
		{
			return false;
		}
	}

	public void SetSight(float radius)
	{
		this.radius = radius;
		apertureFOW.transform.localScale = new Vector3(radius * 2.4f, radius * 2.4f, 1);
	}

	public void EnableSight(bool param)
	{
		apertureFOW.SetActive(param);
		isEnabled = param;
	}

	public void OnDestroy()
	{
		Debug.Log("Destruyendo sight");

		WarFog.warFogRef.listaVisiones.Remove(this);
	}
}

