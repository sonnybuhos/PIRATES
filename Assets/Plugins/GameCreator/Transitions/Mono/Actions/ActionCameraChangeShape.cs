namespace GameCreator.Camera
{
    using System;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;
	using UnityEngine.Events;
	using GameCreator.Core;
	using GameCreator.Core.Hooks;
    using GameCreator.Variables;
    using GameCreator.Transitions;

    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    [AddComponentMenu("")]
	public class ActionCameraChangeShape : IAction 
	{
        private const string RESOURCE_TRANSITION = "GameCreator/Transitions/ColorShape";

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool mainCameraMotor = false;
		public CameraMotor cameraMotor;

        public float duration = 0.25f;
        
        public Image.FillMethod fillMethod = Image.FillMethod.Vertical;
        public int fillOrigin = 0;
        public bool fillClockwise = true;
        public ColorProperty color = new ColorProperty(Color.black);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            if (HookCamera.Instance != null)
            {
                CameraController cameraController = HookCamera.Instance.Get<CameraController>();
                if (cameraController != null)
                {
                    CameraMotor motor = (this.mainCameraMotor
                        ? CameraMotor.MAIN_MOTOR
                        : this.cameraMotor
                    );
                      
                    GameObject prefabTransition = Resources.Load<GameObject>(RESOURCE_TRANSITION);
                    GameObject transition = TransitionsManager.Instance.StartTransition(
                        prefabTransition,
                        motor,
                        this.duration
                    );

                    Image transitionImage = transition.GetComponentInChildren<Image>();
                    if (transitionImage != null)
                    {
                        transitionImage.fillMethod = this.fillMethod;
                        transitionImage.fillOrigin = this.fillOrigin;
                        transitionImage.fillClockwise = this.fillClockwise;
                        transitionImage.color = this.color.GetValue(target);
                    }
                }
            }

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Transitions/Icons/Actions/";

        public static new string NAME = "Transitions/Change Camera Shape";
		private const string NODE_TITLE = "Morph to camera {0}";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spMainCameraMotor;
		private SerializedProperty spCameraMotor;
		private SerializedProperty spDuration;

        private SerializedProperty spFillMethod;
        private SerializedProperty spFillOrigin;
        private SerializedProperty spFillClockwise;
        private SerializedProperty spColor;

        private string[] HORIZONTAL_NAMES;
        private string[] VERTICAL_NAMES;
        private string[] RADIAL90_NAMES;
        private string[] RADIAL180_NAMES;
        private string[] RADIAL360_NAMES;

        private int[] HORIZONTAL_VALUES;
        private int[] VERTICAL_VALUES;
        private int[] RADIAL90_VALUES;
        private int[] RADIAL180_VALUES;
        private int[] RADIAL360_VALUES;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
		{
            string cameraName;
            if (this.mainCameraMotor) cameraName = "Main Camera Motor";
            else cameraName = (this.cameraMotor == null ? "none" : this.cameraMotor.gameObject.name);

			return string.Format(NODE_TITLE, cameraName);
		}

		protected override void OnEnableEditorChild ()
		{
            this.spMainCameraMotor = this.serializedObject.FindProperty("mainCameraMotor");
			this.spCameraMotor = this.serializedObject.FindProperty("cameraMotor");
			this.spDuration = this.serializedObject.FindProperty("duration");

            this.spFillMethod = this.serializedObject.FindProperty("fillMethod");
            this.spFillOrigin = this.serializedObject.FindProperty("fillOrigin");
            this.spFillClockwise = this.serializedObject.FindProperty("fillClockwise");
            this.spColor = this.serializedObject.FindProperty("color");

            this.HORIZONTAL_NAMES = (string[])Enum.GetNames(typeof(Image.OriginHorizontal));
            this.VERTICAL_NAMES = (string[])Enum.GetNames(typeof(Image.OriginVertical));
            this.RADIAL90_NAMES = (string[])Enum.GetNames(typeof(Image.Origin90));
            this.RADIAL180_NAMES = (string[])Enum.GetNames(typeof(Image.Origin180));
            this.RADIAL360_NAMES = (string[])Enum.GetNames(typeof(Image.Origin360));

            this.HORIZONTAL_VALUES = (int[])Enum.GetValues(typeof(Image.OriginHorizontal));
            this.VERTICAL_VALUES = (int[])Enum.GetValues(typeof(Image.OriginVertical));
            this.RADIAL90_VALUES = (int[])Enum.GetValues(typeof(Image.Origin90));
            this.RADIAL180_VALUES = (int[])Enum.GetValues(typeof(Image.Origin180));
            this.RADIAL360_VALUES = (int[])Enum.GetValues(typeof(Image.Origin360));
        }

		protected override void OnDisableEditorChild ()
		{
			this.spMainCameraMotor = null;
			this.spCameraMotor = null;
            this.spDuration = null;

            this.spFillMethod = null;
            this.spFillOrigin = null;
            this.spFillClockwise = null;
            this.spColor = null;
        }

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spMainCameraMotor);

            EditorGUI.BeginDisabledGroup(this.spMainCameraMotor.boolValue);
			EditorGUILayout.PropertyField(this.spCameraMotor);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spDuration);
            EditorGUILayout.PropertyField(this.spFillMethod);

            string[] originNames;
            int[] originValues;
            switch ((Image.FillMethod)this.spFillMethod.intValue)
            {
                case Image.FillMethod.Horizontal:
                    originNames = HORIZONTAL_NAMES;
                    originValues = HORIZONTAL_VALUES;
                    break;

                case Image.FillMethod.Vertical:
                    originNames = VERTICAL_NAMES;
                    originValues = VERTICAL_VALUES;
                    break;

                case Image.FillMethod.Radial90:
                    originNames = RADIAL90_NAMES;
                    originValues = RADIAL90_VALUES;
                    break;

                case Image.FillMethod.Radial180:
                    originNames = RADIAL180_NAMES;
                    originValues = RADIAL180_VALUES;
                    break;

                case Image.FillMethod.Radial360:
                    originNames = RADIAL360_NAMES;
                    originValues = RADIAL360_VALUES;
                    break;

                default:
                    originNames = new string[0];
                    originValues = new int[0];
                    break;
            }

            this.spFillOrigin.intValue = EditorGUILayout.IntPopup(
                this.spFillOrigin.displayName,
                this.spFillOrigin.intValue,
                originNames,
                originValues
            );

            if (this.spFillMethod.intValue == (int)Image.FillMethod.Radial90 ||
                this.spFillMethod.intValue == (int)Image.FillMethod.Radial180 ||
                this.spFillMethod.intValue == (int)Image.FillMethod.Radial360)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(this.spFillClockwise);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(this.spColor);

            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}