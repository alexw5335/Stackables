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
			//IL_WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort += noPrefix;
		}
		
		public override void Unload() {
			IL_ItemSlot.RightClick_ItemArray_int_int -= rightClickIlEdit;
			//IL_WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort -= noPrefix;
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

		/*private static void noPrefix(ILContext context) {
			var cursor = new ILCursor(context);
			var prefixRef = typeof(Item).GetMethod("Prefix");
			if (prefixRef == null) throw new Exception("Prefix reference not found");
			while (cursor.TryGotoNext(i => i.MatchCallvirt(prefixRef))) {
				if (cursor.Prev.OpCode != OpCodes.Ldc_I4_M1) continue;
				cursor.GotoPrev();
				cursor.Remove();
				cursor.Emit(OpCodes.Ldc_I4_0);
			}
		}*/
	}
	
	class StackablesGlobalItem : GlobalItem {
		public override void SetDefaults(Item item) {
			base.SetDefaults(item);
			if (item.IsACoin || !(item.accessory || item.defense <= 0 || item.damage <= 0)) return;
			item.maxStack = 9999;
			item.AllowReforgeForStackableItem = true;
		}
		
		//public override void OnSpawn(Item item, IEntitySource source) => item.prefix = 0;
	}
	
}