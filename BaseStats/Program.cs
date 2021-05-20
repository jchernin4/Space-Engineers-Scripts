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
        private string prefix = "[BS]";

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update10; // Update every 10th tick
        }

        public void Main(string argument, UpdateType updateSource) {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            List<IMyTextPanel> lcds = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocks(blocks); // Get every connected block

            List<IMyTextPanel> allLcds = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(allLcds);

            foreach (IMyTextPanel lcd in allLcds) {
                if (lcd != null && lcd.CustomName.StartsWith(prefix)) {
                    lcds.Add(lcd);
                }
            }

            int numConnected = 0;
            List<IMyShipConnector> connectors = new List<IMyShipConnector>();
            GridTerminalSystem.GetBlocksOfType(connectors);
            foreach (IMyShipConnector connector in connectors) {
                if (connector.Status.Equals(MyShipConnectorStatus.Connected)) {
                    numConnected++;
                }
            }

            float currentTotalOutput = 0;
            float maxTotalOutput = 0;
            List<IMyPowerProducer> powerProducers = new List<IMyPowerProducer>();
            GridTerminalSystem.GetBlocksOfType(powerProducers);
            foreach (IMyPowerProducer producer in powerProducers) {
                currentTotalOutput += producer.CurrentOutput;
                maxTotalOutput += producer.MaxOutput;
            }
            
            foreach (IMyTextPanel lcd in lcds) {
                lcd.WriteText("Base Stats\n==================================\n");
                
                lcd.WriteText("Connected Ships: " + numConnected + " / " + connectors.Count + "\n", true);
                
                lcd.WriteText("Power: " + Math.Round(currentTotalOutput * 1000, 2) + " / " + Math.Round(maxTotalOutput * 1000, 2) + "kW", true);
                lcd.WriteText(" (" + Math.Round(currentTotalOutput / maxTotalOutput * 100, 2) + "% of max)", true);
            }
        }
    }
}