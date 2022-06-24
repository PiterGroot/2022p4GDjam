using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameJam.Game
{
    class Nuke
    {
        private GameContext context;
        private Random rnd = new Random();
        public Nuke(GameContext gc, RenderForm rf)
        {
            context = gc;

            Tile[] tiles = gc.room.GetAllTiles();
            Tile randomTile = tiles[rnd.Next(0, tiles.Length)];
      
            RenderObject nukeSign = new RenderObject()
            {
                frames = gc.spriteMap.GetNukeSignFrames(),
                rectangle = new Rectangle(randomTile.rectangle.X, randomTile.rectangle.Y, 55, 55)
            };
            gc.nukeSigns.Add(nukeSign);
            Timer spawnTimer = new Timer();
            spawnTimer.Interval = (3500);
            spawnTimer.Tick += (sender, e) => SpawnNuke(spawnTimer, randomTile.rectangle.X, randomTile.rectangle.Y, nukeSign, gc);
            spawnTimer.Start();
        }

        private void SpawnNuke(Timer spawnTimer, int x, int y, RenderObject nukeSign, GameContext gc)
        {
            gc.nukeSigns.Remove(nukeSign);
            spawnTimer.Dispose();
            AudioManager.PlaySound(Properties.Resources.explosion);
            RenderObject nuke = new RenderObject()
            {
                frames = context.GetSingeFrameArray('Q'),
                rectangle = new Rectangle(x - 16, y - 16, 80, 82),
            };
            context.nukes.Add(nuke);
            context.SetControllerVibration(65000);
           
            Timer despawnTimer = new Timer();
            despawnTimer.Interval = (2500);
            despawnTimer.Tick += (sender, e) => DespawnNuke(spawnTimer, gc, nuke);
            despawnTimer.Start();

            HandleExplosion(new Vector2(x, y), gc);
        }

        private void HandleExplosion(Vector2 center, GameContext gc)
        {
            center.x += gc.tileSize;
            center.y += gc.tileSize;
            //center segment
            //center
            DestroyTile(GetTileInfo(center, gc), center, gc);
            //mid center
            DestroyTile(GetTileInfo(new Vector2(center.x, center.y - 16), gc), new Vector2(center.x, center.y -16), gc);
            //top center
            DestroyTile(GetTileInfo(new Vector2(center.x, center.y - 32), gc), new Vector2(center.x, center.y - 32), gc);
            //down center mid
            DestroyTile(GetTileInfo(new Vector2(center.x, center.y + 16), gc), new Vector2(center.x, center.y + 16), gc);
            //low center
            DestroyTile(GetTileInfo(new Vector2(center.x, center.y + 32), gc), new Vector2(center.x, center.y + 32), gc);

            //left most segment
            DestroyTile(GetTileInfo(new Vector2(center.x - 32, center.y), gc), new Vector2(center.x - 32, center.y), gc);
            //mid center
            DestroyTile(GetTileInfo(new Vector2(center.x -32, center.y - 16), gc), new Vector2(center.x - 32, center.y - 16), gc);
            //top center
            DestroyTile(GetTileInfo(new Vector2(center.x -32, center.y - 32), gc), new Vector2(center.x -32, center.y - 32), gc);
            //down center mid
            DestroyTile(GetTileInfo(new Vector2(center.x - 32, center.y + 16), gc), new Vector2(center.x -32, center.y + 16), gc);
            //low center
            DestroyTile(GetTileInfo(new Vector2(center.x -32, center.y + 32), gc), new Vector2(center.x -32, center.y + 32), gc);

            //left segment
            DestroyTile(GetTileInfo(new Vector2(center.x - 16, center.y), gc), new Vector2(center.x - 16, center.y), gc);
            //mid center
            DestroyTile(GetTileInfo(new Vector2(center.x - 16, center.y - 16), gc), new Vector2(center.x - 16, center.y - 16), gc);
            //top center
            DestroyTile(GetTileInfo(new Vector2(center.x - 16, center.y - 32), gc), new Vector2(center.x - 16, center.y - 32), gc);
            //down center mid
            DestroyTile(GetTileInfo(new Vector2(center.x - 16, center.y + 16), gc), new Vector2(center.x - 16, center.y + 16), gc);
            //low center
            DestroyTile(GetTileInfo(new Vector2(center.x - 16, center.y + 32), gc), new Vector2(center.x - 16, center.y + 32), gc);

            //center right segment
            //center
            DestroyTile(GetTileInfo(new Vector2(center.x + 16, center.y), gc), new Vector2(center.x + 16, center.y), gc);
            //mid center
            DestroyTile(GetTileInfo(new Vector2(center.x + 16, center.y - 16), gc), new Vector2(center.x + 16, center.y - 16), gc);
            //top center
            DestroyTile(GetTileInfo(new Vector2(center.x + 16, center.y - 32), gc), new Vector2(center.x + 16, center.y - 32), gc);
            //down center mid
            DestroyTile(GetTileInfo(new Vector2(center.x + 16, center.y + 16), gc), new Vector2(center.x + 16, center.y + 16), gc);
            //low center
            DestroyTile(GetTileInfo(new Vector2(center.x + 16, center.y + 32), gc), new Vector2(center.x + 16, center.y + 32), gc);

            //center most right segment
            //center
            DestroyTile(GetTileInfo(new Vector2(center.x + 32, center.y), gc), new Vector2(center.x + 32, center.y), gc);
            //mid center
            DestroyTile(GetTileInfo(new Vector2(center.x + 32, center.y - 16), gc), new Vector2(center.x + 32, center.y - 16), gc);
            //top center
            DestroyTile(GetTileInfo(new Vector2(center.x + 32, center.y - 32), gc), new Vector2(center.x + 32, center.y - 32), gc);
            //down center mid
            DestroyTile(GetTileInfo(new Vector2(center.x + 32, center.y + 16), gc), new Vector2(center.x + 32, center.y + 16), gc);
            //low center
            DestroyTile(GetTileInfo(new Vector2(center.x + 32, center.y + 32), gc), new Vector2(center.x + 32, center.y + 32), gc);

        }
        private void DestroyTile(Tile tile, Vector2 position, GameContext gc)
        {
            if (tile == null) return;
            bool destroyWall = false;
            if (rnd.Next(1, 101) <= 15) destroyWall = true;
            if (!destroyWall)
            {
                if (tile.graphic != 'W' && tile.graphic != '#')
                {
                    if (tile.graphic == ',' && GetRandomChance(5))
                    {
                        new Powerup(gc, new Vector2(tile.rectangle.X, tile.rectangle.Y));
                    }

                    tile.sprite = gc.spriteMap.GetSprite('.');
                    tile.graphic = 'K';

                    Timer resetTileTimer = new Timer();
                    resetTileTimer.Interval = (2500);
                    resetTileTimer.Tick += (sender, e) => ResetFloorTiles(resetTileTimer, tile);
                    resetTileTimer.Start();
                }
            }
            else
            {
                if (tile.graphic != 'W')
                {
                    System.Threading.Thread.Sleep(2);

                    tile.sprite = gc.spriteMap.GetSprite('.');
                    tile.graphic = 'K';

                    Timer resetTileTimer = new Timer();
                    resetTileTimer.Interval = (2500);
                    resetTileTimer.Tick += (sender, e) => ResetFloorTiles(resetTileTimer, tile);
                    resetTileTimer.Start();
                }
            }
        }
        private bool GetRandomChance(float chance)
        {
            float num = rnd.Next(0, 101);
            if (num <= chance) return true;
            else return false;
        }

        private void ResetFloorTiles(Timer timer, Tile tile)
        {
            timer.Dispose();
            tile.graphic = '.';
        }

        private Tile GetTileInfo(Vector2 position, GameContext gc)
        {
            return gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)position.x, (int)position.y))).FirstOrDefault();
        }
        private void DespawnNuke(Timer despawnTimer, GameContext gc, RenderObject nuke)
        {
            despawnTimer.Dispose();
            gc.nukes.Remove(nuke);
            context.SetControllerVibration(0);
        }
    }
}
