namespace GameCreator.Shooter
{
    using GameCreator.Core;
    using UnityEngine;

    [AddComponentMenu("")]
    public class ConditionIsAiming : ICondition
    {
        public enum AimType
        {
            IsAiming,
            IsNotAiming
        }

        public TargetGameObject shooter = new TargetGameObject(TargetGameObject.Target.Player);
        public AimType type = AimType.IsAiming;

        public override bool Check(GameObject target)
        {
            CharacterShooter charShooter = this.shooter.GetComponent<CharacterShooter>(target);
            if (charShooter == null) return this.type != AimType.IsAiming;

            switch (this.type)
            {
                case AimType.IsAiming: return charShooter.isAiming == true;
                case AimType.IsNotAiming: return charShooter.isAiming != true;
            }

            return false;
            
        }

#if UNITY_EDITOR

        public static new string NAME = "Shooter/Is Aiming";
        private const string NODE_TITLE = "Character {0} {1}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                this.shooter,
                UnityEditor.ObjectNames.NicifyVariableName(this.type.ToString())
            );
        }

#endif
    }
}
