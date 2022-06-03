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
        public Vector2 playerPos;
        public Bomb(GameContext gc, int miliSeconds, Vector2 placePos, bool wichPlayer)
        {
            AudioManager.PlaySound(Properties.Resources.bomb_place);
            RenderObject newBomb = new RenderObject();

            if (wichPlayer)
            {
                newBomb = new RenderObject()
                {
                    frames = gc.spriteMap.GetBombFrames(),
                    rectangle = new Rectangle((int)gc.player.rectangle.X, (int)gc.player.rectangle.Y, gc.tileSize, gc.tileSize),
                };
            }
            else
            {
                newBomb = new RenderObject()
                {
                    frames = gc.spriteMap.GetBombFrames(),
                    rectangle = new Rectangle((int)gc.player1.rectangle.X, (int)gc.player1.rectangle.Y, gc.tileSize, gc.tileSize),
                };
            }
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
        }
        
        private void BombTimer(Timer timer) {
            timer.Dispose();
            gc.bombs.Remove(bombObj);
            OnBombExplode();
        }

        private void OnBombExplode()
        {
            AudioManager.PlaySound(Properties.Resources.explosion);
            Console.WriteLine("BOMB EXPLODED" + Properties.Resources.explosion);

            CreateExplosion();
            
        }

        private void CreateExplosion()
        {
            Rectangle center = gc.GetCurrentTileRectangle(playerPos);

            //place core
            if (playerPos == null) return;
            RenderObject coreExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('x'),
                rectangle = gc.GetCurrentTileRectangle(playerPos),
            };
            //go up
            CheckTile(new Vector2(center.X, center.Y - gc.tileSize));
            RenderObject upExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('I'),
                rectangle = new Rectangle(center.X, center.Y - gc.tileSize, gc.tileSize, gc.tileSize),
            };
            CheckTile(new Vector2(center.X, center.Y - gc.tileSize * 2));
            RenderObject upRoofExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('^'),
                rectangle = new Rectangle(center.X, center.Y - gc.tileSize * 2, gc.tileSize, gc.tileSize),
            };

            //go right
            CheckTile(new Vector2(center.X + gc.tileSize, center.Y));
            RenderObject rightExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('-'),
                rectangle = new Rectangle(center.X + gc.tileSize, center.Y, gc.tileSize, gc.tileSize),
            };

            CheckTile(new Vector2(center.X + gc.tileSize * 2, center.Y));
            RenderObject rightRoofExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('>'),
                rectangle = new Rectangle(center.X + gc.tileSize * 2, center.Y, gc.tileSize, gc.tileSize),
            };

            //go down
            CheckTile(new Vector2(center.X, center.Y + gc.tileSize));
            RenderObject downExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('I'),
                rectangle = new Rectangle(center.X, center.Y + gc.tileSize, gc.tileSize, gc.tileSize),
            };

            CheckTile(new Vector2(center.X, center.Y + gc.tileSize * 2));
            RenderObject downRoofExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('V'),
                rectangle = new Rectangle(center.X, center.Y + gc.tileSize * 2, gc.tileSize, gc.tileSize),
            };

            //go left
            CheckTile(new Vector2(center.X - gc.tileSize, center.Y));
            RenderObject leftExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('-'),
                rectangle = new Rectangle(center.X - gc.tileSize, center.Y, gc.tileSize, gc.tileSize),
            };

            CheckTile(new Vector2(center.X - gc.tileSize * 2, center.Y));
            RenderObject leftRoofExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('<'),
                rectangle = new Rectangle(center.X - gc.tileSize * 2, center.Y, gc.tileSize, gc.tileSize),
            };

            RenderObject[] allTiles = new RenderObject[9];

            gc.explosionTiles.Add(coreExplosion);
            allTiles[0] = coreExplosion;
            gc.explosionTiles.Add(upExplosion);
            allTiles[1] = upExplosion;
            gc.explosionTiles.Add(upRoofExplosion);
            allTiles[2] = upRoofExplosion;
            gc.explosionTiles.Add(rightExplosion);
            allTiles[3] = rightExplosion;
            gc.explosionTiles.Add(rightRoofExplosion);
            allTiles[4] = rightRoofExplosion;
            gc.explosionTiles.Add(downExplosion);
            allTiles[5] = downExplosion;
            gc.explosionTiles.Add(downRoofExplosion);
            allTiles[6] = downRoofExplosion;
            gc.explosionTiles.Add(leftExplosion);
            allTiles[7] = leftExplosion;
            gc.explosionTiles.Add(leftRoofExplosion);
            allTiles[8] = leftRoofExplosion;

            Timer despawnTimer = new Timer();
            despawnTimer.Interval = (750);
            despawnTimer.Tick += (sender, e) => DespawnExplosion(despawnTimer, allTiles);
            despawnTimer.Start();
        }
        private void CheckTile(Vector2 pos)
        {
            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)pos.x, (int)pos.y))).FirstOrDefault();
            Console.WriteLine(next);
            if(next.graphic == ',')
            {
                next.graphic = '.';
                next.sprite = gc.spriteMap.GetSprite('.');
            }
        }
        private void DespawnExplosion(Timer timer, RenderObject[] tiles)
        {
            timer.Dispose();
            foreach (RenderObject explosionTile in tiles)
            {
                gc.explosionTiles.Remove(explosionTile);
            }
            
        }
    }
}
