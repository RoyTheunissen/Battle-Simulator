//------------------------------------------------------------------------------------------------
// Trepid Framework copyright 2013-2014 Roy Theunissen. All rights reserved.
//------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using BattleSim.Utility;

namespace BattleSim.Gameplay
{
	public delegate void StaggeredCallback();
	public delegate void DiedEventHandler();

	/// <summary>
	/// Base object that belongs to a faction.
	/// </summary>
	public class GameplayObject : BaseObject
	{
		#region Events
		public event DiedEventHandler DiedEvent;
		protected virtual void DispatchDiedEvent()
		{
			if (DiedEvent != null)
			{
				DiedEvent();
			}
		}
		#endregion Events

		#region Vars
		#region Inspector
		[SerializeField]
		private Faction m_faction;

		[SerializeField]
		protected Animation m_animator;

		#region Base Attributes
		[SerializeField]
		private Attribute m_healthPoints;

		[SerializeField]
		private RandomizedParameterSquared m_visualRange;

		[SerializeField]
		private RandomizedParameter m_attackDamage;

		[SerializeField]
		private RandomizedParameter m_attackSpeed;

		[SerializeField]
		private RandomizedParameterSquared m_attackDistance;

		[SerializeField]
		private RandomizedParameter m_pursuance;

		[SerializeField]
		private bool m_acquireEnemiesAroundInitialPosition;
		#endregion Base Attributes
		#endregion

		#region Public
		#endregion

		#region Private
		protected Vector3 m_initialPosition;

		protected float m_distanceToEnemySqr;
		
		protected GameplayObject m_enemyTarget;

		private GameplayObject m_targetCandidate;
		private float m_targetCandidateDistance;
		private GameplayObject m_targetBestCandidate;
		private float m_targetBestCandidateDistance;
		#endregion
		#endregion

		#region Methods
		#region Unity Callbacks
		protected virtual void Awake()
		{
			m_initialPosition = CachedTransform.position;

			StartStaggeredProcesses();
		}

		protected virtual void Update()
		{
		}
		#endregion

		#region Public
		/// <summary>
		/// Try to persuade the unit to join the specified faction.
		/// The unit has every right to decline if it so chooses.
		/// </summary>
		/// <param name="faction"></param>
		/// <returns><c>True</c> if the unit is persuaded,
		/// <c>false</c> otherwise.</returns>
		public virtual bool PersuadeToJoinFaction(Faction faction)
		{
			// If we haven't enlisted anywhere, just join.
			if (m_faction == null)
			{
				JoinFaction(faction);
				return true;
			}
			else // If we've already enlisted, remain loyal!
			{
				return false;
			}
		}

		#region Conditions
		/// <summary>
		/// Whether the gameplayobject has run out of health.
		/// </summary>
		/// <returns><c>True</c> if it's dead, <c>false</c> otherwise.</returns>
		public bool IsDead()
		{
			return m_healthPoints.GetValue() <= 0;
		}
		
		protected virtual bool CanAttack()
		{
			return HaveEnemyTarget() && m_distanceToEnemySqr != 0
				&& IsInAttackRange();
		}
		#endregion Conditions

		#region Attributes
		/// <summary>
		/// Make this gameplay object die.
		/// As of yet it disappears instantly, for simplicity.
		/// </summary>
		public void Die()
		{
			DispatchDiedEvent();
			GameObject.Destroy(gameObject);
			m_faction.RemoveObject(this);
		}

		/// <summary>
		/// Take damage from a specific source.
		/// </summary>
		/// <param name="inflictor">Source of the damage.</param>
		/// <param name="damage">Amount of damage.</param>
		public void TakeDamage(BaseObject inflictor, float damage)
		{
			// We don't take any more damage if we're dead.
			if (IsDead())
			{
				return;
			}

			// Take damage.
			m_healthPoints.Decrement(damage);

			// Die if we're now out of health.
			if (IsDead())
			{
				Die();
			}
		}
		#endregion Attributes

		#endregion

		#region Private
		#region Staggering
		protected virtual void StartStaggeredProcesses()
		{
			StartStaggeredProcess(LookForEnemyTarget, 1.0f, 3.0f);
			StartStaggeredProcess(FindDistanceToEnemy, 1.0f, 1.5f);

			StartStaggeredProcess(AttackEnemy,
				m_attackSpeed.GetValue(), m_attackSpeed.GetValue());
		}

		protected IEnumerator StaggeredProcess(StaggeredCallback callback,
			float minDelay, float maxDelay)
		{
			while (true)
			{
				yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
				callback();
			}
		}

		protected void StartStaggeredProcess(StaggeredCallback callback,
			float minDelay, float maxDelay)
		{
			StartCoroutine(StaggeredProcess(callback, minDelay, maxDelay));
		}
		#endregion Staggering

