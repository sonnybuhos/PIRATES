namespace GameCreator.Core
{
	using UnityEngine;
    using UnityEngine.UI;

	#if UNITY_EDITOR
	using UnityEditor;
	using UnityEngine.EventSystems;
	#endif

	[AddComponentMenu("")]
	public class ActionDeselectUIElement : IAction
	{
        public Selectable selectable;

		// EXECUTABLE: ----------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			if (this.selectable != null)
			{
				EventSystem system = FindObjectOfType<EventSystem>();
				if (system != null)
				{
					system.SetSelectedGameObject(null);
				}
			}

            return true;
		}

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

        #if UNITY_EDITOR

		public static new string NAME = "UI/Deselect UI Element";
        private const string NODE_TITLE = "Deselect {0}";

		// PROPERTIES: ----------------------------------------------------------------------------

		private SerializedProperty spSelectable;


		// INSPECTOR METHODS: ---------------------------------------------------------------------

		public override string GetNodeTitle()
		{
            return string.Format(NODE_TITLE, (this.selectable == null ? "None" : this.selectable.name));
		}

		protected override void OnEnableEditorChild ()
		{
            this.spSelectable = this.serializedObject.FindProperty("selectable");
		}

		protected override void OnDisableEditorChild ()
		{
            this.spSelectable = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.spSelectable);
			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}
