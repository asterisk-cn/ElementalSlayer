using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    public class PlayerCharacterAnimatorEvents:MonoBehaviour
    {
		/// <summary>
		/// Placeholder functions for Animation events.
		/// </summary>
		public UnityEvent OnHit = new UnityEvent();
		public UnityEvent OnFootR = new UnityEvent();
		public UnityEvent OnFootL = new UnityEvent();
		public UnityEvent OnLand = new UnityEvent();
		public UnityEvent OnShoot = new UnityEvent();
		public UnityEvent OnWeaponOn = new UnityEvent();
		public UnityEvent OnWeaponOff = new UnityEvent();
		public UnityEvent OnWeaponSwitch = new UnityEvent();

		[HideInInspector] public PlayerController playerController;

		void Start()
		{
			OnWeaponOn.AddListener(playerController.ActivateWeaponCollider);
			OnWeaponOn.AddListener(playerController.PlayAttackSound);
			OnWeaponOff.AddListener(playerController.DeactivateWeaponCollider);
		}
		public void Hit()
		{
			OnHit.Invoke();
		}

		public void FootR()
		{
			OnFootR.Invoke();
		}

		public void FootL()
		{
			OnFootL.Invoke();
		}

		public void Land()
		{
			OnLand.Invoke();
		}

		public void Shoot()
		{
			OnShoot.Invoke();
		}

		public void WeaponOn()
		{
			OnWeaponOn.Invoke();
		}

		public void WeaponOff()
		{
			OnWeaponOff.Invoke();
		}
	}
}
