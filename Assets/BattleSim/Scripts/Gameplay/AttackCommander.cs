//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleSim.Gameplay
{
	/// <summary>
	/// Commands units in an attacking manner.
	/// </summary>
	public class AttackCommander : Commander
	{
		#region Vars
		#region Inspector
		[SerializeField]
		private Building m_enemyCore;
		#endregion

		#region Public
		#endregion

		#region Private
		#endregion
		#endregion

		#region Methods
		#region Unity Callbacks
		void Start()
		{
			CommandAllUnits(CommandToStorm, m_enemyCore);
		}
		#endregion

		#region Public
		#endregion

		#region Private
		protected virtual void CommandToStorm(Unit unit, params object[] args)
		{
			if (args.Length >= 1 && args[0] is GameplayObject)
			{
				unit.Storm(args[0] as Building);
			}
		}
		#endregion

		#region Overrides
		#endregion
		#endregion
	}
}
