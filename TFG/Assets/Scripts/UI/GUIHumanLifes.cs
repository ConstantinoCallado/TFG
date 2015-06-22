using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIHumanLifes : MonoBehaviour 
{
	public GameObject humanLifePrefab;
	public GameObject canvasLifes;

	public List<GameObject> iconosHumano;

	public static GUIHumanLifes GUIHumanLifesRef;

	void Awake()
	{
		GUIHumanLifesRef = this;
	}

	// Use this for initialization
	void Start () 
	{
		for(int i=0; i < GameManager.gameManager.humanTries; i++)
		{
			AddLife();
		}
	}

	public void AddLife()
	{
		GameObject gameObjectInstanciado = GameObject.Instantiate(humanLifePrefab);
		gameObjectInstanciado.transform.parent = canvasLifes.transform;
		iconosHumano.Add(gameObjectInstanciado);
	}

	public void RemoveLife()
	{
		if(iconosHumano.Count > 0)
		{
			GameObject.Destroy(iconosHumano[iconosHumano.Count - 1]);
			iconosHumano.RemoveAt(iconosHumano.Count - 1);
		}
	}
}
