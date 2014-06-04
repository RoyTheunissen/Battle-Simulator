//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleSim.Gameplay
{
	public delegate void Command(GameplayObject obj, params object[] args);
	public delegate void UnitCommand(Unit unit, params object[] args);

	/// <summary>
	/// Passes commands to a faction's objects.
	/// </summary>
	public class Commander : MonoBehaviour
	{
		#region Vars
		#region Inspector
		[SerializeField]
		private Faction m_faction;
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
			
		}
		#endregion

		#region Public
		#endregion

		#region Private
		protected virtual void CommandAllUnits(UnitCommand command, params object[] args)
		{
			CommandUnits(m_faction.GetUnits(), command, args);
		}

		protected virtual void CommandUnits(List<Unit> units, UnitCommand command, params object[] args)
		{
			for (int i = 0; i < units.Count; i++)
			{
				command(units[i], args);
			}
		}

		protected virtual void CommandAllObjects(Command command, params object[] args)
		{
			CommandObjects(m_faction.GetObjects(), command, args);
		}

		protected virtual void CommandObjects(List<GameplayObject> objects, Command command, params object[] args)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				command(objects[i], args);
			}
		}
		#endregion

		#region Overrides
		#endregion
		#endregion
	}
}
