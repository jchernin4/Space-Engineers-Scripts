﻿using Sandbox.Game.EntityComponents;
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
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // In order to add a new utility class, right-click on your project, 
        // select 'New' then 'Add Item...'. Now find the 'Space Engineers'
        // category under 'Visual C# Items' on the left hand side, and select
        // 'Utility Class' in the main area. Name it in the box below, and
        // press OK. This utility class will be merged in with your code when
        // deploying your final script.
        //
        // You can also simply create a new utility class manually, you don't
        // have to use the template if you don't want to. Just do so the first
        // time to see what a utility class looks like.
        // 
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1; // Update every tick
        }

        public void Main(string argument, UpdateType updateSource) {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();

            GridTerminalSystem.GetBlocksOfType<IMyShipConnector>(blocks);
            IMyShipConnector connector = blocks[0] as IMyShipConnector;

            blocks.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(blocks);
            IMyTextPanel lcd = blocks[0] as IMyTextPanel;
            
            blocks.Clear();
            GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(blocks);
            IMyBatteryBlock battery = blocks[0] as IMyBatteryBlock;
            
            blocks.Clear();
            GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(blocks);
            IMySolarPanel solarPanel = blocks[0] as IMySolarPanel;

            if (lcd != null) {
                lcd.WriteText("");
                if (connector != null) {
                    lcd.WriteText("Connector status: " + connector.Status, true);
                }

                if (battery != null) {
                    lcd.WriteText("\nBattery: " + Math.Round(battery.CurrentStoredPower / battery.MaxStoredPower * 100, 2) + "%", true);
                }

                if (solarPanel != null) {
                    lcd.WriteText("\nSolar Panel Output: " + Math.Round(solarPanel.CurrentOutput * 1000, 2) + " kW", true);

                    if (solarPanel.MaxOutput > 0) { 
                        lcd.WriteText(" (" + Math.Round(solarPanel.CurrentOutput / solarPanel.MaxOutput * 100, 2) + "% of max)", true);
                    } else {
                        lcd.WriteText(" (100%, nighttime)", true);
                    }
                }
            }
        }
    }
}