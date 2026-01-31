using System;
using UnityEngine;

namespace UnknownMod
{
	public class CalculateOrbitalVelocity : MonoBehaviour
	{
		private bool isMenuOn;
		private string HeightA;
		private string HeightP;
		private bool calculate;
		private float gm;
		private float radius;
		private string diffy;
		private float apo;
		private float peri;
		private float sma;
		private double A;
		private double P;
		private int selected;

		public void OnGUI()
		{
			GUI.Window(GUIUtility.GetControlID(FocusType.Passive), new Rect(1700f, 100f, 200f, 100f), new GUI.WindowFunction(MainWindow), "Main");
			if (isMenuOn)
			{
				GUI.Window(GUIUtility.GetControlID(FocusType.Passive), new Rect(1700f, 200f, 200f, 450f), new GUI.WindowFunction(CalcWin), "Calculations");
			}
		}
		public void MainWindow(int id)
		{
			GUI.Label(new Rect(10f, 30f, 180f, 180f), "Main Menu");
			isMenuOn = GUI.Toggle(new Rect(30f, 50f, 100f, 100f), isMenuOn, " Calculations");
			GUI.DragWindow();
		}

		public void CalcWin(int id)
		{
			string[] array = new string[] { "Normal", "Hard", "Realistic" };
			GUI.Label(new Rect(40f, 20f, 150f, 25f), "Select Game Difficulty");
			GUI.Label(new Rect(0f, 35f, 200f, 25f), "---------------------------------------------------------------------------");
			selected = GUI.Toolbar(new Rect(10f, 50f, 180f, 25f), selected, array);
			GUI.Label(new Rect(0f, 75f, 200f, 25f), "---------------------------------------------------------------------------");
			GUI.Label(new Rect(50f, 95f, 150f, 25f), "Enter The Values");
			GUI.Label(new Rect(0f, 105f, 200f, 25f), "---------------------------------------------------------------------------");
			GUI.Label(new Rect(10f, 125f, 200f, 25f), "Apoapsis:");
			HeightA = GUI.TextField(new Rect(10f, 150f, 180f, 25f), HeightA);
			GUI.Label(new Rect(10f, 175f, 200f, 25f), "Periapsis:");
			HeightP = GUI.TextField(new Rect(10f, 200f, 180f, 25f), HeightP);
			GUI.Label(new Rect(0f, 225f, 200f, 25f), "---------------------------------------------------------------------------");
			calculate = GUI.Toggle(new Rect(50f, 250f, 100f, 25f), calculate, "Calculate!", "button");
			GUI.Label(new Rect(0f, 275f, 200f, 25f), "---------------------------------------------------------------------------");
			GUI.Label(new Rect(10f, 410f, 200f, 25f), "Made by Mr. Unknown");
			GUI.Label(new Rect(0f, 400f, 200f, 25f), "---------------------------------------------------------------------------");
			if (calculate)
			{
				if (selected == 0)
				{
					gm = 9.72405E+11f;
					radius = 315000f;
					diffy = "Normal";
					apo = (float)Convert.ToInt32(HeightA);
					peri = (float)Convert.ToInt32(HeightP);
					apo += radius;
					peri += radius;
					sma = (apo + peri) / 2f;
					A = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / apo - 1f / sma))));
					P = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / peri - 1f / sma))));
					if (apo == 315000f && peri == 315000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (peri == 315000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (apo == 315000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					else
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					GUI.Label(new Rect(10f, 300f, 200f, 25f), "Difficulty: " + diffy);
					GUI.Label(new Rect(10f, 375f, 200f, 25f), "SMA: " + sma.ToString() + " m");
				}
				if (selected == 1)
				{
					gm = 3.88962E+12f;
					radius = 630000f;
					diffy = "Hard";
					apo = (float)Convert.ToInt32(HeightA);
					peri = (float)Convert.ToInt32(HeightP);
					apo += radius;
					peri += radius;
					sma = (apo + peri) / 2f;
					A = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / apo - 1f / sma))));
					P = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / peri - 1f / sma))));
					if (apo == 630000f && peri == 630000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (peri == 630000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (apo == 630000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					else
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					GUI.Label(new Rect(10f, 300f, 200f, 25f), "Difficulty: " + diffy);
					GUI.Label(new Rect(10f, 375f, 200f, 25f), "SMA: " + sma.ToString() + " m");
				}
				if (selected == 2)
				{
					gm = 3.88962E+14f;
					radius = 6300000f;
					diffy = "Realistic";
					apo = (float)Convert.ToInt32(HeightA);
					peri = (float)Convert.ToInt32(HeightP);
					apo += radius;
					peri += radius;
					sma = (apo + peri) / 2f;
					A = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / apo - 1f / sma))));
					P = (double)Convert.ToInt32(Math.Sqrt((double)(gm * (2f / peri - 1f / sma))));
					if (apo == 6300000f && peri == 6300000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (peri == 6300000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + 0 + " m/s");
					}
					else if (apo == 6300000f)
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + 0 + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					else
					{
						GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
						GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					}
					GUI.Label(new Rect(10f, 300f, 200f, 25f), "Difficulty: " + diffy);
					GUI.Label(new Rect(10f, 325f, 200f, 25f), "Velocity at Apoapsis: " + A.ToString() + " m/s");
					GUI.Label(new Rect(10f, 350f, 200f, 25f), "Velocity at Periapsis: " + P.ToString() + " m/s");
					GUI.Label(new Rect(10f, 375f, 200f, 25f), "SMA: " + sma.ToString() + " m");
				}
			}

		}
	}
}
