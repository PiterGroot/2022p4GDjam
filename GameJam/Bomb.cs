using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameJam.Tools;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;
using MathNet;
using GameJam.Game;

namespace GameJam
{
    class Bomb
    {
        SoundPlayer soundPlayer = new SoundPlayer(Properties.Resources.bomb_place);
        private GameContext gc;
        private RenderObject bombObj;
        public Vector2 bombPos;
        public Vector2 playerPos;
        public Bomb(GameContext gc, int miliSeconds, Vector2 placePos)
        {
            soundPlayer.Play();
            RenderObject newBomb = new RenderObject();
            newBomb = new RenderObject()
            {
                frames = gc.spriteMap.GetBombFrames(),
                rectangle = new Rectangle((int)gc.player.rectangle.X, (int)gc.player.rectangle.Y, gc.tileSize, gc.tileSize),
            };
            gc.bombs.Add(newBomb);
            StartTimer(miliSeconds, newBomb, gc, placePos);
        }

        public void StartTimer(int ms, RenderObject bomb, GameContext _gc, Vector2 _playerPos) {
            bombObj = bomb;
            gc = _gc;
            bombPos = _playerPos;
            playerPos = _playerPos;

            Timer MyTimer = new Timer();
            MyTimer.Interval = (ms);
            MyTimer.Tick += (sender, e) => BombTimer(MyTimer);
            MyTimer.Start();
            Debug.WriteLine(gc.bombs.Count);
        }
        
        private void BombTimer(Timer timer) {
            timer.Dispose();
            gc.bombs.Remove(bombObj);
            OnBombExplode();
        }
        private void OnBombExplode()
        {
            Console.WriteLine("BOMB EXPLODED");
            // TODO: Make new bomb explosion detection
        }
    }
}
