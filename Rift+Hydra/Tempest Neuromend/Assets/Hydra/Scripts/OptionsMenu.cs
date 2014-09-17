using UnityEngine;
using System;
using System.Collections;


public class OptionsMenu : MonoBehaviour
{
	private int m_selectedOption;
	private string[] m_options;

	private void Start()
	{
		m_options = new string[4];
		m_options [0] = "Asdfsdfsdfsdfsd";
		m_options [1] = "Bsdfsdfsdfsdf";
		m_options [2] = "Csdfdzxvxcvxcv";
	}

	private void DisplayHeading()
	{
		GUIStyle style = new GUIStyle ();
		style.fontSize = 25;
		style.alignment = TextAnchor.MiddleCenter;

		GUI.Box (new Rect (0.0f, 30.0f, Screen.width, 20.0f), "Settings", style);
	}

	private void DisplayChoices()
	{
		GUIStyle style = new GUIStyle ();
		style.fontSize = 15;
		style.alignment = TextAnchor.MiddleCenter;
		style.padding = new RectOffset (4, 4, 4, 4);
		style.onHover.textColor = Color.green;

		int items = m_options.Length;
		m_selectedOption = GUI.SelectionGrid (new Rect (10.0f, 100.0f,
		                                                 Screen.width, 25.0f * items),
		                                       m_selectedOption, m_options, 1, style);
	}

	private void OnGUI()
	{
		DisplayHeading ();
		DisplayChoices ();
	}
}
