using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameJam.Game
{
    internal class SpriteMap
    {
        public static Dictionary<char, Rectangle> tileMap = new Dictionary<char, Rectangle>();
        private readonly Rectangle[] playerAnimation;
        private readonly Rectangle[] bombAnimation;
        private readonly Rectangle newTile;

        internal SpriteMap()
        {
            //selecteer meest links boven pixel
            //66 75 = bom
            //23 75 = floor
            //45 75 = wall

            tileMap.Add('#', new Rectangle(45, 75, 16, 16));
            tileMap.Add('.', new Rectangle(23, 75, 16, 16));
            tileMap.Add('!', new Rectangle(66, 75, 16, 16));

            playerAnimation = new Rectangle[]
                {
                    new Rectangle(43, 9, 16, 16),
                    new Rectangle(60, 9, 16, 16),
                    new Rectangle(77, 9, 16, 16)
                };

            bombAnimation = new Rectangle[] {
                new Rectangle(66, 75, 16, 16),
                new Rectangle(84, 75, 16, 16),
                new Rectangle(104, 75, 16, 16)
            };
        }

        internal Dictionary<char, Rectangle> GetMap()
        {
            return tileMap;
        }
        internal Rectangle[] GetPlayerFrames()
        {
            return playerAnimation;
        }
        internal Rectangle[] GetBombFrames() {
            return bombAnimation;
        }
        internal Rectangle GetSprite(char tile) {
            return tileMap[tile];
        }
    }

}


