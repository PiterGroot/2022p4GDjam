using System;
using System.Drawing;
using System.Threading;

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
            Thread.Sleep(15); //need delay because its to fast to be random
            thisPowerUp = (PowerUpType)randomPowerup;
            switch (thisPowerUp)
            {
                case PowerUpType.ExtraBomb:                 
                    thisPowerUpChar = 'B';
                    break;
                case PowerUpType.Nuke:
                    thisPowerUpChar = 'N';
                    break;
                case PowerUpType.Jump:
                    thisPowerUpChar = 'J';
                    break;
                case PowerUpType.Shield:
                    thisPowerUpChar = 'S';
                    break;
                case PowerUpType.BombStealer:
                    thisPowerUpChar = '/';
                    break;
            }
            Console.WriteLine("Trying this powerup " + thisPowerUpChar);
            RenderObject newPowerup = new RenderObject()
            {
                frames = gc.GetSingeFrameArray(thisPowerUpChar),
                rectangle = new Rectangle((int)position.x, (int)position.y, gc.tileSize, gc.tileSize),
            };
            gc.powerUps.Add(newPowerup);

        }
    }
}
