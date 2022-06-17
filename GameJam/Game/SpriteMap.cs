using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameJam.Game
{
    public class SpriteMap
    {
        private Dictionary<char, Rectangle> tileMap = new Dictionary<char, Rectangle>();
        private readonly Rectangle[] playerAnimation;
        private readonly Rectangle[] player1Animation;
        private readonly Rectangle[] bombAnimation;
        private readonly Rectangle[] nukeSignAnimation;

        //to change current tile:
        //Tile currentTile = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();
        //currentTile.sprite = gc.spriteMap.GetSprite('!');
        internal SpriteMap()
        {
            //selecteer meest links boven pixel
            //66 75 = bom
            //23 75 = floor
            //45 75 = wall
            //148 74 = full heart
            //132 74 = empty heart

            tileMap.Add('~', new Rectangle(4, 92, 16, 16));
            tileMap.Add(',', new Rectangle(4, 75, 16, 16));
            tileMap.Add('#', new Rectangle(45, 75, 16, 16));
            tileMap.Add('.', new Rectangle(23, 75, 16, 16));
            tileMap.Add('!', new Rectangle(66, 75, 16, 16));
            tileMap.Add('O', new Rectangle(148, 74, 15, 15));
            tileMap.Add('0', new Rectangle(132, 74, 15, 15));
            tileMap.Add('K', new Rectangle(4, 92, 16, 16));

            //power ups
            tileMap.Add('B', new Rectangle(66, 91, 16, 16));
            tileMap.Add('N', new Rectangle(86, 93, 16, 16));
            tileMap.Add('J', new Rectangle(142, 92, 16, 16));
            tileMap.Add('/', new Rectangle(122, 93, 16, 16));
            tileMap.Add('S', new Rectangle(103, 92, 16, 16));

            //undestroyable walls
            tileMap.Add('Q', new Rectangle(193, 15, 80, 82));
            tileMap.Add('W', new Rectangle(4, 57, 16, 16));

            //explosion tiles
            //                   ^
            //                   I
            //                <- x ->
            //                   I
            //                   V
            tileMap.Add('^', new Rectangle(36, 94, 16, 16));
            tileMap.Add('I', new Rectangle(36, 110, 16, 16));
            tileMap.Add('>', new Rectangle(68, 126, 16, 16));
            tileMap.Add('-', new Rectangle(52, 126, 16, 16));
            tileMap.Add('V', new Rectangle(36, 158, 16, 16));
            tileMap.Add('<', new Rectangle(4, 126, 16, 16));
            tileMap.Add('x', new Rectangle(36, 126, 16, 16));

            //freeze players
            tileMap.Add('F', new Rectangle(26, 9, 15, 16)); //p1
            tileMap.Add('f', new Rectangle(26, 27, 15, 16)); //p2

            nukeSignAnimation = new Rectangle[] {
                new Rectangle(88, 116, 30, 29),
                new Rectangle(122, 114, 31, 33),
                new Rectangle(160, 112, 33, 34),
                new Rectangle(86, 154, 32, 32),
                new Rectangle(129, 158, 29, 28)
            };

            playerAnimation = new Rectangle[]
                {
                    new Rectangle(43, 9, 16, 16),
                    new Rectangle(60, 9, 16, 16),
                    new Rectangle(77, 9, 16, 16)
                };
            player1Animation = new Rectangle[]
             {
                    new Rectangle(42, 27, 16, 16),
                    new Rectangle(62, 27, 16, 16),
                    new Rectangle(78, 28, 16, 16)
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
        internal Rectangle[] GetPlayer1Frames()
        {
            return player1Animation;
        }
        internal Rectangle[] GetBombFrames() {
            return bombAnimation;
        }
        internal Rectangle[] GetNukeSignFrames()
        {
            return nukeSignAnimation;
        }
        internal Rectangle GetSprite(char tile) {
            return tileMap[tile];
        }
    }

}


