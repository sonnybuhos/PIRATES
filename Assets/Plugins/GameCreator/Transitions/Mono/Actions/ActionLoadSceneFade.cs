namespace GameCreator.Transitions
{
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
	public class ActionLoadSceneFade : IAction
	{
		private const string RESOURCE_TRANSITION = "GameCreator/Transitions/ColorFade";

		public string scene;
        public LoadSceneMode mode = LoadSceneMode.Single;

		public float duration = 1.0f;
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
            if (transitionImage != null) transitionImage.color = this.color.GetValue(target);

            return true;
        }

		// +--------------------------------------------------------------------------------------+
		// | EDITOR                                                                               |
		// +--------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Transitions/Icons/Actions/";

		public static new string NAME = "Transitions/Load Scene Fade";
		private const string NODE_TITLE = "Fade to scene {0} in {1} s";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spScene;
        private SerializedProperty spMode;

		private SerializedProperty spDuration;
		private SerializedProperty spColor;

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
			this.spColor = this.serializedObject.FindProperty("color");
		}

		protected override void OnDisableEditorChild ()
		{
			this.spScene = null;
            this.spMode = null;

			this.spDuration = null;
			this.spColor = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			EditorGUILayout.PropertyField(this.spScene);
            EditorGUILayout.PropertyField(this.spMode);

            EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.spDuration);
			EditorGUILayout.PropertyField(this.spColor);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
