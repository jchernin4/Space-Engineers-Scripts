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
        private string prefix = "[IV]";

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

            List<IMyTextPanel> oresPanels = new List<IMyTextPanel>();
            List<IMyTextPanel> ingotsPanels = new List<IMyTextPanel>();
            List<IMyTextPanel> componentsPanels = new List<IMyTextPanel>();
            List<IMyTextPanel> otherPanels = new List<IMyTextPanel>();
            List<IMyTextPanel> allPanels = new List<IMyTextPanel>();

            foreach (IMyTerminalBlock block in blocks) {
                IMyTextPanel lcd = block as IMyTextPanel;
                if (lcd != null && lcd.CustomName.StartsWith(prefix)) {
                    switch (lcd.CustomName.ToLower().Split(' ')[1]) {
                        case "ores":
                            oresPanels.Add(lcd);
                            break;

                        case "ingots":
                            ingotsPanels.Add(lcd);
                            break;

                        case "components":
                            componentsPanels.Add(lcd);
                            break;

                        case "other":
                            otherPanels.Add(lcd);
                            break;

                        case "all":
                            allPanels.Add(lcd);
                            break;
                    }
                }
            }

            foreach (IMyTextPanel lcd in oresPanels) {
                lcd.WriteText("Ores\n==================================\n");
                foreach (MyItemType type in totalBlocks.Keys) {
                    if (type.GetItemInfo().IsOre) {
                        lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
                    }
                }
            }

            foreach (IMyTextPanel lcd in ingotsPanels) {
                lcd.WriteText("Ingots\n==================================\n");
                foreach (MyItemType type in totalBlocks.Keys) {
                    if (type.GetItemInfo().IsIngot) {
                        lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
                    }
                }
            }

            foreach (IMyTextPanel lcd in componentsPanels) {
                lcd.WriteText("Components\n==================================\n");
                foreach (MyItemType type in totalBlocks.Keys) {
                    if (type.GetItemInfo().IsComponent) {
                        lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
                    }
                }
            }

            foreach (IMyTextPanel lcd in otherPanels) {
                lcd.WriteText("Other\n==================================\n");
                foreach (MyItemType type in totalBlocks.Keys) {
                    if (type.GetItemInfo().IsAmmo || type.GetItemInfo().IsTool) {
                        lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
                    }
                }
            }

            foreach (IMyTextPanel lcd in allPanels) {
                lcd.WriteText("All\n==================================\n");
                foreach (MyItemType type in totalBlocks.Keys) {
                    lcd.WriteText(type.SubtypeId + " - " + totalBlocks[type] + "\n", true);
                }
            }
        }
    }
}