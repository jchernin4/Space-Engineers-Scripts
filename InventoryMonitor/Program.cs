using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript {
	partial class Program : MyGridProgram {

		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Update10; // Update every 10th tick
		}

		public void Main(string argument, UpdateType updateSource) {
			List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocks(blocks); // Get every connected block

			// For storing item amounts
			Dictionary<MyItemType, MyFixedPoint> totalBlocks = new Dictionary<MyItemType, MyFixedPoint>();
			
			// Check every block
			foreach (IMyTerminalBlock block in blocks) {
				// Check every inventory
				for (int i = 0; i < block.InventoryCount; i++) {
					IMyInventory curInv = block.GetInventory(i);

					for (int j = 0; j < curInv.ItemCount; j++) {
						MyInventoryItem item = curInv.GetItemAt(j).Value;
						
						if (!totalBlocks.ContainsKey(item.Type)) {
							totalBlocks.Add(item.Type, item.Amount);
							
						} else {
							totalBlocks[item.Type] = totalBlocks[item.Type] + item.Amount;
						}
					}
				}
			}
			
			blocks.Clear();
			GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(blocks);
			IMyTextPanel lcd = blocks[0] as IMyTextPanel;

			if (lcd != null) {
				lcd.WriteText("");
				foreach (MyItemType type in totalBlocks.Keys) {
					lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
				}
			}
		}
	}
}
