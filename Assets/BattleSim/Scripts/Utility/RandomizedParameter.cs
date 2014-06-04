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
	public class RandomizedParameter
	{
		#region Vars
		public float minValue;
		public float maxValue;

		protected bool generated = false;
		protected float generatedValue;
		protected float initialGeneratedValue;
		#endregion

		#region Methods
		public virtual float GetRandomNumberInRange()
		{
			return Random.Range(minValue, maxValue);
		}

		public virtual void GenerateValue()
		{
			generatedValue = GetRandomNumberInRange();
		}

		public virtual float GetValue()
		{
			if (!generated)
			{
				GenerateValue();
				initialGeneratedValue = generatedValue;
				generated = true;
			}
			return generatedValue;
		}
		#endregion
	}
}
