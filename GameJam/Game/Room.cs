using System.Collections.Generic;

namespace GameJam.Game
{
    public class Room
    {
        public Tile[][] tiles;
    
        internal int roomx;
        internal int roomy;
        public Tile[] GetAllTiles()
        {
            List<Tile> allTiles = new List<Tile>();

            int yLength = tiles.Length;
            for (int y = 0; y < yLength; y++)
            {
                int xLength = tiles[y].Length;
                for (int x = 0; x < xLength; x++)
                {
                    allTiles.Add(tiles[y][x]);
                }
            }

            return allTiles.ToArray();
        }
    }
}



