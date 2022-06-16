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
        private Vector2 spawnPos;
        private GameContext context;
        private Random rnd = new Random();
        public Nuke(GameContext gc, Vector2 position)
        {
            spawnPos = position;
            context = gc;

            Tile[] tiles = gc.room.GetAllTiles();
            Tile randomTile = tiles[rnd.Next(0, tiles.Length)];
      
            RenderObject nukeSign = new RenderObject()
            {
                frames = gc.spriteMap.GetNukeSignFrames(),
                rectangle = new Rectangle(randomTile.rectangle.X +8, randomTile.rectangle.Y + 8, 35, 35)
            };
            gc.nukeSigns.Add(nukeSign);

            Timer spawnTimer = new Timer();
            spawnTimer.Interval = (2500);
            spawnTimer.Tick += (sender, e) => SpawnNuke(spawnTimer, randomTile.rectangle.X, randomTile.rectangle.Y, nukeSign, gc);
            spawnTimer.Start();
        }

        private void SpawnNuke(Timer spawnTimer, int x, int y, RenderObject nukeSign, GameContext gc)
        {
            gc.nukeSigns.Remove(nukeSign);
            spawnTimer.Dispose();
            RenderObject nuke = new RenderObject()
            {
                frames = context.GetSingeFrameArray('Q'),
                rectangle = new Rectangle(x - 16, y - 16, 80, 82),
            };
            context.nukes.Add(nuke);

            Timer despawnTimer = new Timer();
            despawnTimer.Interval = (2500);
            despawnTimer.Tick += (sender, e) => DespawnNuke(spawnTimer, gc, nuke);
            despawnTimer.Start();
        }
        private void DespawnNuke(Timer despawnTimer, GameContext gc, RenderObject nuke)
        {
            despawnTimer.Dispose();
            gc.nukes.Remove(nuke);
        }
    }
}
