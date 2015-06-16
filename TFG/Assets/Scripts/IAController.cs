using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAController : MonoBehaviour 
{
	public Queue<Vector2> colaPosicionesObjetivo = new Queue<Vector2>();
	public BasicMovementServer basicMovementScript;

	public void Start()
	{
		StartCoroutine(corutinaIrASiguientePosicion());
	}


	public IEnumerator corutinaIrASiguientePosicion()
	{
		while(true)
		{
			if(basicMovementScript.targetPos != basicMovementScript.characterTransform.position)
			{


			}


			yield return new WaitForEndOfFrame();
		}
	}
}
