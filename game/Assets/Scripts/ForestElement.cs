using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Elements
{
    public enum ForestElementType{
        EMPTY = 0,
        BURNEDTREE = 1,
        TREE = 2,
        FIRE = 3,
        UNIT = 4
    }

    public struct ForestElement {
        public ForestElementType Type;
        public float Healt;
    }
}