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
	public class Archer : Unit
	{
		#region Vars
		#region Constant
		#endregion Constant

		#region Inspector
		[SerializeField]
		private GameObject m_projectilePrefab;
		#endregion Inspector

		#region Parameters
		#endregion Parameters

		#region Public
		#endregion Public

		#region Private
		#endregion Private
		private Projectile m_lastShotProjectile;
		#endregion Vars

		#region Methods
		#region Unity Callbacks
		#endregion Unity Callbacks

		#region Public
		#endregion Public

		#region Private
		protected virtual void FireAt(GameplayObject target)
		{
			GameObject obj = (GameObject.Instantiate(m_projectilePrefab,
				CachedTransform.position, transform.rotation) as GameObject);
			m_lastShotProjectile = obj.GetComponent<Projectile>();
			m_lastShotProjectile.Fire(target);
		}
		#endregion Private

		#region Overrides
		#endregion Overrides
		protected override void AttackEnemy()
		{
			if (CanAttack())
			{
				FireAt(m_enemyTarget);
			}
		}

		protected override void MoveToEnemy()
		{
			// If we don't have an enemy target, don't move.
			if (!HaveEnemyTarget())
			{
				return;
			}

			// If we're in range, stop moving.
			if (IsInAttackRange())
			{
				StopMoving();
				return;
			}

			base.MoveToEnemy();
		}


		protected override void StartStaggeredProcesses()
		{
			base.StartStaggeredProcesses();

			StartStaggeredProcess(MoveToEnemy, 1.5f, 3.5f);
		}

		protected override void SetEnemyTarget(GameplayObject target)
		{
			base.SetEnemyTarget(target);
			MoveToEnemy();
		}
		#endregion Methods
	}
}
