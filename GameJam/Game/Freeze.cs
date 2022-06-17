using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameJam.Game
{
    class Freeze
    {
        private GameContext gc;
        private RenderForm rf;
        public Freeze(GameContext context, RenderForm renderform, bool player)
        {
            gc = context;
            rf = renderform;
            
            if (player)
            {
                gc.player.frame = 0;
                gc.player.frames = gc.GetSingeFrameArray('F');
            }
            else
            {
                gc.player1.frame = 0;
                gc.player1.frames = gc.GetSingeFrameArray('f');
            }
            LockPlayerMovement(player);
            AudioManager.PlaySound(Properties.Resources.freeze);

            Timer freezeDisableTimer = new Timer();
            freezeDisableTimer.Interval = (3500);
            freezeDisableTimer.Tick += (sender, e) => RemoveFreezeEffect(freezeDisableTimer, player);
            freezeDisableTimer.Start();
        }
        private void LockPlayerMovement(bool player)
        {
            if (player)
            {
                rf.canMoveP1 = false;
            }
            else rf.canMoveP2 = false;
        }
        private void RemoveFreezeEffect(Timer timer, bool player)
        {
            timer.Dispose();
            if (player)
            {
                rf.canMoveP1 = true;
                gc.player.frames = gc.spriteMap.GetPlayerFrames();
            }
            else {
                rf.canMoveP2 = true;
                gc.player1.frames = gc.spriteMap.GetPlayer1Frames();
            }
        }
    }
}
