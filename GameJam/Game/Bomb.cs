using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using GameJam.Game;
using System;

namespace GameJam
{
    class Bomb
    {
        Random rnd = new Random();
        private bool canUp = true, canRight = true, canDown = true, canLeft = true;
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
            StartTimer(miliSeconds, newBomb, gc, placePos, wichPlayer);
        }

        public void StartTimer(int ms, RenderObject bomb, GameContext _gc, Vector2 _playerPos, bool player) {
            bombObj = bomb;
            gc = _gc;
            bombPos = _playerPos;
            playerPos = _playerPos;

            Timer vibrationTimer = new Timer();
            vibrationTimer.Interval = (250);
            vibrationTimer.Tick += (sender, e) => DisableVibration(vibrationTimer, player);
            vibrationTimer.Start();

            Timer MyTimer = new Timer();
            MyTimer.Interval = (ms);
            MyTimer.Tick += (sender, e) => BombTimer(MyTimer, player);
            MyTimer.Start();
        }
        private void DisableVibration(Timer timer, bool player)
        {
            timer.Dispose();
            if (!player)
            {
                gc.SetControllerVibration(0);
            }
        }
        private void BombTimer(Timer timer, bool player) {
            timer.Dispose();
            gc.bombs.Remove(bombObj);
            OnBombExplode();
        }

        private void OnBombExplode()
        {
            AudioManager.PlaySound(Properties.Resources.explosion);

            CreateExplosion();
        }

        private void CreateExplosion()
        {
            Rectangle center = gc.GetCurrentTileRectangle(playerPos);
            RenderObject[] allTiles = new RenderObject[9];

            //place core
            if (playerPos == null) return;
            RenderObject coreExplosion = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('x'),
                rectangle = gc.GetCurrentTileRectangle(playerPos),
            };
            gc.explosionTiles.Add(coreExplosion);
            allTiles[0] = coreExplosion;

            //go up
            SetTile(new Vector2(center.X, center.Y - gc.tileSize), ExplosionDirection.UP);
            if (canUp)
            {
                RenderObject upExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('I'),
                    rectangle = new Rectangle(center.X, center.Y - gc.tileSize, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(upExplosion);
                allTiles[1] = upExplosion;
            }
            SetTile(new Vector2(center.X, center.Y - gc.tileSize * 2), ExplosionDirection.UP);
            if (canUp)
            {
                RenderObject upRoofExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('^'),
                    rectangle = new Rectangle(center.X, center.Y - gc.tileSize * 2, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(upRoofExplosion);
                allTiles[2] = upRoofExplosion;
            }

            SetTile(new Vector2(center.X + gc.tileSize, center.Y), ExplosionDirection.RIGHT);
            if (canRight)
            {
                //go right
                RenderObject rightExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('-'),
                    rectangle = new Rectangle(center.X + gc.tileSize, center.Y, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(rightExplosion);
                allTiles[3] = rightExplosion;
            }

            SetTile(new Vector2(center.X + gc.tileSize * 2, center.Y), ExplosionDirection.RIGHT);
            if (canRight)
            {
                RenderObject rightRoofExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('>'),
                    rectangle = new Rectangle(center.X + gc.tileSize * 2, center.Y, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(rightRoofExplosion);
                allTiles[4] = rightRoofExplosion;
            }

            //go down
            SetTile(new Vector2(center.X, center.Y + gc.tileSize), ExplosionDirection.DOWN);
            if (canDown)
            {
                RenderObject downExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('I'),
                    rectangle = new Rectangle(center.X, center.Y + gc.tileSize, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(downExplosion);
                allTiles[5] = downExplosion;
            }

            SetTile(new Vector2(center.X, center.Y + gc.tileSize * 2), ExplosionDirection.DOWN);
            if (canDown)
            {
                RenderObject downRoofExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('V'),
                    rectangle = new Rectangle(center.X, center.Y + gc.tileSize * 2, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(downRoofExplosion);
                allTiles[6] = downRoofExplosion;
            }

            //go left
            SetTile(new Vector2(center.X - gc.tileSize, center.Y), ExplosionDirection.LEFT);
            if (canLeft)
            {
                RenderObject leftExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('-'),
                    rectangle = new Rectangle(center.X - gc.tileSize, center.Y, gc.tileSize, gc.tileSize),
                };
                gc.explosionTiles.Add(leftExplosion);
                allTiles[7] = leftExplosion;
            }

            SetTile(new Vector2(center.X - gc.tileSize * 2, center.Y), ExplosionDirection.LEFT);
            if (canLeft)
            {
                RenderObject leftRoofExplosion = new RenderObject()
                {
                    frames = gc.GetSingeFrameArray('<'),
                    rectangle = new Rectangle(center.X - gc.tileSize * 2, center.Y, gc.tileSize, gc.tileSize),
                };

                gc.explosionTiles.Add(leftRoofExplosion);
                allTiles[8] = leftRoofExplosion;
            }


            Timer despawnTimer = new Timer();
            despawnTimer.Interval = (750);
            despawnTimer.Tick += (sender, e) => DespawnExplosion(despawnTimer, allTiles);
            despawnTimer.Start();
        }

        private void SetTile(Vector2 pos, ExplosionDirection direction)
        {
            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)pos.x, (int)pos.y))).FirstOrDefault();
            if (next == null) return;
            if (next.graphic == '#' || next.graphic == 'W')
            {
                switch (direction)
                {
                    case ExplosionDirection.UP:
                        canUp = false;
                        break;
                    case ExplosionDirection.RIGHT:
                        canRight = false;
                        break;
                    case ExplosionDirection.DOWN:
                        canDown = false;
                        break;
                    case ExplosionDirection.LEFT:
                        canLeft = false;
                        break;
                }
            }
            if (next.graphic == ',' && direction == ExplosionDirection.DOWN && canDown)
            {
                DestroyTile(next);
            }
            if (next.graphic == ',' && direction == ExplosionDirection.UP && canUp)
            {
                DestroyTile(next);
            }
            if (next.graphic == ',' && direction == ExplosionDirection.LEFT && canLeft)
            {
                DestroyTile(next);
            }
            if (next.graphic == ',' && direction == ExplosionDirection.RIGHT && canRight)
            {
                DestroyTile(next);
            }
        }
        private void DestroyTile(Tile tile)
        {
            tile.graphic = '.';
            tile.sprite = gc.spriteMap.GetSprite('.');
            if(GetRandomChance(23))
            {
                new Powerup(gc, new Vector2(tile.rectangle.X, tile.rectangle.Y));
            }
        }

        private bool GetRandomChance(float chance)
        {
            float num = rnd.Next(0, 101);
            if (num <= chance) return true;
            else return false;
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
