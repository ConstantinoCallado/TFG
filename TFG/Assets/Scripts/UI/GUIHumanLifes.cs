using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIHumanLifes : MonoBehaviour 
{
	public GameObject humanLifePrefab;
	public GameObject canvasLifes;

	public List<GameObject> iconosHumano;

	public static GUIHumanLifes GUIHumanLifesRef;

	public short vidasInstanciadas;

	void Awake()
	{
		GUIHumanLifesRef = this;
	}

	// Use this for initialization
	void Start () 
	{
		for(int i=0; i < NetworkManager.networkManagerRef.humanLifes; i++)
		{
			AddLife();
			++vidasInstanciadas;
		}

		StartCoroutine(corutinaActualizarVidas());
	}

	IEnumerator corutinaActualizarVidas()
	{
		while(true)
		{
			if(vidasInstanciadas > NetworkManager.networkManagerRef.humanLifes)
			{
				RemoveLife();
				--vidasInstanciadas;
			}
			else if(vidasInstanciadas < NetworkManager.networkManagerRef.humanLifes)
			{
				AddLife();
				++vidasInstanciadas;
			}
			else
			{
				yield return new WaitForSeconds(0.4f);
			}
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
		if(vidasInstanciadas > 0)
		{
			GameObject.Destroy(iconosHumano[iconosHumano.Count - 1]);
			iconosHumano.RemoveAt(iconosHumano.Count - 1);
		}
	}
}
