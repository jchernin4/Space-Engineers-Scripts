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
using Sandbox.Game.Entities;
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
		float curProduction = 0;
		private string prefix = "[SPR]";
		
		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Update10; // Update every 10th tick
		}

		public void Save() {
			Storage = curProduction.ToString();
		}

		public void Main(string argument, UpdateType updateSource) {
			List<IMyTerminalBlock> allRotors = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocksOfType<IMyMotorRotor>(allRotors);
			List<IMySolarPanel> allPanels = new List<IMySolarPanel>();
			GridTerminalSystem.GetBlocksOfType(allPanels);
			
			List<IMySolarPanel> panels = new List<IMySolarPanel>();
			foreach (IMySolarPanel panel in allPanels) {
				if (panel.CustomName.EndsWith(prefix)) {
					panels.Add(panel);
				}
			}
			
			List<IMyTerminalBlock> rotors = new List<IMyTerminalBlock>();
			foreach (IMyTerminalBlock rotor in allRotors) {
				Echo("Display name: " + rotor.DisplayName);
				if (rotor.DisplayName.EndsWith(prefix)) {
					rotors.Add(rotor);
				}
			}

			curProduction = 0;
			foreach (IMySolarPanel panel in panels) {
				curProduction += panel.CurrentOutput;
			}

			if (Storage != null && curProduction < float.Parse(Storage)) {
				foreach (IMyTerminalBlock rotor in rotors) {
					IMyMotorStator stator = (IMyMotorStator) rotor;
					float angle = (stator.Angle / (float) Math.PI * 180f);
					stator.SetValue<float>("UpperLimit", angle + 20);
					stator.SetValue<float>("LowerLimit", angle - 20);
					stator.SetValue<float>("Velocity", 3);
				}
			}
		}
	}
}
