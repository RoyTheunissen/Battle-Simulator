//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BattleSim.Utility;

namespace BattleSim.Gameplay
{
	/// <summary>
	/// Moves through the air in a small curve and hits enemies.
	/// </summary>
	public class Rock : Projectile
	{
		#region Vars
		#region Constant
		#endregion Constant

		#region Inspector
		#endregion Inspector

		#region Parameters
		#endregion Parameters

		#region Public
		#endregion Public

		#region Private
		private Vector3 m_startRotationOffset;
		private Vector3 m_spin;
		#endregion Private
		#endregion Vars

		#region Methods
		#region Unity Callbacks
		protected override void Awake()
		{
			base.Awake();

			//m_startRotationOffset = Random.onUnitSphere;
			m_spin = Random.insideUnitSphere * 500;
		}
		#endregion Unity Callbacks

		#region Public
		#endregion Public

		#region Private
		#endregion Private

		#region Overrides
		protected override Vector3 GetRotationOffset()
		{
			return m_startRotationOffset + m_spin * Time.time;
		}
		#endregion Overrides
		#endregion Methods
	}
}
