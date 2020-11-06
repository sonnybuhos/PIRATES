namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	[AddComponentMenu("")]
	public class ConditionIsMouseMoving : ICondition
	{
		

        public override bool Check()
		{
            if (Input.GetAxis("Mouse X") == 0 && (Input.GetAxis("Mouse Y") == 0)) return false;
            else
            {
                return true;
            }
        }
        
		#if UNITY_EDITOR
        public static new string NAME = "Custom/IsMouseMoving";


		#endif
	}
}
