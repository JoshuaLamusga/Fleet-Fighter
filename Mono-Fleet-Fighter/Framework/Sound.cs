using Microsoft.Xna.Framework.Audio;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Supplies useful sound methods.
    /// </summary>
    public class Sound
    {
        public SoundEffect snd;
        public SoundEffectInstance sndInstance;

        /// <summary>
        /// Plays directly from a sound, returning the instance.
        /// </summary>
        public static SoundEffectInstance Play(SoundEffect snd, float vol)
        {
            SoundEffectInstance sound = snd.CreateInstance();
            sound.Volume = vol;
            sound.Play();
            return sound;
        }

        /// <summary>
        /// Plays directly from a sound, returning the instance.
        /// </summary>
        public static SoundEffectInstance Play(SoundEffect snd, float vol, float pitch)
        {
            SoundEffectInstance sound = snd.CreateInstance();
            sound.Volume = vol;
            sound.Pitch = pitch;
            sound.Play();
            return sound;
        }

        /// <summary>
        /// Loop-plays directly from a sound, returning the instance.
        /// </summary>
        public static SoundEffectInstance PlayLooped(SoundEffect snd, float vol)
        {
            SoundEffectInstance sound = snd.CreateInstance();
            sound.IsLooped = true;
            sound.Volume = vol;
            sound.Play();
            return sound;
        }

        public static void Stop(SoundEffectInstance snd)
        {
            snd.Stop();
        }
    }
}
