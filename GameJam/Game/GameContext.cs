namespace GameJam.Game
{
    using GameJam.Tools;
    using GameJam;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using SharpDX.XInput;

    public class GameContext {

        public Controller controller;
        internal Vibration vibration = new Vibration();
        internal int vibrationLeftMotorSpeed;
        internal float p1BombCount =3;
        internal float p2BombCount =3;
        internal List<RenderObject> bombs = new List<RenderObject>();
        internal List<RenderObject> explosionTiles = new List<RenderObject>();
        internal List<RenderObject> powerUps = new List<RenderObject>();
        internal int scaleunit = 3;
        internal string winner = "entity";
        internal int tileSize = 16;
        internal RenderObject player = new RenderObject();
        internal RenderObject player1 = new RenderObject();
        internal RenderObject p1Heart = new RenderObject();
        internal RenderObject p2Heart = new RenderObject();
        internal RenderObject nukeSign = new RenderObject();
        internal SpriteMap spriteMap = new SpriteMap();
        internal Room room;
        
        internal Rectangle[] GetSingeFrameArray(char singleSpriteGraphic)
        {
            Rectangle[] singleFrameArray = new Rectangle[1];
            singleFrameArray[0] = spriteMap.GetSprite(singleSpriteGraphic);
            return singleFrameArray;
        }
        internal void ReloadBombs()
        {
            Timer MyTimer = new Timer();
            MyTimer.Interval = (5000);
            MyTimer.Tick += (sender, e) => ReloadNow(MyTimer);
            MyTimer.Start();
        }
        internal void ReloadNow(Timer mytimer)
        {
            mytimer.Dispose();
            if (p1BombCount <= 2)
            {
                p1BombCount++;
            }
            if(p2BombCount <= 2)
            {
                p2BombCount++;
            }
            ReloadBombs();
        }

        internal Rectangle GetCurrentTileRectangle(Vector2 pos)
        {
            return new Rectangle((int)pos.x, (int)pos.y, tileSize, tileSize);
        }
        internal void SetRenderScale(int amount)
        {
            scaleunit += amount;
            if(scaleunit == 1)
            {
                scaleunit = 2;
            }
            else if (scaleunit == 10)
            {
                scaleunit = 9;
            }
        }
    }

}