using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonIndicador : MonoBehaviour 
{
	public static bool activados = true;
	public bool inicializado = false;
	static float coolDownReset = 0;
	static short touchCount = 0;
	public Image imagen; 

	public static Color colorDeIconos = Color.magenta;

	void Update()
	{
		if(!inicializado)
		{
			if(activados)
			{
				if(colorDeIconos != Color.magenta)
				{
					imagen.color = colorDeIconos;
					inicializado = true;
				}
			}
			else
			{
				inicializado = true;
				gameObject.SetActive(false);
			}
		}
	}

	public void Click(int enumTipoIndicador)
	{
		if(Time.time > coolDownReset)
		{
			coolDownReset = Time.time + 10;
			touchCount = 0;
		}

		if(touchCount < 4)
		{
			LocalInput.localInputRef.ClickBotonIndicador((EnumTipoInidcador)enumTipoIndicador);
			++touchCount;
		}
	}
}
