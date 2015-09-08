using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public static AudioManager audioManagerRef;

	public AudioSource audioTuerca;
	public AudioSource audioPocion;
	public AudioSource audioMuerteHumano;
	public AudioSource audioMuerteRobot;
	public AudioSource audioVictoria;
	public AudioSource audioDerrota;

	public void Awake()
	{
		audioManagerRef = this;
	}

	public void PlayTuerca()
	{
		audioTuerca.Play();
	}

	public void PlayPocion()
	{
		audioPocion.Play();
	}

	public void PlayMuerteHumano()
	{
		audioMuerteHumano.Play();
	}

	public void PlayMuerteRobot()
	{
		audioMuerteRobot.Play();
	}

	public void PlayVictoria()
	{
		audioVictoria.Play();
	}

	public void PlayDerrota()
	{
		audioDerrota.Play();
	}
}
