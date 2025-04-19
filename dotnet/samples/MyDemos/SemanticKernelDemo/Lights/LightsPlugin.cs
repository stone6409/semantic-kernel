﻿using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernelDemo.Lights
{
    public class LightsPlugin
    {
        // Mock data for the lights
        private readonly List<LightModel> lights = new()
       {
          new LightModel { Id = 1, Name = "Table Lamp", IsOn = false },
          new LightModel { Id = 2, Name = "Porch light", IsOn = false },
          new LightModel { Id = 3, Name = "Chandelier", IsOn = true }
       };

        [KernelFunction("get_lights")]
        [Description("Gets a list of lights and their current state")]
        public async Task<List<LightModel>> GetLightsAsync()
        {
            return lights;
        }

        [KernelFunction("change_state")]
        [Description("Changes the state of the light")]
        public async Task<LightModel?> ChangeStateAsync(int id, bool isOn)
        {
            var light = lights.FirstOrDefault(light => light.Id == id);

            if (light == null)
            {
                return null;
            }

            // Update the light with the new state
            light.IsOn = isOn;

            return light;
        }
    }
}
