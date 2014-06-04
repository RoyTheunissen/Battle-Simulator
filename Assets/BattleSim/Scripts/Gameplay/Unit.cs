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
	public class Unit : GameplayObject
	{
		#region Vars

		#region Inspector
		#region Parameters
		#endregion Parameters

		[SerializeField]
		protected NavMeshAgent m_navAgent;
		#endregion

		#region Public
		#endregion

		#region Private
		private Building m_stormTarget;
		#endregion
		#endregion

		#region Methods
		#region Unity Callbacks
		protected override void Update()
		{
			base.Update();

			//m_navAgent.updateRotation = false;
			if (m_navAgent.velocity == Vector3.zero)
			{
				if (HaveEnemyTarget())
				{
					m_navAgent.updateRotation = false;

					Vector3 delta = CachedTransform.InverseTransformPoint(
						m_enemyTarget.CachedTransform.position);

					float angle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
					transform.Rotate(0, angle * Time.deltaTime * (m_navAgent.angularSpeed/100), 0);
				}
			}
			else
			{
				m_navAgent.updateRotation = true;
			}
		}
		#endregion

		#region Public
		public void Storm(Building obj)
		{
			m_stormTarget = obj;
		}
		#endregion

		#region Private

		#region Staggering
		#endregion Staggering

		#region Locomotion
		protected virtual void MoveTo(Vector3 pos)
		{
			// Dead men don't walk.
			if (IsDead())
			{
				return;
			}

			m_navAgent.SetDestination(pos);
		}

		protected virtual void StopMoving()
		{
			m_navAgent.Stop();
		}

		protected virtual void MoveToEnemy()
		{
			if (!HaveEnemyTarget() || IsInAttackRange())
			{
				return;
			}

			MoveTo(m_enemyTarget.CachedTransform.position);
		}
		#endregion Locomotion

		#region Logic
		#endregion Logic
		#endregion

		#region Overrides
		protected override void LookForEnemyTarget()
		{
			base.LookForEnemyTarget();

			// If we couldn't find a unit to kill, go for the storm target!
			if (!HaveEnemyTarget())
			{
				SetEnemyTarget(m_stormTarget);
			}
		}
		#endregion
		#endregion
	}
}
