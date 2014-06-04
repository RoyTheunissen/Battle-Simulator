//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BattleSim.Utility;

namespace BattleSim.Gameplay
{
	/// <summary>
	/// Base Unit.
	/// </summary>
	public class Catapult : Archer
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
		#endregion Private
		#endregion Vars

		#region Methods
		#region Unity Callbacks
		#endregion Unity Callbacks

		#region Public
		#endregion Public

		#region Private
		#endregion Private

		#region Overrides
		protected override void AttackEnemy()
		{
			if (CanAttack())
			{
				base.AttackEnemy();

				m_animator.Stop();
				m_animator.Play("Fire");
				Debug.LogWarning("FIRE! " + Time.time);
			}
		}
		#endregion Overrides
		#endregion Methods
	}
}
