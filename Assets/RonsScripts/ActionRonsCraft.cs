namespace GameCreator.Inventory
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator.Core;

	#if UNITY_EDITOR
	using UnityEditor;
	using TMPro;
#endif

	[AddComponentMenu("")]
	public class ActionRonsCraft : IAction 
	{
		public RonsRecipeCookBook CookBook;
		public ItemHolder Crafting;
		private bool RecipieValid;
		private bool RecipeAccepted;
		private int CookbookID;

		// EXECUTABLE: -------------------------------------------------------------------------------------------------

		public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			// InventoryManager.Instance.UseRecipe(this.item1.item.uuid, this.item2.item.uuid);
			RecipieValid = false;

			for (int i = 0; i < CookBook.CookBook.Length; i++)
			{
				if (Crafting.item.uuid == CookBook.CookBook[i].Crafting.item.uuid)
					{
						RecipieValid = true;
						CookbookID = i;
					}				
			}

			if (RecipieValid)
			{
				RecipeAccepted = true;
				for (int i = 0; i < CookBook.CookBook[CookbookID].Ingredients.Length; i++)
				{
					if (CookBook.CookBook[CookbookID].IngredientAmounts[i] > InventoryManager.Instance.GetInventoryAmountOfItem(CookBook.CookBook[CookbookID].Ingredients[i].item.uuid))
						RecipeAccepted = false;					
				}

				if (RecipeAccepted)
				{
					InventoryManager.Instance.AddItemToInventory(Crafting.item.uuid, 1);
					for (int i = 0; i < CookBook.CookBook[CookbookID].Ingredients.Length; i++)
					{
						InventoryManager.Instance.SubstractItemFromInventory(CookBook.CookBook[CookbookID].Ingredients[i].item.uuid, CookBook.CookBook[CookbookID].IngredientAmounts[i]);
					}

				}

				}


            return true;
        }

		// +-----------------------------------------------------------------------------------------------------------+
		// | EDITOR                                                                                                    |
		// +-----------------------------------------------------------------------------------------------------------+

		#if UNITY_EDITOR

		public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Inventory/Icons/Actions/";

		public static new string NAME = "Inventory/Rons Craft!";
		private const string NODE_TITLE = "Crafting Your Way, Always";

		// PROPERTIES: -----------------------------------------------------------------------------------------------------

		private SerializedProperty spItem1;
		private SerializedProperty spCookBook;
		private SerializedProperty spCrafting;

		// INSPECTOR METHODS: ----------------------------------------------------------------------------------------------
		public override string GetNodeTitle()
		{
			return string.Format(
				NODE_TITLE
			);


		}
		protected override void OnEnableEditorChild ()
		{
			//		this.spItem1 = this.serializedObject.FindProperty("item1");
			this.spCookBook = this.serializedObject.FindProperty("CookBook");
			this.spCrafting = this.serializedObject.FindProperty("Crafting");
		}

		protected override void OnDisableEditorChild ()
		{
			//		this.spItem1 = null;
			this.spCookBook = null;
			this.spCrafting = null;
		}

		public override void OnInspectorGUI()
		{
			this.serializedObject.Update();

			//		EditorGUILayout.PropertyField(this.spItem1);
			EditorGUILayout.PropertyField(this.spCookBook);
			EditorGUILayout.PropertyField(this.spCrafting);

			this.serializedObject.ApplyModifiedProperties();
		}

		#endif
	}
}