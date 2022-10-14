// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC\u2019s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using UnityEngine.UI;

namespace Wave.Essence.Tracker.Model.Demo
{
	[RequireComponent(typeof(Text))]
	public class TrackerText : MonoBehaviour
	{
		public enum TextSelect
		{
			Connection,
			Role,
			Position,
			Rotation,
			Press,
			Touch,
			Battery,
			Extend,
		}

		readonly TrackerButton[] s_Buttons =
		{
			TrackerButton.System,
			TrackerButton.Menu,
			TrackerButton.A,
			TrackerButton.B,
			TrackerButton.X,
			TrackerButton.Y,
			TrackerButton.Trigger,
		};

		#region Inspector
		[SerializeField]
		private TrackerId m_Tracker = TrackerId.Tracker0;
		public TrackerId Tracker { get { return m_Tracker; } set { m_Tracker = value; } }

		public TextSelect TextFor = TextSelect.Connection;
		#endregion

		private Text m_Text = null;

		private void Awake()
		{
			m_Text = GetComponent<Text>();
		}

		private void Update()
		{
			if (m_Text == null || TrackerManager.Instance == null) { return; }

			m_Text.text = m_Tracker.ToString() + " ";

			switch (TextFor)
			{
				case TextSelect.Connection: TextConnection(); break;
				case TextSelect.Role: TextRole(); break;
				case TextSelect.Position: TextPosition(); break;
				case TextSelect.Rotation: TextRotation(); break;
				case TextSelect.Press: TextPress(); break;
				case TextSelect.Touch: TextTouch(); break;
				case TextSelect.Battery: TextBattery(); break;
				case TextSelect.Extend: TextExtend(); break;
				default:
					break;
			}
		}

		void TextConnection()
		{
			m_Text.text += "Connected: " + TrackerManager.Instance.IsTrackerConnected(m_Tracker);
		}
		void TextRole()
		{
			m_Text.text += "Role: " + TrackerManager.Instance.GetTrackerRole(m_Tracker);
		}
		void TextPosition()
		{
			Vector3 pos = TrackerManager.Instance.GetTrackerPosition(m_Tracker);
			m_Text.text += "X: " + pos.x + ", Y: " + pos.y + ", Z: " + pos.z;
		}
		void TextRotation()
		{
			Vector3 rot = TrackerManager.Instance.GetTrackerRotation(m_Tracker).eulerAngles;
			m_Text.text += "pitch: " + rot.x + ", yaw: " + rot.y + ", roll: " + rot.z;
		}
		void TextPress()
		{
			for (int i = 0; i < s_Buttons.Length; i++)
			{
				if (TrackerManager.Instance.TrackerButtonHold(m_Tracker, s_Buttons[i]))
				{
					m_Text.text += "Pressed: " + s_Buttons[i];
					return;
				}
			}
			m_Text.text += "N/A";
		}
		void TextTouch()
		{
			for (int i = 0; i < s_Buttons.Length; i++)
			{
				if (TrackerManager.Instance.TrackerButtonTouching(m_Tracker, s_Buttons[i]))
				{
					m_Text.text += "Touched: " + s_Buttons[i];
					m_Text.text += ", Type: " + TrackerManager.Instance.GetTrackerButtonAxisType(m_Tracker, s_Buttons[i]);
					Vector2 axis = TrackerManager.Instance.TrackerButtonAxis(m_Tracker, s_Buttons[i]);
					m_Text.text += ", Axis (" + axis.x + ", " + axis.y + ")";
					return;
				}
			}
			m_Text.text += "N/A";
		}
		void TextBattery()
		{
			m_Text.text += "Battery: " + TrackerManager.Instance.GetTrackerBatteryLife(m_Tracker) * 100 + "%";
		}
		void TextExtend()
		{
			var extData = TrackerManager.Instance.GetTrackerExtData(m_Tracker);
			string extStr = "";
			for (int i = 0; i < extData.Length; i++) { extStr += extData[i].ToString(); }
			m_Text.text += "Extend: " + extStr;
		}
	}
}
