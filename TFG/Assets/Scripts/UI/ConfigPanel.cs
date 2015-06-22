using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfigPanel : MonoBehaviour 
{
	public InputField inputFieldName;

	void OnEnable()
	{
		inputFieldName.text = PlayerPrefs.GetString("PlayerName");
	}
}
