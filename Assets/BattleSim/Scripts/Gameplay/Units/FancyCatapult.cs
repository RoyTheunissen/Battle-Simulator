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
	public class FancyCatapult : Archer
	{
		#region Vars
		#region Constant
		#endregion Constant

		#region Inspector
		[SerializeField]
		private Animator m_mecanimAnimator;
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
		protected override void Update()
		{
			base.Update();

			// Set the speed parameter based on the movement speed.
			float speed = m_navAgent.velocity.magnitude;
			m_mecanimAnimator.SetFloat("Speed", speed);
			Debug.Log("The current speed is "+speed);
		}
		protected override void AttackEnemy()
		{
			if (CanAttack())
			{
				base.AttackEnemy();

				// Trigger the fire animation.
				m_mecanimAnimator.SetTrigger("Fire");

				Debug.LogWarning("FIRE FANCY! " + Time.time);
			}
		}
		#endregion Overrides
		#endregion Methods
	}
}
