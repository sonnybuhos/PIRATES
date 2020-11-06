namespace GameCreator.Camera
{
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
	public class ActionCameraChangeFade : IAction 
	{
        private const string RESOURCE_TRANSITION = "GameCreator/Transitions/ColorFade";

        // PROPERTIES: ----------------------------------------------------------------------------

        public bool mainCameraMotor = false;
		public CameraMotor cameraMotor;

        public float duration = 0.25f;
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
                    if (transitionImage != null) transitionImage.color = this.color.GetValue(target);
                }
            }

            return true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Transitions/Icons/Actions/";

        public static new string NAME = "Transitions/Change Camera Fade";
		private const string NODE_TITLE = "Fade to camera {0}";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spMainCameraMotor;
		private SerializedProperty spCameraMotor;
		private SerializedProperty spDuration;
        private SerializedProperty spColor;

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
            this.spColor = this.serializedObject.FindProperty("color");
        }

		protected override void OnDisableEditorChild ()
		{
			this.spMainCameraMotor = null;
			this.spCameraMotor = null;
            this.spDuration = null;
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
            EditorGUILayout.PropertyField(this.spColor);

            this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}