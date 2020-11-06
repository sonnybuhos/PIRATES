namespace GameCreator.Inventory
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GameCreator.Core;

#if UNITY_EDITOR
	using UnityEditor;
	using System.IO;
#endif

	[System.Serializable]
	public class RonsRecipe 
	{
		// PROPERTIES: -------------------------------------------------------------------------------------------------
		public ItemHolder Crafting;

		public ItemHolder[] Ingredients;
		public int[] IngredientAmounts;

	//	[Tooltip("If on, itemA and itemB will be removed from the Player's inventory.")]
		public bool removeItemsOnCraft = true;
//		public IActionsList actionsList;

		// CONSTRUCTOR: ------------------------------------------------------------------------------------------------


	}
}