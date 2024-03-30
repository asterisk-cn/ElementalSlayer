using UnityEngine;

namespace MyGame
{
	public class AnimatorParentMove:MonoBehaviour
	{
		public Animator animator;
		public PlayerController playerController;

		void OnAnimatorMove()
		{
			if(playerController.useRootMotion) {
				transform.parent.rotation = animator.rootRotation;
				transform.parent.position += animator.deltaPosition;
			}
		}
	}
}
