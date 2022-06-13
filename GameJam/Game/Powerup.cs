using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam.Game
{
    class Powerup
    {
        private Random rnd = new Random();
        public PowerUpType thisPowerUp;
        public char thisPowerUpChar;
        public Powerup(GameContext gc, Vector2 position)
        {
            int randomPowerup = rnd.Next(0, Enum.GetNames(typeof(PowerUpType)).Length);
            switch (randomPowerup)
            {
                case 1:
                    thisPowerUp = PowerUpType.ExtraBomb;
                    thisPowerUpChar = '*';
                    break;
                case 2:
                    thisPowerUp = PowerUpType.Nuke;
                    thisPowerUpChar = '@';
                    break;
                case 3:
                    thisPowerUp = PowerUpType.Jump;
                    thisPowerUpChar = 'J';
                    break;
                case 4:
                    thisPowerUp = PowerUpType.Shield;
                    thisPowerUpChar = '%';
                    break;
                case 5:
                    thisPowerUp = PowerUpType.BombStealer;
                    thisPowerUpChar = '/';
                    break;
                default:
                    break;
            }
            RenderObject newPowerup = new RenderObject()
            {
                frames = gc.GetSingeFrameArray(thisPowerUpChar),
                rectangle = new Rectangle((int)position.x, (int)position.y, gc.tileSize, gc.tileSize),
            };
            gc.powerUps.Add(newPowerup);

        }
    }
}
