using UnityEngine;
using System.Collections;

namespace BattleSim.Utility
{
	/// <summary>
	/// This is a parameter that has a minimum value and a maximum value.
	/// It generates a random value in that range once,
	/// after generating it it stays the same.
	/// </summary>
	[System.Serializable]
	public class Attribute : RandomizedParameter
	{
		#region Vars
		#endregion

		#region Methods
		public void Decrement(float subtraction)
		{
			SetValue(generatedValue - subtraction);
		}

		public void Increment(float addition)
		{
			SetValue(generatedValue + addition);
		}

		public void SetValue(float newValue)
		{
			generatedValue = Mathf.Clamp(
				newValue, 0, initialGeneratedValue);
		}
		#endregion
	}
}
