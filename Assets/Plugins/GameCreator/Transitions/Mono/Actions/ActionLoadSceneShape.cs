namespace GameCreator.Transitions
{
    using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
    using UnityEngine.SceneManagement;
	using GameCreator.Core;
    using GameCreator.Variables;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif

	[AddComponentMenu("")]
    public class ActionLoadSceneShape : IAction
	{
		private const string RESOURCE_TRANSITION = "GameCreator/Transitions/ColorShape";

		public string scene;
        public LoadSceneMode mode = LoadSceneMode.Single;
		public float duration = 1.0f;

        public Image.FillMethod fillMethod = Image.FillMethod.Vertical;
        public int fillOrigin = 0;
        public bool fillClockwise = true;
        public ColorProperty color = new ColorProperty(Color.black);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            GameObject prefabTransition = Resources.Load<GameObject>(RESOURCE_TRANSITION);
            GameObject transition = TransitionsManager.Instance.StartTransition(
                prefabTransition,
                this.scene,
                this.mode,
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

            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Transitions/Icons/Actions/";

		public static new string NAME = "Transitions/Load Scene Shape";
		private const string NODE_TITLE = "Morph to scene {0} in {1} s";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spScene;
        private SerializedProperty spMode;
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
			return string.Format(NODE_TITLE, this.scene, this.duration);
		}

		protected override void OnEnableEditorChild ()
		{
			this.spScene = this.serializedObject.FindProperty("scene");
            this.spMode = this.serializedObject.FindProperty("mode");
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
			this.spScene = null;
            this.spMode = null;
			this.spDuration = null;
            this.spFillMethod = null;
            this.spFillOrigin = null;
            this.spFillClockwise = null;
            this.spColor = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spScene);
            EditorGUILayout.PropertyField(this.spMode);

			EditorGUILayout.PropertyField(this.spDuration);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(this.spFillMethod);

            string[] originNames;
            int[] originValues;
            switch ((Image.FillMethod)this.spFillMethod.intValue)
            {
                case Image.FillMethod.Horizontal :
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

                default :
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
