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
        internal List<RenderObject> explosionTiles = new List<RenderObject>();
        internal int scaleunit = 4;

        internal int tileSize = 16;
        internal RenderObject player = new RenderObject();
        internal RenderObject player1 = new RenderObject();
        internal RenderObject p1Heart = new RenderObject();
        internal RenderObject p2Heart = new RenderObject();
        internal SpriteMap spriteMap = new SpriteMap();
        internal Room room;

        internal Rectangle[] GetSingeFrameArray(char singleSpriteGraphic)
        {
            Rectangle[] singleFrameArray = new Rectangle[1];
            singleFrameArray[0] = spriteMap.GetSprite(singleSpriteGraphic);
            return singleFrameArray;
        }

        internal Rectangle GetCurrentTileRectangle(Vector2 pos)
        {
            return new Rectangle((int)pos.x, (int)pos.y, tileSize, tileSize);
        }
    }

}