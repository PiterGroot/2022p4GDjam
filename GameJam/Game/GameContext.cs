namespace GameJam.Game
{
    using GameJam.Tools;
    using GameJam;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    public class GameContext {

        internal List<RenderObject> bombs = new List<RenderObject>();
        internal int scaleunit = 4;

        internal int tileSize = 16;
        internal RenderObject player = new RenderObject();
        internal RenderObject player1 = new RenderObject();
        internal SpriteMap spriteMap = new SpriteMap();
        internal Room room;

        
    }

}