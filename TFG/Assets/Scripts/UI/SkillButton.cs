using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillButton : MonoBehaviour 
{
	public static SkillButton skillButtonRef;
	public Image imageCoolDown;
	public Button botonHabilidad;
	public float tiempoDeCD;
	public float tiempoFinalCD;


	public void Awake()
	{
		skillButtonRef = this;
	}

	public void Press()
	{
		LocalInput.localInputRef.ClickPower();
	}

	public void SetCoolDown(float time)
	{
		tiempoDeCD = time;
		tiempoFinalCD = Time.time + time;
		imageCoolDown.fillAmount = 1;
		botonHabilidad.interactable = false;
		StartCoroutine(corutinaReactivar());
	}

	public void ResetCoolDown()
	{
		tiempoFinalCD = 0;
	}

	public IEnumerator corutinaReactivar()
	{
		while(Time.time <= tiempoFinalCD)
		{
			imageCoolDown.fillAmount = (tiempoFinalCD - Time.time) / tiempoDeCD;
			yield return new WaitForEndOfFrame();
		}
	
		botonHabilidad.interactable = true;
		imageCoolDown.fillAmount = 0;
	}
}
