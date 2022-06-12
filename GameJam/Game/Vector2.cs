using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam
{
    class Vector2
    {
        public float x;
        public float y;
        public Vector2(float _x, float _y) {
            x = _x;
            y = _y;
        }
        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
    }
}
