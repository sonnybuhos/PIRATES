namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Variables;
	


	[AddComponentMenu("")]
	public class ProjectileAction : IAction
	{
		public TargetGameObject prefab = new TargetGameObject();
		public TargetPosition initLocation = new TargetPosition(TargetPosition.Target.Player);
		public bool Gravity = false;
		public NumberProperty ShootForce = new NumberProperty(10f);


		[Space]
		public VariableProperty storeInstance = new VariableProperty();
		

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)

		{
			    GameObject prefabValue = this.prefab.GetGameObject(target);
			   
		     	

		 	if (prefabValue != null)


				{

				Vector3 position = this.initLocation.GetPosition(target, Space.Self);
				Quaternion rotation = this.initLocation.GetRotation(target);
				rotation = this.initLocation.GetRotation(target);

				

				Vector3 p = transform.forward;

				GameObject instance = Instantiate(prefabValue, position, rotation);

				if (instance != null) this.storeInstance.Set(instance, target);






				instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

				instance.GetComponent<Rigidbody>().AddRelativeForce(p * this.ShootForce.GetValue(target));

				if (Gravity == false)

				{
					instance.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
				}

			}

			return true;
		}


#if UNITY_EDITOR
		public static new string NAME = "Custom/Projectile Action";

#endif
	}
}