using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour 
{	
	private CircleCollider2D circleCollider;
	public float radius = 4;

	public static short cantidadViendo = 0;
	public static bool isHumanInSight = false;

	public void InitializeComun()
	{
		gameObject.transform.parent.GetComponent<Robot>().sightGameObject = this;
	}

	public void InitializeInClient()
	{
		InitializeComun();
		setRadius(radius);
	}

	public void InitializeInServer()
	{
		InitializeComun();
		gameObject.AddComponent<CircleCollider2D>();
		circleCollider = gameObject.GetComponent<CircleCollider2D>();
		setRadius(radius);
		circleCollider.isTrigger = true;
		gameObject.AddComponent<Rigidbody2D>();

		Rigidbody2D rigidInst = gameObject.GetComponent<Rigidbody2D>();
		rigidInst.isKinematic = true;
	}

	public void setRadius(float radiusParam)
	{
		radius = radiusParam;

		if(circleCollider)
		{
			circleCollider.radius = radius;
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("He avistado al humano");

		++cantidadViendo;
		checkCuantosViendo();
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log("Se ha perdido de vista al humano");

		--cantidadViendo;
		checkCuantosViendo();
	}

	void checkCuantosViendo()
	{
		isHumanInSight = cantidadViendo > 0;
	}
}

