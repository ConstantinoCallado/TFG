using UnityEngine;
using System.Collections;

public class BotonIndicador : MonoBehaviour 
{
	public static bool activados = true;
	static float coolDownReset = 0;
	static short touchCount = 0;

	void Update()
	{
		if(!activados)
		{
			gameObject.SetActive(false);
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
