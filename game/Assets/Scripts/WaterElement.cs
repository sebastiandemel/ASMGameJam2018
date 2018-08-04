using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

namespace Elements {
    public class WaterElement {
        public int X;
        public int Y;

        public int Cooldown;

        private float _currentCooldown;

        /// <summary>
        /// Creates a WaterElement with coold down
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="cooldown">Cooldown in seconds (how long does it take to trigger again)</param>
        /// <returns>number of neighbors</returns>
        public WaterElement(int x, int y, int cooldown){
            X = x;
            Y = y;
            Cooldown = cooldown;
            _currentCooldown = 0.0f;
        }

        /// <summary>
        /// Return true or false depending has the cooldown reached zero
        /// </summary>
        /// <returns>number of neighbors</returns>
        public bool Update(){
            if(_currentCooldown <= 0){
                _currentCooldown = Cooldown;
                return true;
            }
            else {
                _currentCooldown -= Time.deltaTime;
                return false;
            }
        }
    }
}
