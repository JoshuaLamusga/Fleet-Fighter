using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace SimpleXnaFramework.Framework
{
    /// <summary>
    /// Plays individual sounds and has a primitive shuffle playlist.
    /// </summary>
    public class SoundPlaylist
    {
        public List<Song> music; //List of all music to play.
        private int _index; //The current song by index.
        private float _vol; //The volume of the music.
        public float Vol
        {
            get
            {
                return _vol;
            }

            set
            {
                if (value >= 0 && value <= 1)
                {
                    _vol = value;
                }
                else
                {
                    throw new Exception("Volume must be from 0 to 1.");
                }
            }
        }

        //Used to shuffle the playlist.
        private Random chance;

        /// <summary>
        /// Initializes default values.
        /// </summary>
        public SoundPlaylist()
        {
            music = new List<Song>();
            _index = 0;
            Vol = 1;

            chance = new Random();
        }

        /// <summary>
        /// Plays directly from a song.
        /// </summary>
        public static void Play(Song snd, float vol)
        {
            MediaPlayer.Volume = vol;
            MediaPlayer.Play(snd);
        }

        /// <summary>
        /// Plays the given playlist.
        /// </summary>
        public void Begin()
        {
            _index = chance.Next(0, music.Count);

            if (music.Count != 0)
            {
                MediaPlayer.Play(music[_index]);
            }
        }

        /// <summary>
        /// Pauses the active sound from the playlist.
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes the active sound from the playlist.
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// When music has finished, plays another randomly.
        /// </summary>
        public void Update()
        {
            //When the current sound has stopped (assuming it finished), plays
            //a different one on shuffle.
            if (MediaPlayer.State == MediaState.Stopped)
            {
                //The new song to play by index.
                int newIndex;

                //Gets a random song from the list that isn't the current one.
                do
                {
                    newIndex = chance.Next(0, music.Count);
                } while (_index == newIndex);

                //Sets the new index and plays its associated music.
                _index = newIndex;
                MediaPlayer.Play(music[_index]);
            }
        }
    }
}
