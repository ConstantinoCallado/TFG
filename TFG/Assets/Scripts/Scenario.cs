using UnityEngine;
using System.Collections;

public class Scenario : MonoBehaviour 
{
	public static Scenario scenarioRef;
	public GameObject prefabMuro;
	int tamanyoMapa = 14;
	public byte[,] arrayNivel = new byte[14, 14] { 
													{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
													{0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0},
													{0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0},
													{0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0},
													{0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
													{0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0},
													{0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0},
													{0, 0, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0},
													{0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0},
													{0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
													{0, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0},
													{0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 0},
													{0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0},
													{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
												};


	
	public void Awake()
	{
		scenarioRef = this;
	
		crearMuros();
	}

	void crearMuros()
	{
		for(int i=0; i<tamanyoMapa; i++)
		{
			for(int j=0; j<tamanyoMapa; j++)
			{
				if(arrayNivel[i,j] == 0)
				{
					GameObject cube = GameObject.Instantiate(prefabMuro);
					cube.transform.position = new Vector3(j, i, -1);
					cube.transform.localScale = new Vector3(1, 1, 2f);
				}
			}
		}
	}
}
