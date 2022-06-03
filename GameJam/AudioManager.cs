using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace GameJam
{
    class AudioManager
    {
        public static void PlaySound(System.IO.UnmanagedMemoryStream soundFile)
        {
            SoundPlayer newSoundEffect = new SoundPlayer(soundFile);
            newSoundEffect.Play();
        }
    }
}
