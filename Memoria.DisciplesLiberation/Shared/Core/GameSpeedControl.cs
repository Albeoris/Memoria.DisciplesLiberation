using System;
using Memoria.Disciples.IL2CPP;
using UnityEngine;

namespace Memoria.Disciples.Core
{
    public sealed class GameSpeedControl
    {
        public GameSpeedControl()
        {
        }

        private Boolean _isDisabled;
        private Boolean _isToggled;
        private Single _speedFactor = Time.timeScale;

        public void Update()
        {
            try
            {
                if (_isDisabled)
                    return;

                ProcessSpeedUp();
            }
            catch (Exception ex)
            {
                _isDisabled = true;
                ModComponent.Log.LogError($"[{nameof(GameSpeedControl)}].{nameof(Update)}(): {ex}");
            }
        }

        private void ProcessSpeedUp()
        {
            Single currentFactor = Time.timeScale;
            if (currentFactor == 0.0f) // Do not unpause the game 
                return;

            var config = ModComponent.Instance.Config;

            var toggleFactor = config.Speed.ToggleFactor.Value;
            var toggleKey = config.Speed.ToggleKey.Value;

            Boolean isToggled = InputManager.GetKeyUp(toggleKey);
            Single speedFactor = 0.0f;

            if (isToggled)
            {
                if (!_isToggled)
                    speedFactor = Math.Max(speedFactor, toggleFactor);

                _isToggled = !_isToggled;
            }

            if (speedFactor == 0.0f)
            {
                speedFactor = _isToggled ? _speedFactor : 1.0f;
            }
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (currentFactor != speedFactor)
            {
                Time.timeScale = speedFactor;
                _speedFactor = speedFactor;
            }
        }
    }
}