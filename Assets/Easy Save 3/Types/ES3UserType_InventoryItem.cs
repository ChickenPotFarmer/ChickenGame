using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("itemName", "amount", "maxAmount", "isStrain", "locked", "itemID", "previousParent", "targetParent", "currentParent", "uiName", "uiAmount", "cg", "inventoryCanvas", "rectTransform", "inventoryGUI", "hoverInfo", "strainProfile", "seedDropDown", "inventoryController")]
	public class ES3UserType_InventoryItem : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_InventoryItem() : base(typeof(InventoryItem)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (InventoryItem)obj;
			
			writer.WriteProperty("itemName", instance.itemName, ES3Type_string.Instance);
			writer.WriteProperty("amount", instance.amount, ES3Type_float.Instance);
			writer.WriteProperty("maxAmount", instance.maxAmount, ES3Type_float.Instance);
			writer.WriteProperty("isStrain", instance.isStrain, ES3Type_bool.Instance);
			writer.WriteProperty("locked", instance.locked, ES3Type_bool.Instance);
			writer.WriteProperty("itemID", instance.itemID, ES3Type_string.Instance);
			writer.WritePropertyByRef("previousParent", instance.previousParent);
			writer.WritePropertyByRef("targetParent", instance.targetParent);
			writer.WritePropertyByRef("currentParent", instance.currentParent);
			writer.WritePropertyByRef("uiName", instance.uiName);
			writer.WritePropertyByRef("uiAmount", instance.uiAmount);
			writer.WritePrivateFieldByRef("cg", instance);
			writer.WritePrivateFieldByRef("inventoryCanvas", instance);
			writer.WritePrivateFieldByRef("rectTransform", instance);
			writer.WritePrivateFieldByRef("inventoryGUI", instance);
			writer.WritePrivateFieldByRef("hoverInfo", instance);
			writer.WritePrivateFieldByRef("strainProfile", instance);
			writer.WritePrivateFieldByRef("seedDropDown", instance);
			writer.WritePrivateFieldByRef("inventoryController", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (InventoryItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "itemName":
						instance.itemName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "amount":
						instance.amount = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "maxAmount":
						instance.maxAmount = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "isStrain":
						instance.isStrain = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "locked":
						instance.locked = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "itemID":
						instance.itemID = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "previousParent":
						instance.previousParent = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "targetParent":
						instance.targetParent = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "currentParent":
						instance.currentParent = reader.Read<UnityEngine.Transform>(ES3Type_Transform.Instance);
						break;
					case "uiName":
						instance.uiName = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "uiAmount":
						instance.uiAmount = reader.Read<UnityEngine.UI.Text>(ES3Type_Text.Instance);
						break;
					case "cg":
					reader.SetPrivateField("cg", reader.Read<UnityEngine.CanvasGroup>(), instance);
					break;
					case "inventoryCanvas":
					reader.SetPrivateField("inventoryCanvas", reader.Read<UnityEngine.Canvas>(), instance);
					break;
					case "rectTransform":
					reader.SetPrivateField("rectTransform", reader.Read<UnityEngine.RectTransform>(), instance);
					break;
					case "inventoryGUI":
					reader.SetPrivateField("inventoryGUI", reader.Read<InventoryGUI>(), instance);
					break;
					case "hoverInfo":
					reader.SetPrivateField("hoverInfo", reader.Read<HoverInfo>(), instance);
					break;
					case "strainProfile":
					reader.SetPrivateField("strainProfile", reader.Read<StrainProfile>(), instance);
					break;
					case "seedDropDown":
					reader.SetPrivateField("seedDropDown", reader.Read<SeedDropDown>(), instance);
					break;
					case "inventoryController":
					reader.SetPrivateField("inventoryController", reader.Read<InventoryController>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_InventoryItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_InventoryItemArray() : base(typeof(InventoryItem[]), ES3UserType_InventoryItem.Instance)
		{
			Instance = this;
		}
	}
}