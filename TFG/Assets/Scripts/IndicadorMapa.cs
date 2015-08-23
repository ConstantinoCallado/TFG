using UnityEngine;
using System.Collections;

public enum EnumTipoInidcador{IndicadorIrA, IndicadorAyuda};

public class IndicadorMapa : MonoBehaviour 
{
	const float duracion = 4;

	public Renderer rendererAyuda;
	public Renderer rendererIrA;


	public void Inicializar(Vector2 pos, Color color, EnumTipoInidcador enumIndicador)
	{
		transform.position = new Vector3(pos.x, pos.y, 0);

		if(enumIndicador == EnumTipoInidcador.IndicadorIrA)
		{
			rendererIrA.gameObject.SetActive(true);
			rendererIrA.material.color = color;
		}
		else
		{
			rendererAyuda.gameObject.SetActive(true);
			rendererAyuda.material.color = color;
		}

		StartCoroutine(CorutinaDestruccion());
	}

	public IEnumerator CorutinaDestruccion()
	{
		yield return new WaitForSeconds(duracion);
		Destroy(gameObject);
	}
}
