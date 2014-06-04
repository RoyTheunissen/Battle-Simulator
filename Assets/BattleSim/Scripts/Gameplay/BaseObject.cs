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
	public class BaseObject : MonoBehaviour
	{
		#region Vars
		#region Constant
		#endregion Constant

		#region Inspector
		#endregion Inspector

		#region Parameters
		#endregion Parameters

		#region Public
		private Transform m_cachedTransform;
		public Transform CachedTransform
		{
			get
			{
				if (m_cachedTransform == null)
				{
					m_cachedTransform = transform;
				}
				return m_cachedTransform;
			}
		}
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
		#endregion Overrides
		#endregion Methods
	}
}
