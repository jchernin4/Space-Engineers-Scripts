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

					List <IMyInventoryItem> curItems = new List<IMyInventoryItem>();
					for (int j = 0; j < curInv.Itemcount; j++) {
						IMyInventoryItem item = curInv.GetItemAt(j);
						
						if (!totalBlocks.ContainsKey(item.Type)) {
							totalBlocks.Add(item.Type, item.Amount);
							
						} else {
							totalBlocks[item.Type] = totalBlocks[item.Type] + item.Amount;
						}
					}
				}
			}
		}
	}
}
