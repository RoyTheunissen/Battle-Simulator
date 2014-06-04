//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BattleSim.Gameplay
{
	/// <summary>
	/// A group to which buildings and units and such belong.
	/// </summary>
	[RequireComponent(typeof(Commander))]
	public class Faction : MonoBehaviour
	{
		#region Vars
		#region Inspector
		[SerializeField]
		private string m_factionName;

		[SerializeField]
		private Color m_color;

		[SerializeField]
		private Faction m_enemy;

		[SerializeField]
		private Commander m_commander;

		[SerializeField]
		private Material m_material;
		#endregion

		#region Public
		#endregion

		#region Private
		protected List<GameplayObject> m_objects = new List<GameplayObject>();
		protected List<Unit> m_units = new List<Unit>();
		protected List<Building> m_buildings = new List<Building>();
		#endregion
		#endregion

		#region Methods
		#region Unity Callbacks
		void Awake()
		{
			RecruitUnitsAndBuildings();
		}
		#endregion

		#region Public
		public Material GetMaterial()
		{
			return m_material;
		}

		public List<Building> GetBuildings()
		{
			return m_buildings;
		}
		
		public List<Unit> GetUnits()
		{
			return m_units;
		}

		public void RemoveObject(GameplayObject obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is Unit)
			{
				m_units.Remove(obj as Unit);
			}
			if (obj is Building)
			{
				m_buildings.Remove(obj as Building);
			}
			m_objects.Remove(obj);
		}

		public List<GameplayObject> GetObjects()
		{
			return m_objects;
		}

		public List<Building> GetEnemyBuildings()
		{
			return m_enemy.GetBuildings();
		}

		public List<Unit> GetEnemyUnits()
		{
			return m_enemy.GetUnits();
		}

		public List<GameplayObject> GetEnemyObjects()
		{
			return m_enemy.GetEnemyObjects();
		}
		#endregion

		#region Private
		/// <summary>
		/// Try to persuade the object to join us.
		/// If we were successful, keep track of it.
		/// </summary>
		/// <param name="obj">Object to recruit.</param>
		protected virtual void Recruit(GameplayObject obj)
		{
			if (obj.PersuadeToJoinFaction(this))
			{
				// Add to unit list if it's a unit.
				if (obj is Unit)
				{
					m_units.Add(obj as Unit);
				}

				// Add to building list if it's a building.
				if (obj is Unit)
				{
					m_units.Add(obj as Unit);
				}
			}
		}

		/// <summary>
		/// Try to recruit all the units and buildings in our transform hierarchy.
		/// </summary>
		protected virtual void RecruitUnitsAndBuildings()
		{
			GameplayObject[] objects = GetComponentsInChildren<GameplayObject>();
			for (int i = 0; i < objects.Length; i++)
			{
				Recruit(objects[i]);
			}
		}
		#endregion

		#region Overrides
		#endregion
		#endregion
	}
}