		#region Logic
		protected virtual float GetVisualRangeSqr()
		{
			return m_visualRange.GetValueSqr();
		}

		protected virtual Vector3 GetEnemySearchCenter()
		{
			if (m_acquireEnemiesAroundInitialPosition)
			{
				return m_initialPosition;
			}
			else
			{
				return CachedTransform.position;
			}
		}

		protected virtual bool CanSeeObject(GameplayObject obj)
		{
			if ((obj.CachedTransform.position
				- GetEnemySearchCenter()).sqrMagnitude
				< GetVisualRangeSqr())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		protected virtual bool CanTargetObject(GameplayObject target)
		{
			if (m_targetCandidate == null || m_targetCandidate.IsDead()
					|| !CanSeeObject(m_targetCandidate))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		protected virtual void JoinFaction(Faction faction)
		{
			m_faction = faction;

			AssumeMaterial(m_faction.GetMaterial());
		}

		protected virtual void AssumeMaterial(Material material)
		{
			Renderer[] meshRenderers = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				meshRenderers[i].sharedMaterial = material;
			}
		}

		/// <summary>
		/// Consider abandoning our current target.
		/// Units can be configured to pursue targets more fervently.
		/// </summary>
		/// <returns><c>True</c> if it should abandon the target,
		/// <c>false</c> otherwise.</returns>
		protected virtual bool ConsiderAbandoningTarget()
		{
			return true;// Random.Range(0.0f, 100.0f) > m_pursuance.GetValue();
		}

		protected virtual void LookForEnemyTarget()
		{
			// Stop if we already have a target.
			// In the future, this should consider more conditions.
			if (HaveEnemyTarget() && !ConsiderAbandoningTarget())
			{
				return;
			}

			// Initialize the search.
			m_targetBestCandidate = null;
			m_targetBestCandidateDistance = float.MaxValue;

			// Go through all potential enemies.
			for (int i = 0; i < m_faction.GetEnemyUnits().Count; i++)
			{
				// Find the candidate.
				m_targetCandidate = m_faction.GetEnemyUnits()[i];

				// Filter out invalid candidates.
				if (!CanTargetObject(m_targetCandidate))
				{
					continue;
				}

				// Find out how close it is to us.
				m_targetCandidateDistance =
					(m_targetCandidate.CachedTransform.position
					- CachedTransform.position).sqrMagnitude;

				// Find out if it is the best one.
				if (m_targetCandidateDistance < m_targetBestCandidateDistance)
				{
					m_targetBestCandidateDistance = m_targetCandidateDistance;
					m_targetBestCandidate = m_targetCandidate;
				}
			}
			m_targetCandidate = null;

			// Assume the new target.
			if (m_targetBestCandidate != null)
			{
				SetEnemyTarget(m_targetBestCandidate);
			}
		}

		/// <summary>
		/// Try to attack our enemy target.
		/// </summary>
		protected virtual void AttackEnemy()
		{
			// First check that we can attack.
			if (CanAttack())
			{
				m_enemyTarget.TakeDamage(this,
					m_attackDamage.GetValue()
					);
			}
		}

		protected virtual void FindDistanceToEnemy()
		{
			if (!HaveEnemyTarget())
			{
				m_distanceToEnemySqr = 0;
			}
			else
			{
				m_distanceToEnemySqr =
					(m_enemyTarget.CachedTransform.position
					- CachedTransform.position).sqrMagnitude;

				if (IsInAttackRange())
				{

				}
			}
		}

		protected virtual float GetAttackDistanceSqr()
		{
			return m_attackDistance.GetValueSqr();
		}

		protected virtual bool IsInAttackRange()
		{
			return m_distanceToEnemySqr <= GetAttackDistanceSqr();
		}

		protected virtual void EnemyDiedEventHandler()
		{
			// Cancel if we already don't have a target any more.
			if (m_enemyTarget == null)
			{
				return;
			}

			// Unsubscribe to the die event.
			m_enemyTarget.DiedEvent -= EnemyDiedEventHandler;

			// Forget about the enemy.
			m_enemyTarget = null;
		}

		protected virtual bool HaveEnemyTarget()
		{
			return m_enemyTarget != null && !m_enemyTarget.IsDead();
		}

		protected virtual void SetEnemyTarget(GameplayObject target)
		{
			// Unsubscribe to the old target's death event.
			if (m_enemyTarget != null && m_enemyTarget != target)
			{
				m_enemyTarget.DiedEvent -= EnemyDiedEventHandler;
			}

			// Set the new target.
			m_enemyTarget = target;

			// Subscribe to the new target's death event.
			if (m_enemyTarget != null)
			{
				m_enemyTarget.DiedEvent += EnemyDiedEventHandler;
			}
		}
		#endregion
		#endregion

		#region Overrides
		#endregion
		#endregion
	}
}
