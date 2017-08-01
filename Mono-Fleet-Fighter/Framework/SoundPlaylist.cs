using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleXnaFramework.Framework
{
    /// <summary>
    /// An extremely basic music player.
    /// </summary>
    public class SfxPlaylist
    {
        public List<SoundEffect> music = new List<SoundEffect>(); //The list of sounds.
        public SoundEffectInstance sound; //The current sound.
        public int soundIndex = 0; //The position of the sound in the list.

        /// <summary>
        /// Creates a new playlist for sound effects.
        /// </summary>
        /// <param name="sounds">Takes any number of sounds.</param>
        public SfxPlaylist(params SoundEffect[] snds)
        {
            foreach (SoundEffect sfx in snds)
            {
                music.Add(sfx);
            }
        }

        /// <summary>
        /// Randomly selects the next sound to play and returns sound index.
        /// Returns -1 if there are no sounds loaded.
        /// </summary>
        public int NextSoundRandom()
        {
            if (music.Count == 0)
            {
                return -1;
            }

            soundIndex = new Random().Next(music.Count);
            sound = music.ElementAt(soundIndex).CreateInstance();
            sound.Play();
            return soundIndex;
        }

        /// <summary>
        /// Pauses the active sound from the playlist.
        /// </summary>
        public void Pause()
        {
            sound?.Pause();
        }

        /// <summary>
        /// Resumes the active sound from the playlist.
        /// </summary>
        public void Resume()
        {
            sound.Resume();
        }

        /// <summary>
        /// Shuffles the sound list.
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            int numSongs = music.Count;
                
            //Iterates through O(1) times.
            while (numSongs > 1)
            {
                numSongs--;
                int next = rng.Next(numSongs + 1);
                SoundEffect value = music[next];
                music[next] = music[numSongs];
                music[numSongs] = value;
            }
        }

        /// <summary>
        /// Checks to see if the song ended and begins the next.
        /// </summary>
        public void Update()
        {
            //Starts playing the first sound.
            if (sound == null)
            {
                NextSoundRandom();
                return;
            }

            //When the sound finishes, start another.
            if (sound?.State == SoundState.Stopped)
            {
                //Keeps track of the old sound index for the loop.
                int tempSoundIndex = soundIndex;

                while (tempSoundIndex == soundIndex)
                {
                    sound.Stop();
                    NextSoundRandom();
                }
            }
        }
    }
}
