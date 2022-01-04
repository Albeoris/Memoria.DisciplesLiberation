using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace Memoria.Disciples.Core
{
    public static class InputManager
    {
        public static Boolean GetKey(KeyCode keyCode) => Check(keyCode, Input.GetKey);
        public static Boolean GetKeyDown(KeyCode keyCode) => Check(keyCode, Input.GetKeyDown);
        public static Boolean GetKeyUp(KeyCode keyCode) => Check(keyCode, Input.GetKeyUp);

        private static Boolean Check(KeyCode keyCode, Func<KeyCode, Boolean> checker)
        {
            return keyCode != KeyCode.None && checker(keyCode);
        }
    }
}