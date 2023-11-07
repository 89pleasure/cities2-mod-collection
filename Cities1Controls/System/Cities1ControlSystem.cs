using Game;
using Game.Audio;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Tools;
using Game.UI;
using System;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace Cities1Controls.System
{
    public class Cities1ControlSystem : GameSystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            string binding = "<Keyboard>/home";
            bool developerMode = GameManager.instance.configuration.developerMode;

            if (developerMode)
            {
                UnityEngine.Debug.Log("Developer mode is enabled. Set binding to END instead.");
                binding = "<Keyboard>/end";
            }

            AddBinding(name: "ResetElevationToZero", binding: binding, callback: OnResetElevation);

            UnityEngine.Debug.Log("Cities1ControlSystem created!");

        }

        protected override void OnUpdate()
        {
        }

        private void OnResetElevation(InputAction.CallbackContext obj)
        {
            World.GetOrCreateSystemManaged<NetToolSystem>().elevation = 0f;

            var uxSoundQuery = GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
            ToolUXSoundSettingsData soundSettingsData = uxSoundQuery.GetSingleton<ToolUXSoundSettingsData>();
            AudioManager.instance.PlayUISound(soundSettingsData.m_NetElevationDownSound);

            UnityEngine.Debug.Log("[Cities1Controls Mod]: Reset elevation to 0.");
        }

        private void AddBinding(string name, string binding, Action<InputAction.CallbackContext> callback)
        {
            InputAction customInputAction = new(name: name, binding: binding);
            customInputAction.performed += callback;
            customInputAction.Enable();
            UnityEngine.Debug.Log("[Cities1Controls Mod]: Added control " + name + " with key " + binding);
        }
    }
}
