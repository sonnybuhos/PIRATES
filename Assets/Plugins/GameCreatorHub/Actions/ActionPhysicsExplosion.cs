namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Variables;

	[AddComponentMenu("")]
	public class ActionPhysicsExplosion : IAction
	{
        public TargetPosition position = new TargetPosition(TargetPosition.Target.Player);
        [Space]
        public NumberProperty radius = new NumberProperty(5f);
        [Space]
        public NumberProperty force = new NumberProperty(10f);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Vector3 originPosition = this.position.GetPosition(target);
            float originRadius = this.radius.GetValue();
            float originForce = this.force.GetValue();

            QueryTriggerInteraction query = QueryTriggerInteraction.Ignore;
            Collider[] colliders = Physics.OverlapSphere(
                originPosition, originRadius, 
                Physics.AllLayers, query
            );

            for (int i = 0; i < colliders.Length; ++i)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (rb != null) rb.AddExplosionForce(
                    originForce, originPosition, originRadius, 
                    0f, ForceMode.Impulse
                );
            }

            return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Physics/Explosion";
		#endif
	}
}
