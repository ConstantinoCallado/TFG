using UnityEngine;
using System.Collections;

public class Ward : MonoBehaviour 
{
	public Sight sight;
	
	public void TurnOn(float radius, Color color, float time, Vector2 pos)
	{
		transform.position = pos;
		sight.SetSight(radius, color);
		Invoke("TurnOff", time);
	}
	
	public void TurnOff()
	{
		Destroy(gameObject);
	}
}
