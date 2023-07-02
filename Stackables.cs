using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace Stackables {

	
	public class Stackables : Mod {
		
		public override void Load() {
			IL_ItemSlot.RightClick_ItemArray_int_int += rightClickIlEdit;
		}
		
		public override void Unload() {
			IL_ItemSlot.RightClick_ItemArray_int_int -= rightClickIlEdit;
		}
		
		private static void rightClickIlEdit(ILContext context) {
			var cursor = new ILCursor(context);
			var maxStackRef = typeof(Item).GetField("maxStack");
			var stackRef = typeof(Item).GetField("stack");
			if (maxStackRef == null || stackRef == null) throw new Exception();
			while (cursor.TryGotoNext(i => i.MatchLdfld(maxStackRef))) {
				cursor.Remove();
				cursor.Emit(OpCodes.Ldfld, stackRef);
			}
		}
		
	}
	
	
	
	class StackablesGlobalItem : GlobalItem {
		
		public override void SetDefaults(Item item) {
			base.SetDefaults(item);
			if (item.IsACoin || !(item.accessory || item.defense <= 0 || item.damage <= 0)) return;
			item.maxStack = 9999;
			item.AllowReforgeForStackableItem = true;
		}
		
	}
	
	
}