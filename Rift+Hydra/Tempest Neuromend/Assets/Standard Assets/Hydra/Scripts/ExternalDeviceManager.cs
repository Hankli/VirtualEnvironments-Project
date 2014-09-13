using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Tempest
{
	public class ExternalDeviceManager : MonoBehaviour
	{
		private float m_nativeWidth;
		private float m_nativeHeight;

		private int m_selectedDevice;
		private int m_lastKnownTotalDevices;
		private string[] m_connectedDevices;

		private void Start()
		{
			m_connectedDevices = Input.GetJoystickNames ();
			m_lastKnownTotalDevices = m_connectedDevices.Length;

			m_nativeWidth = Screen.width;
			m_nativeHeight = Screen.height;
		}

		private void Update()
		{
			if(m_lastKnownTotalDevices != m_connectedDevices.Length)
			{
				m_connectedDevices = Input.GetJoystickNames();
				m_selectedDevice = 0;
				m_lastKnownTotalDevices = m_connectedDevices.Length;
			}
		}

		private void RenderNavigationButtons()
		{
			float x = 30.0f;
			float y = 25.0f;
			float width = 100.0f;
			float height = 30.0f;

			GUIStyle style = new GUIStyle (GUI.skin.button);
			style.normal.textColor = Color.black;
			style.fontSize = 20;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;

			if(GUI.Button (new Rect (x,y,width,height), "Back", style))
			{
			}
		}

		private void RenderWindow()
		{		
			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;
			style.fontSize = 25;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			
			GUI.Box (new Rect (0f, 0f, Screen.width, Screen.height * 0.1f), "Device Settings", style);
		}

		private void RenderConnectedDevices()
		{
			float x = 30.0f;
			float y = 100.0f;
			float width = 100.0f;
			float height = 80.0f;

			GUIStyle style = new GUIStyle ();
			style.normal.textColor = Color.black;
			style.fontSize = 15;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			
			GUI.Label (new Rect(x,y,width,height), "Devices Detected", style);
			
		    x = 30.0f;
			y = 180.0f;
			width = 100.0f;
			height = m_connectedDevices.Length * 25.0f;

			m_selectedDevice = GUI.SelectionGrid (new Rect (x, y, width, height), m_selectedDevice, m_connectedDevices, 1);
		}

		private void RenderSelectedDeviceInfo()
		{
			//instructions, textual info, tweakable numbers ??

		}

		private void OnGUI()
		{
			float wd = Screen.width / m_nativeWidth;
			float ht = Screen.height / m_nativeHeight;

			GUI.matrix = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.identity, new Vector3 (wd, ht, 1.0f));

			RenderWindow ();
			RenderNavigationButtons ();
			RenderConnectedDevices ();
		}

	}
}
