using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game_Mechanics
{
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] toolControllers;

        private Dictionary<string, IToolController> tools = new();

        public void Start()
        {
            foreach (var tool in toolControllers)
            {
                if (!tool.TryGetComponent<IToolController>(out var controller))
                    throw new Exception($"{tool.name} does NOT have IToolController!");
                
                tools.Add(controller.ToolId, controller);
                controller.SetIsEquipped(false);
            }

            if (tools.Count == 0) throw new Exception("No tool detected???");

            tools.First().Value.SetIsEquipped(true);
        }

        public void Equip(string toolId)
        {
            if (!tools.ContainsKey(toolId)) throw new Exception($"TRYING TO EQUIP ID {toolId} BUT DOES NOT EXIST");

            foreach (var (id, tool) in tools)
                tool.SetIsEquipped(id.Equals(toolId));
        }
    }
}