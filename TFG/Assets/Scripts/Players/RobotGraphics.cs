using UnityEngine;
using System.Collections;

public class RobotGraphics : MonoBehaviour 
{
	public Renderer[] renderersDeColores;
	public Animator animator;
	public Material materialTrail;
	public GameObject particulasMuerto;
	public ParticleSystem particulasFlash;
	public GameObject prefabBarrier;

	public void setColor(Color color)
	{
//		Debug.Log("seteando color " + color);
		for(int i=0; i< renderersDeColores.Length; i++)
		{
			renderersDeColores[i].material.color = color;
			particulasFlash.startColor = color;
		}
	}

	public void Kill()
	{
		animator.enabled = false;
		gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 0.1f);
		gameObject.transform.localPosition = new Vector3(0, 0, -0.5f);
		particulasMuerto.SetActive(true);
	}

	public void UnKill()
	{
		animator.enabled = true;
		gameObject.transform.localScale = Vector3.one;
		particulasMuerto.SetActive(false);
	}
}
