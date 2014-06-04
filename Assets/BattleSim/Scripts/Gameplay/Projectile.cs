//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BattleSim.Utility;

namespace BattleSim.Gameplay
{
	/// <summary>
	/// Moves through the air and hits enemies.
	/// </summary>
	public class Projectile : BaseObject
	{
		#region Vars
		#region Constant
		#endregion Constant

		#region Inspector
		[SerializeField]
		private float m_speed;

		[SerializeField]
		private float m_arc;

		[SerializeField]
		private RandomizedParameter m_attackDamage;
		#endregion Inspector

		#region Parameters
		#endregion Parameters

		#region Public
		#endregion Public

		#region Private
		private GameplayObject m_intendedTarget;

		private Vector3 m_startPos;
		private Vector3 m_endPos;
		private float m_distance;

		private float m_duration;

		private float m_startTime = -1;

		private float m_currentNormal;
		private Vector3 m_nextPos;
		#endregion Private
		#endregion Vars

		#region Methods
		#region Unity Callbacks
		protected virtual void Awake()
		{
		}

		protected virtual void Update()
		{
			if (m_startTime != -1)
			{
				// Figure out the current normal.
				m_currentNormal = GetMovementNormal();

				// Figure out where to move to.
				m_nextPos = GetPositionAlongArc(m_currentNormal);

				// Face our movement direction.
				CachedTransform.LookAt(m_nextPos);

				// Add a rotational offset.
				transform.Rotate(GetRotationOffset());

				// Perform the movement.
				transform.position = m_nextPos;

				// If we are done, destroy ourselves!
				if (Time.time > m_startTime + m_duration)
				{
					Hit();
				}
			}
		}
		#endregion Unity Callbacks

		#region Public
		public void Fire(GameplayObject target)
		{
			m_intendedTarget = target;

			m_startPos = CachedTransform.position;
			m_endPos = target.CachedTransform.position;

			m_distance = (m_endPos - m_startPos).magnitude;
			m_duration = m_distance / m_speed;

			m_startTime = Time.time;
		}
		#endregion Public

		#region Private
		protected virtual Vector3 GetRotationOffset()
		{
			return Vector3.zero;
		}

		protected virtual void Die()
		{
			GameObject.Destroy(gameObject);
		}

		protected virtual void Hit()
		{
			// If we still have a valid target, damage it.
			if (m_intendedTarget != null
				&& !m_intendedTarget.IsDead())
			{
				// Inflict it random damage.
				m_intendedTarget.TakeDamage(this,
					m_attackDamage.GetValue());
			}
			Die();
		}

		protected virtual void Miss()
		{
			Die();
		}

		protected virtual float GetMovementNormal()
		{
			if (m_startTime == -1)
			{
				return 0;
			}
			else
			{
				return (Time.time - m_startTime) / m_duration;
			}
		}

		protected virtual Vector3 GetPositionAlongArc(float normal)
		{
			return Vector3.Lerp(m_startPos, m_endPos, normal)
				+ (Vector3.up * m_distance * m_arc * Mathf.Sin(normal * Mathf.PI));
		}
		#endregion Private

		#region Overrides
		#endregion Overrides
		#endregion Methods
	}
}
