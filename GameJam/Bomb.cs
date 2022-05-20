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
        private GameContext gc;
        private RenderObject bombObj;
        public Vector2 bombPos;
        public Action onFinish;
        public void StartTimer(int ms, RenderObject bomb, GameContext _gc, Vector2 _playerPos) {
            bombObj = bomb;
            gc = _gc;
            bombPos = _playerPos;
    
            Timer MyTimer = new Timer();
            MyTimer.Interval = (ms);
            MyTimer.Tick += (sender, e) => BombTimer(MyTimer);
            MyTimer.Start();
            Debug.WriteLine(gc.bombs.Count);
        }
        
        private void BombTimer(Timer timer) {
            timer.Dispose();
            onFinish.Invoke();
            gc.bombs.Remove(bombObj);
            Debug.WriteLine(gc.bombs.Count);
        }

    }
}
