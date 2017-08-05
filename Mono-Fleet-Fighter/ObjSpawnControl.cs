using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;
using System;
using System.Collections.Generic;

namespace FleetFighter
{
    /// <summary>
    /// Controls the spawning of enemies and player.
    /// Instantiate once.
    /// </summary>
    public class ObjSpawnControl
    {
        public MainLoop game; //The game instance.
        public ObjPlayer objPlayer; //Creates a player.
        public List<ObjEnemy> enemyIndex; //The list of enemies.
        public List<ObjBullet> bulletIndex; //The list of bullets.
        public List<ObjMissile> missileIndex; //The list of missiles.
        public List<ObjFragment> fragmentIndex; //The list of fragments.
        public List<ObjExplosion> explosionIndex; //The list of explosions.        

        //Used to remove items from their lists without doing so during list iterations.
        public List<ObjEnemy> enemyIndexDelete;
        public List<ObjBullet> bulletIndexDelete;
        public List<ObjMissile> missileIndexDelete;
        public List<ObjFragment> fragmentIndexDelete;
        public List<ObjExplosion> explosionIndexDelete;

        public int wave = 0; //The current wave of enemies.
        public int waveNameTimeDuration = 120; //Time in ticks to display current wave.
        public int waveNameTime = 120; //Current time for displaying the wave.
        public ObjTimer timing; //A timer used for precise enemy creation.

        //Used for bullet creation. Not in ObjEnemy to prevent
        //seeding duplicates with the same current system time.
        public Random random;

        //The current difficulty level (automatically changes when the player loops the game).
        public int difficulty = 0; //0 = normal, 1 = hard, 2 = very hard, 3 = intense

        public ObjSpawnControl(MainLoop game)
        {
            this.game = game;
            enemyIndex = new List<ObjEnemy>();
            bulletIndex = new List<ObjBullet>();
            missileIndex = new List<ObjMissile>();
            fragmentIndex = new List<ObjFragment>();
            explosionIndex = new List<ObjExplosion>();
            enemyIndexDelete = new List<ObjEnemy>();
            bulletIndexDelete = new List<ObjBullet>();
            missileIndexDelete = new List<ObjMissile>();
            fragmentIndexDelete = new List<ObjFragment>();
            explosionIndexDelete = new List<ObjExplosion>();
            timing = new ObjTimer();
            random = new Random();
        }

        /// <summary>
        /// Loads the texture for the player and enemies.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            objPlayer = new ObjPlayer(
                game,
                game.GraphicsDevice.Viewport.Width / 2,
                game.GraphicsDevice.Viewport.Height - 64);
            objPlayer.LoadContent(Content);
        }

        /// <summary>
        /// Handles the creation of enemies using a timer.
        /// Extensive, so placed in its own method.
        /// </summary>
        public void EnemyCreation()
        {
            //Exits if the wave number is being displayed.
            //This simplifies timing.
            if (waveNameTime > 0)
            {
                return;
            }

            //For convenience, shortens the variable name.
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            //Plays a sound at the start of each wave.
            if (timing.atZero)
            {
                Sound.Play(game.sfxWaveStart, Convert.ToInt16(!game.isMuted) * 0.5f);
            }

            //Does things based on the wave.
            switch (wave)
            {
                //wave 1
                case (0):
                    if (timing.AtTime(0, 2, 0))
                    {
                        CreateSingle(0, width / 2, 0, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSingle(0, 42, 0, 0);
                        CreateSingle(0, width - 42, 0, 0);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreateSingle(0, width / 2, 0, 0);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreateRelative(0, 3, 12, game.texEnemy01.Width, 0, 0, 0);
                        CreateRelative(0, 3, width - 12, -game.texEnemy01.Width, 0, 0, 0);
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 3, width / 2 - game.texEnemy01.Width, game.texEnemy01.Width, 0, 0, 0);
                    }
                    if (timing.secs >= 9 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                //wave 2
                case (1):
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreateRelative(0, 5, width / 2 - (game.texEnemy01.Width * 2), game.texEnemy01.Width, game.texEnemy01.Height, 0, 0);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateRelative(0, 5, width / 2 + (game.texEnemy01.Width * 2), -game.texEnemy01.Width, game.texEnemy01.Height, 0, 0);
                    }
                    else if (timing.AtTime(0, 3, 0))
                    {
                        CreatePointLine(0, 32, 2, game.texEnemy01.Height + 4, 0, 0);
                        CreatePointLine(0, width - 32, 2, game.texEnemy01.Height + 4, 0, 0);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreateSingle(EnemyType.Flyer, width / 2, 0, 0);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateSpreadDiagonal(0, 10, true, 0, 0);
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreateSpreadDiagonal(0, 10, false, 0, 0);
                    }
                    else if (timing.AtTime(0, 10, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 3, width / 2 - game.texEnemy01.Width, game.texEnemy01.Width, 0, 0, 0);
                    }
                    if (timing.secs >= 10 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                //wave 3
                case (2):
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, width / 2, 3, game.texEnemy01.Height + 32, 0, 0);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateSpreadStaggered(0, 20, 0, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSingle(EnemyType.Sentry, width / 2, 200, 30);
                        CreateSingle(EnemyType.Sentry, width / 2 - game.texEnemy02.Width, 200, 60);
                        CreateSingle(EnemyType.Sentry, width / 2 + game.texEnemy02.Width, 200, 60);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, game.texEnemy01.Width / 2, 200, 0.1f);
                        CreateSingle(EnemyType.BankingFlyer, width - game.texEnemy01.Width / 2, 200, -0.1f);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreateSpread(EnemyType.BankingFlyer, 10, game.texEnemy01.Width, 0, 0);
                    }
                    if (timing.secs >= 7 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                //wave 4
                case (3):
                {
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreateRelative(0, 12, width / 2 - (game.texEnemy01.Width * 12), game.texEnemy01.Width, 0, 0, 0);
                    }
                    if (timing.AtTime(0, 2, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 10, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 10, 0, 0);
                    }
                    else if (timing.AtTime(0, 3, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, width / 2, 0, 0);
                    }
                    else if (timing.AtTime(30, 3, 0))
                    {
                        CreateSingle(EnemyType.Sentry, width / 2, 100, 60);
                        CreateSingle(EnemyType.Sentry, width / 2, 200, 60);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 10, 200, 0.1f);
                        CreateSingle(EnemyType.BankingFlyer, width - 10, 200, -0.1f);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, 10, 5, game.texEnemy01.Height, 0, 0);
                        CreatePointLine(EnemyType.BankingFlyer, width - 10, 5, game.texEnemy01.Height, 0, 0);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateSpreadStaggered(EnemyType.BankingFlyer, 10, 0, 0);
                    }
                    else if (timing.AtTime(0, 8, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 10, 200, 0.2f);
                        CreateSingle(EnemyType.BankingFlyer, width - 10, 200, -0.2f);
                    }
                    else if (timing.AtTime(30, 8, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, width / 2 - 64, 6, game.texEnemy01.Height, 250, -0.2f);
                        CreatePointLine(EnemyType.BankingFlyer, width / 2 + 64, 6, game.texEnemy01.Height, 250, 0.2f);
                    }
                    if (timing.secs >= 8 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                }
                //wave 5
                case (4):
                {
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 32, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 32, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, 128, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 128, 0, 0);
                    }
                    else if (timing.AtTime(30, 1, 0))
                    {
                        CreateSingle(EnemyType.Sentry, 64, 0, 0);
                        CreateSingle(EnemyType.Sentry, width - 64, 0, 0);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 64, 200, 0.2f);
                        CreateSingle(EnemyType.BankingFlyer, width - 64, 200, -0.2f);
                        CreateSingle(EnemyType.BankingFlyer, 196, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 196, 0, 0);
                    }
                    else if (timing.AtTime(30, 2, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 32, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 32, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, 128, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 128, 0, 0);
                    }
                    else if (timing.AtTime(0, 3, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 10, game.texEnemy02.Height, 100, 60);
                    }
                    else if (timing.AtTime(30, 3, 0))
                    {
                        CreateSpread(EnemyType.BankingFlyer, 10, game.texEnemy01.Height, 0, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, 64, 200, 0.2f);
                        CreateSingle(EnemyType.BankingFlyer, width - 64, 200, -0.2f);
                        CreateSingle(EnemyType.BankingFlyer, 196, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width - 196, 0, 0);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, width - game.texEnemy01.Width, 4, game.texEnemy01.Height, 0, 0);
                        CreatePointLine(EnemyType.BankingFlyer, width + game.texEnemy01.Width, 4, game.texEnemy01.Height, 0, 0);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 2, 20, 0, -game.texEnemy01.Height, 200, 0.2f);
                        CreateRelative(EnemyType.BankingFlyer, 2, width - 20, 0, -game.texEnemy01.Height, 200, -0.2f);
                        CreateSingle(EnemyType.Distractor, width / 2 - 32, 2, 0);
                        CreateSingle(EnemyType.Distractor, width / 2 + 32, -2, 0);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.Sentry, 10, true, 200, 60);
                        CreateSpreadDiagonal(EnemyType.Sentry, 10, false, 200, 60);
                    }
                    if (timing.secs >= 7 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                }
                //Wave 6
                case (5):
                {
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, game.texEnemy01.Width, 10, 20, 260, 0.2f);
                        CreatePointLine(EnemyType.BankingFlyer, width - game.texEnemy01.Width, 10, 20, 260, -0.2f);
                    }
                    else if (timing.AtTime(30, 1, 0))
                    {
                        CreateSingle(EnemyType.Sentry, 40, 20, 60);
                        CreateSingle(EnemyType.Sentry, width - 40, 20, 60);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.Sentry, 10, true, 100, 60);
                        CreateSpreadDiagonal(EnemyType.Sentry, 10, false, 100, 60);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 3, true, 200, 0.1f);
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 3, false, 200, -0.1f);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 2, 30, game.texEnemy01.Width, 0, 0, 0);
                        CreateRelative(EnemyType.BankingFlyer, 2, width - 30, game.texEnemy01.Width, 0, 0, 0);
                        CreateSingle(EnemyType.Distractor, width / 2 - 60, 2f, 0);
                        CreateSingle(EnemyType.Distractor, width / 2 + 60, -2f, 0);
                    }
                    else if (timing.AtTime(30, 6, 0))
                    {
                        CreateSingle(EnemyType.Distractor, width / 2, 3f, 0);
                        CreateSingle(EnemyType.Distractor, width / 2, -3f, 0);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, game.texEnemy01.Width, 3, 20, 320, 0.4f);
                        CreatePointLine(EnemyType.BankingFlyer, width - game.texEnemy01.Width, 3, 20, 320, -0.4f);
                    }
                    else if (timing.AtTime(30, 7, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyer, width / 2 - 32, 0, 0);
                        CreateSingle(EnemyType.BankingFlyer, width / 2 + 32, 0, 0);
                        CreateRelative(EnemyType.BankingFlyer, 2, game.texEnemy01.Width / 2, game.texEnemy01.Width, 0, 0, 0);
                        CreateRelative(EnemyType.BankingFlyer, 2, width - game.texEnemy01.Width / 2, game.texEnemy01.Width, 0, 0, 0);
                    }
                    else if (timing.AtTime(0, 8, 0))
                    {
                        CreateSingle(EnemyType.ThrustFlyer, width / 2, 0, 0);
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 5, true, 300, 0.3f);
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 5, false, 300, -0.3f);
                    }
                    else if (timing.AtTime(30, 8, 0))
                    {
                        CreateSpreadStaggered(EnemyType.Sentry, 10, 200, 60);
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreateSingle(EnemyType.Distractor, game.texEnemy03.Width, 2, 0);
                        CreateSingle(EnemyType.Distractor, width - game.texEnemy03.Width, -2, 0);
                    }
                    else if (timing.AtTime(30, 9, 0))
                    {
                        CreatePointLine(EnemyType.Distractor, 40, 3, game.texEnemy03.Width, 4, 0);
                        CreatePointLine(EnemyType.Distractor, width - 40, 3, game.texEnemy03.Width, -4, 0);
                    }
                    if (timing.secs >= 9 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                }
                //Wave 7
                case (6):
                {
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreatePointLine(EnemyType.ThrustFlyer, width / 2, 5, game.texEnemy03.Width, 4, 0);
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 10, true, 240, 0.2f);
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 10, false, 240, -0.2f);
                        CreateSingle(EnemyType.ThrustFlyer, 32, 0, 0);
                        CreateSingle(EnemyType.ThrustFlyer, width - 32, 0, 0);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateSingle(EnemyType.Sentry, width / 2, 0, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSingle(EnemyType.Sentry, width / 2, 0, 0);
                        CreateRelative(EnemyType.BankingFlyer, 3, game.texEnemy01.Width, game.texEnemy01.Width, 0, 100, 0.1f);
                        CreateRelative(EnemyType.BankingFlyer, 3, width - game.texEnemy01.Width, -game.texEnemy01.Width, 0, 100, -0.1f);
                        CreatePointLine(EnemyType.Sentry, 160, 2, game.texEnemy02.Height + 2, 360, 120);
                        CreatePointLine(EnemyType.Sentry, width - 160, 2, game.texEnemy02.Height + 2, 360, 120);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreateSpread(EnemyType.ThrustFlyer, 5, game.texEnemy04.Width + 20, 100, 0.1f);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 30, 20, 5, game.texEnemy01.Height, 300, 0.1f);
                        CreateRelative(EnemyType.BankingFlyer, 30, width - 20, -5, game.texEnemy01.Height, 300, -0.1f);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 10, game.texEnemy02.Width, 250, 120);
                    }
                    else if (timing.AtTime(30, 7, 0))
                    {
                        CreateSingle(EnemyType.ThrustFlyer, width / 2, 100, 0.2f);
                    }
                    else if (timing.AtTime(0, 8, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 10, game.texEnemy02.Width, 250, 120);
                        CreateSingle(EnemyType.ThrustFlyer, width / 2, 100, 0.2f);
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 30, 100, 5, game.texEnemy01.Height, 200, 0.2f);
                        CreateRelative(EnemyType.BankingFlyer, 30, width - 100, -5, game.texEnemy01.Height, 200, -0.2f);
                    }
                    else if (timing.AtTime(30, 9, 0))
                    {
                        CreateSingle(EnemyType.ThrustFlyer, width / 2, 100, 0.2f);
                        CreateSingle(EnemyType.ThrustFlyer, 20, 200, 0.2f);
                        CreateSingle(EnemyType.ThrustFlyer, width  - 20, 200, 0.2f);
                    }
                    else if (timing.AtTime(0, 11, 0))
                    {
                        CreateRelative(EnemyType.Sentry, 10, game.texEnemy02.Width, game.texEnemy02.Width * 2, 0, 250, 60);
                    }
                    if (timing.secs >= 11 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                }
                //Wave 8
                case (7):
                {
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyerHard, width / 2, 200, 60);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 10, game.texEnemy01.Width, 0, game.texEnemy01.Height, 200, 0.2f);
                        CreateRelative(EnemyType.BankingFlyer, 10, width - game.texEnemy01.Width, 0, game.texEnemy01.Height, 200, -0.2f);
                    }
                    else if (timing.AtTime(0, 3, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 10, 32 + game.texEnemy01.Width, 0, game.texEnemy01.Height, 175, 0.3f);
                        CreateRelative(EnemyType.BankingFlyer, 10, width - 32 - game.texEnemy01.Width, 0, game.texEnemy01.Height, 175, -0.3f);
                    }
                    else if (timing.AtTime(30, 3, 0))
                    {
                        CreateRelative(EnemyType.Distractor, 2, game.texEnemy03.Width, game.texEnemy03.Width, 0, -2, 0);
                        CreateRelative(EnemyType.Distractor, 2, width - game.texEnemy03.Width, -game.texEnemy03.Width, 0, 2, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 10, true, 200, 0.2f);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.BankingFlyer, 10, false, 200, -0.2f);
                    }
                    else if (timing.AtTime(30, 5, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, 20, 50, game.texEnemy01.Height, 0, 0);
                        CreatePointLine(EnemyType.BankingFlyer, width - 20, 50, game.texEnemy01.Height, 0, 0);
                        CreateSingle(EnemyType.BankingFlyerHard, width / 2, 200, 120);
                    }
                    else if (timing.AtTime(0, 6, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyerHard, width / 2, 200, 120);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 5, game.texEnemy02.Width, 100, 30);
                    }
                    else if (timing.AtTime(0, 8, 0))
                    {
                        CreateSpreadStaggered(0, 10, 0, 0);
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 5, game.texEnemy02.Width, 200, 30);
                    }
                    else if (timing.AtTime(0, 10, 0))
                    {
                        CreateSpreadStaggered(0, 20, 0, 0);
                    }
                    else if (timing.AtTime(0, 11, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 5, game.texEnemy02.Width, 300, 30);
                    }
                    else if (timing.AtTime(0, 12, 0))
                    {
                        CreateSpreadStaggered(0, 30, 0, 0);
                    }
                    else if (timing.AtTime(0, 13, 0))
                    {
                        CreateSpread(EnemyType.Sentry, 5, game.texEnemy02.Width, 400, 30);
                    }
                    else if (timing.AtTime(0, 15, 0))
                    {
                        CreateSpreadStaggered(0, 50, 0, 0);
                    }
                    if (timing.secs >= 15 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
                }
                //Wave 9
                case (8):
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreatePointLine(EnemyType.Bullet, width / 2, 5, 10, 0, 0);
                        CreateSingle(EnemyType.Bullet, 20, 0, 0);
                        CreateSingle(EnemyType.Bullet, width - 20, 0, 0);
                    }
                    else if (timing.AtTime(0, 2, 0))
                    {
                        CreateSingle(EnemyType.Bullet, 40, 0, 0);
                        CreateSingle(EnemyType.Bullet, width - 40, 0, 0);
                        CreateSpreadStaggered(0, 20, 0, 0);
                    }
                    else if (timing.AtTime(0, 3, 0))
                    {
                        ObjEnemy enemy = CreateSingle(EnemyType.Spawner, width / 2, 30, 0);
                    }
                    else if (timing.AtTime(0, 4, 0))
                    {
                        CreatePointLine(EnemyType.Sentry, 40, 3, 0, 200, 60);
                        CreatePointLine(EnemyType.Sentry, width - 40, 3, 0, 200, 60);
                    }
                    else if (timing.AtTime(30, 4, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyer, 3, 0, game.texEnemy01.Width + 32, 0, 300, 0.02f);
                        CreateRelative(EnemyType.BankingFlyer, 3, width, -game.texEnemy01.Width - 32, 0, 300, -0.02f);
                    }
                    else if (timing.AtTime(0, 5, 0))
                    {
                        CreateSpreadStaggered(0, 20, 0, 0);
                    }
                    else if (timing.AtTime(0, 7, 0))
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            CreateSingleY(EnemyType.BankingFlyer, 50, -10 * i, 10 * i, 0.03f);
                            CreateSingleY(EnemyType.BankingFlyer, width - 50, -10 * i, 10 * i, -0.03f);
                        }
                    }
                    else if (timing.AtTime(0, 8, 0))
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            CreateSingleY(EnemyType.BankingFlyer, 150, -10 * i, 10 * i, -0.03f);
                            CreateSingleY(EnemyType.BankingFlyer, width - 150, -10 * i, 10 * i, 0.03f);
                        }
                    }
                    else if (timing.AtTime(0, 9, 0))
                    {
                        CreatePointLine(EnemyType.Sentry, 20, 10, game.texEnemy01.Width, 200, 60);
                        CreatePointLine(EnemyType.Sentry, width - 20, 10, game.texEnemy01.Width, 200, 60);
                    }
                    else if (timing.AtTime(0, 10, 0))
                    {
                        CreateSingle(EnemyType.BankingFlyerHard, width / 2, 0, 0);

                        for (int i = 0; i < 30; i++)
                        {
                            CreateSingleY(EnemyType.BankingFlyer, 10 * i, -10 * i, 10 * i, -0.03f);
                            CreateSingleY(EnemyType.BankingFlyer, width - (10 * i), -10 * i, 10 * i, 0.03f);
                        }
                    }
                    else if (timing.AtTime(0, 11, 0))
                    {
                        CreateSingle(EnemyType.Distractor, 20, 2, 0);
                        CreateSingle(EnemyType.Distractor, width - 20, 2, 0);
                        CreateSingle(EnemyType.Distractor, 80, -2, 0);
                        CreateSingle(EnemyType.Distractor, width - 80, -2, 0);
                    }
                    else if (timing.AtTime(0, 12, 0))
                    {
                        CreateSpreadDiagonal(EnemyType.Distractor, 20, true, 4, 0);
                        CreateSpreadDiagonal(EnemyType.Distractor, 20, false, -4, 0);
                    }
                    else if (timing.AtTime(0, 13, 0))
                    {
                        CreateSpread(0, 20, game.texEnemy01.Width, 0, 0);
                    }
                    else if (timing.AtTime(0, 14, 0))
                    {
                        CreateSingle(EnemyType.Spawner, 100, 10, 0);
                        CreateSingle(EnemyType.Spawner, width - 100, 10, 0);
                    }
                    else if (timing.AtTime(0, 15, 0))
                    {
                        CreateRelative(EnemyType.ThrustFlyer, 5, 120, game.texEnemy05.Width, 0, 200, 0.02f);
                        CreateRelative(EnemyType.ThrustFlyer, 5, width - 120, game.texEnemy05.Width, 0, 200, 0.02f);
                    }
                    if (timing.secs >= 15 && enemyIndex.Count == 0)
                    {
                        wave++;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;

                //Wave 10
                case (9):
                    if (timing.AtTime(0, 1, 0))
                    {
                        CreateSpreadStaggered(0, 10, 0, 0);
                    }
                    if (timing.AtTime(0, 2, 0))
                    {
                        CreateSpreadStaggered(0, 20, 0, 0);
                    }
                    if (timing.AtTime(0, 3, 0))
                    {
                        CreateSpreadStaggered(0, 30, 0, 0);
                    }
                    if (timing.AtTime(0, 4, 0))
                    {
                        CreateSpreadStaggered(0, 40, 0, 0);
                    }
                    if (timing.AtTime(0, 5, 0))
                    {
                        CreateSpreadStaggered(0, 50, 0, 0);
                    }
                    if (timing.AtTime(0, 6, 0))
                    {
                        CreateSpreadStaggered(0, 60, 0, 0);
                    }
                    if (timing.AtTime(0, 9, 0))
                    {
                        CreatePointLine(EnemyType.BankingFlyer, 0, 30, game.texEnemy01.Height, width - 50, 0.03f);
                        CreatePointLine(EnemyType.BankingFlyer, width, 30, game.texEnemy01.Height, width - 50, -0.03f);
                        CreateRelative(EnemyType.BankingFlyer, 3, width / 2 - 50, 50, 0, 0, 0);
                    }
                    if (timing.AtTime(0, 10, 0))
                    {
                        CreateRelative(EnemyType.Sentry, 5, width / 2 - 60, 30, 0, 200, 120);
                    }
                    if (timing.AtTime(0, 11, 0))
                    {
                        CreateRelative(EnemyType.Distractor, 5, width / 2 - 60, 30, 0, 0, 0);
                    }
                    if (timing.AtTime(0, 12, 0))
                    {
                        CreateRelative(EnemyType.ThrustFlyer, 5, width / 2 - 60, 30, 0, 0, 0);
                    }
                    if (timing.AtTime(0, 13, 0))
                    {
                        CreateRelative(EnemyType.BankingFlyerHard, 5, width / 2 - 100, 50, 0, 0, 0);
                    }
                    if (timing.AtTime(0, 14, 0))
                    {
                        CreateSpreadStaggered(EnemyType.Spawner, 10, 10, 0);
                    }
                    if (timing.AtTime(0, 15, 0))
                    {
                        CreateSpreadStaggered(EnemyType.Spawner, 30, 20, 0);
                    }
                    if (timing.secs >= 15 && enemyIndex.Count == 0)
                    {
                        if (difficulty < 3)
                        {
                            difficulty++;
                        }
                        wave = 0;
                        timing.Reset();
                        waveNameTime = waveNameTimeDuration;
                    }
                    break;
            }

            //Plays a sound at the end of each wave.
            if (waveNameTime == waveNameTimeDuration)
            {
                Sound.Play(game.sfxWaveEnd, Convert.ToInt16(!game.isMuted) * 0.5f);
            }
        }

        /// <summary>
        /// Updates the player things.
        /// </summary>
        public void Update(KeyboardState keyState, KeyboardState keyStateOld, MouseState mouseState, MouseState mouseStateOld)
        {
            //Updates the timer for the wave time.
            waveNameTime--;
            if (waveNameTime < 0)
            {
                waveNameTime = 0;
            }

            //Updates the timer for enemy creation
            if (waveNameTime == 0)
            {
                timing.Update();
                EnemyCreation();
            }
            objPlayer.Update(keyState, keyStateOld, mouseState, mouseStateOld, this);

            //Accomodates for type 7 modifying the enemy index.
            for (int i = 0; i < enemyIndex.Count; i++)
            {
                if (enemyIndex[i].type == EnemyType.Spawner)
                {
                    if (enemyIndex[i].actionTime == 0)
                    {
                        ObjEnemy enemy = new ObjEnemy(game, this, EnemyType.Bullet);
                        enemy.SetCharacteristics();
                        enemy.sprite.rectDest.X = enemyIndex[i].sprite.rectDest.X;
                        enemy.sprite.rectDest.Y = enemyIndex[i].sprite.rectDest.Y;                        
                        enemyIndex.Add(enemy);

                        //Resets the time to re-trigger enemy creation again.
                        enemyIndex[i].actionTime = enemyIndex[i].actionTimeDuration;
                    }
                }
            }

            //Updates all lists, cleans up unused resources, and
            //gets rid of off-screen items.
            foreach (ObjEnemy enemy in enemyIndex)
            {
                enemy.Update();

                //This doesn't delete enemies above the top of the screen or distractors.
                if (enemy.type != EnemyType.Distractor)
                {
                    if (enemy.sprite.rectDest.X < -(enemy.sprite.rectDest.Width / 2) ||
                        enemy.sprite.rectDest.X > game.GraphicsDevice.Viewport.Width + (enemy.sprite.rectDest.Width / 2) ||
                        enemy.sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height + (enemy.sprite.rectDest.Height / 2))
                    {
                        enemyIndexDelete.Add(enemy);
                    }
                }
                if (enemy.isDestroyed)
                {
                    Sound.Play(game.sfxEnemyDestroyed, Convert.ToInt16(!game.isMuted) * 0.5f, (float)(game.chance.NextDouble() * 1.5) - 0.5f);
                    enemyIndexDelete.Add(enemy);
                    ObjExplosion explosion = new ObjExplosion(game, game.texExplosion, this);
                    explosion.sprite.rectDest.X = enemy.sprite.rectDest.X;
                    explosion.sprite.rectDest.Y = enemy.sprite.rectDest.Y;
                    explosionIndex.Add(explosion);

                    //Creates a bunch of fragments.
                    int fragments;
                    if (enemy.type != EnemyType.Bullet) //Excludes type 6 enemies as they are very easy to destroy.
                    {
                        fragments = random.Next(1, (int)(random.Next(1, 4) * ((int)enemy.type + 1)));
                    }
                    else
                    {
                        fragments = 1; //Spawns one fragment.
                    }
                    for (int i = 0; i < fragments; i++)
                    {
                        ObjFragment fragment = new ObjFragment(game, this);
                        fragment.sprite.rectDest.X = enemy.sprite.rectDest.X;
                        fragment.sprite.rectDest.Y = enemy.sprite.rectDest.Y;
                        do //repeats until the colors are bright enough to see (combined values >= 255)
                        {
                            fragment.sprite.color = new Color(random.Next(0, 3) * 128, random.Next(0, 3) * 128, random.Next(0, 3) * 128);
                        } while (fragment.sprite.color.R + fragment.sprite.color.G + fragment.sprite.color.B < 255);
                        double scale = 0.5 + random.NextDouble();
                        fragment.sprite.scaleX = (float)(scale);
                        fragment.sprite.scaleY = (float)(scale);
                        fragment.spritePhysics.AddMotionVector(false, random.Next(2, 5), random.NextDouble() * 360);
                        fragment.spritePhysics.AddRotation(random.NextDouble() * 5);
                        fragmentIndex.Add(fragment);
                    }
                }
            }
            foreach (ObjBullet bullet in bulletIndex)
            {
                bullet.Update();
                
                //Removes the bullet if off-screen
                if (bullet.sprite.rectDest.X < 0 ||
                    bullet.sprite.rectDest.X > game.GraphicsDevice.Viewport.Width ||
                    bullet.sprite.rectDest.Y < 0 ||
                    bullet.sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height)
                {
                    bulletIndexDelete.Add(bullet);
                }
                if (bullet.destroyed)
                {
                    bulletIndexDelete.Add(bullet);
                }
            }
            foreach (ObjMissile missile in missileIndex)
            {
                missile.Update();
                
                //Removes the missile if off-screen
                if (missile.sprite.rectDest.X < 0 ||
                    missile.sprite.rectDest.X > game.GraphicsDevice.Viewport.Width ||
                    missile.sprite.rectDest.Y < 0 ||
                    missile.sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height)
                {
                    missileIndexDelete.Add(missile);
                }
                if (missile.timer.atZero)
                {
                    missileIndexDelete.Add(missile);
                    Sound.Play(game.sfxCollBullet, Convert.ToInt16(!game.isMuted) * 0.5f);
                    ObjExplosion explosion = new ObjExplosion(game, game.texMissileExplosion, this);
                    explosion.sprite.rectDest.X = missile.sprite.rectDest.X;
                    explosion.sprite.rectDest.Y = missile.sprite.rectDest.Y;
                    explosionIndex.Add(explosion);

                    //Destroys the enemy if they are within a radius of the missile.
                    foreach (ObjEnemy enemy in enemyIndex)
                    {
                        if (Mathematics.PointDistance(
                            missile.sprite.rectDest.X,
                            missile.sprite.rectDest.Y,
                            enemy.sprite.rectDest.X,
                            enemy.sprite.rectDest.Y) < 150)
                        {
                            enemy.isDestroyed = true;
                        }
                    }
                }
            }
            foreach (ObjFragment fragment in fragmentIndex)
            {
                fragment.Update();
                
                //Checks if the fragment is within a small distance of the player.
                //If so, the fragments move towards the player (unless they're on a high difficulty).
                if (difficulty < 3)
                {
                    if (Mathematics.PointDistance(
                        fragment.sprite.rectDest.X,
                        fragment.sprite.rectDest.Y,
                        objPlayer.sprite.rectDest.X,
                        objPlayer.sprite.rectDest.Y) < 75)
                    {
                        fragment.spritePhysics.AddMotionDirection(
                            true, 5, (float)Mathematics.PointDirection(
                                new Vector2(fragment.sprite.rectDest.X, fragment.sprite.rectDest.Y),
                                new Vector2(objPlayer.sprite.rectDest.X, objPlayer.sprite.rectDest.Y)));
                    }
                }
                if (fragment.sprite.rectDest.X < 0 ||
                    fragment.sprite.rectDest.Y < 0 ||
                    fragment.sprite.rectDest.X > game.GraphicsDevice.Viewport.Width ||
                    fragment.sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height)
                {
                    fragmentIndexDelete.Add(fragment);
                }
                if (fragment.destroyed)
                {
                    fragmentIndexDelete.Add(fragment);
                }
            }
            foreach (ObjExplosion explosion in explosionIndex)
            {
                explosion.Update();
                if (explosion.destroyed)
                {
                    explosionIndexDelete.Add(explosion);
                }
            }

            //Cleans all items out of lists qeued for deletion.
            foreach (ObjEnemy enemy in enemyIndexDelete)
            {
                enemyIndex.Remove(enemy);
            }
            foreach (ObjBullet bullet in bulletIndexDelete)
            {
                bulletIndex.Remove(bullet);
            }
            foreach (ObjMissile missile in missileIndexDelete)
            {
                missileIndex.Remove(missile);
            }
            foreach (ObjFragment fragment in fragmentIndexDelete)
            {
                fragmentIndex.Remove(fragment);
            }
            foreach (ObjExplosion explosion in explosionIndexDelete)
            {
                explosionIndex.Remove(explosion);
            }
            //Resets the removal lists.
            enemyIndexDelete = new List<ObjEnemy>();
            bulletIndexDelete = new List<ObjBullet>();
            missileIndexDelete = new List<ObjMissile>();
            fragmentIndexDelete = new List<ObjFragment>();
            explosionIndexDelete = new List<ObjExplosion>();
        }

        /// <summary>
        /// Creates a spread of enemies of one type evenly spaced across the x-axis.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="margins">The border in pixels on both sides of the x-axis.</param>
        public ObjEnemy CreateSingle(EnemyType type, int xPos, float actionFloat1, float actionFloat2)
        {
            ObjEnemy enemy = new ObjEnemy(game, this, type);
            enemy.sprite.rectDest.X = xPos;
            enemy.actionFloat1 = actionFloat1;
            enemy.actionFloat2 = actionFloat2;
            enemy.SetCharacteristics();
            enemyIndex.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// Creates a spread of enemies of one type evenly spaced across the x-axis.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="xPos">The horizontal location of the ship.</param>
        /// <param name="yPos">The vertical offset of the ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="margins">The border in pixels on both sides of the x-axis.</param>
        public ObjEnemy CreateSingleY(EnemyType type, int xPos, int yOffset, float actionFloat1, float actionFloat2)
        {
            ObjEnemy enemy = new ObjEnemy(game, this, type);
            enemy.sprite.rectDest.X = xPos;
            enemy.sprite.rectDest.Y = yOffset;
            enemy.actionFloat1 = actionFloat1;
            enemy.actionFloat2 = actionFloat2;
            enemy.SetCharacteristics();
            enemyIndex.Add(enemy);
            return enemy;
        }

        /// <summary>
        /// Creates a spread of enemies of one type evenly spaced across the x-axis.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="actionFloat1">A float used to indicate behavior for the enemies based on their type.</param>
        /// <param name="actionFloat2">A float used to indicate behavior for the enemies based on their type.</param>
        public List<ObjEnemy> CreateSpread(EnemyType type, int num, int distance, float actionFloat1, float actionFloat2)
        {
            List<ObjEnemy> enemyList = new List<ObjEnemy>();

            for (int i = 0; i < num; i++)
            {
                ObjEnemy enemy = new ObjEnemy(game, this, type);
                enemy.sprite.rectDest.X = 
                    (game.GraphicsDevice.Viewport.Width / 2) - (distance * num) + (distance * 2 * i);
                enemy.actionFloat1 = actionFloat1;
                enemy.actionFloat2 = actionFloat2;
                enemy.SetCharacteristics();
                enemyIndex.Add(enemy);
                enemyList.Add(enemy);
            }

            return enemyList;
        }
        
        /// <summary>
        /// Creates a spread of enemies of one type evenly spaced across the x-axis.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="actionFloat1">A float used to indicate behavior for the enemies based on their type.</param>
        /// <param name="actionFloat2">A float used to indicate behavior for the enemies based on their type.</param>
        public List<ObjEnemy> CreateSpreadStaggered(EnemyType type, int num, float actionFloat1, float actionFloat2)
        {
            List<ObjEnemy> enemyList = new List<ObjEnemy>();

            for (int i = 0; i < num; i++)
            {
                ObjEnemy enemy = new ObjEnemy(game, this, type);
                enemy.sprite.rectDest.X = (game.GraphicsDevice.Viewport.Width / num) * i;
                //The even rows start further away.
                if (i % 2 == 0)
                {
                    enemy.sprite.rectDest.Y = -enemy.spriteAtlas.frameHeight - (i * 0);
                }
                enemy.actionFloat1 = actionFloat1;
                enemy.actionFloat2 = actionFloat2;
                enemy.SetCharacteristics();
                enemyIndex.Add(enemy);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        /// <summary>
        /// Creates a spread of enemies of one type evenly spaced across the x-axis.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="isLeftMost">Whether or not the first enemy is on the left.</param>
        /// <param name="actionFloat1">A float used to indicate behavior for the enemies based on their type.</param>
        /// <param name="actionFloat2">A float used to indicate behavior for the enemies based on their type.</param>
        public List<ObjEnemy> CreateSpreadDiagonal(EnemyType type, int num, bool isLeftMost, float actionFloat1, float actionFloat2)
        {
            List<ObjEnemy> enemyList = new List<ObjEnemy>();

            for (int i = 0; i < num; i++)
            {
                ObjEnemy enemy = new ObjEnemy(game, this, type);
                enemy.sprite.rectDest.X = (game.GraphicsDevice.Viewport.Width / num) * i;
                if (isLeftMost)
                {
                    enemy.sprite.rectDest.Y = -i * enemy.spriteAtlas.frameHeight;
                }
                else
                {
                    enemy.sprite.rectDest.Y = (-num + i) * enemy.spriteAtlas.frameHeight;
                }
                enemy.actionFloat1 = actionFloat1;
                enemy.actionFloat2 = actionFloat2;
                enemy.SetCharacteristics();
                enemyIndex.Add(enemy);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        /// <summary>
        /// Creates a line of enemies of the chosen type in the chosen x position.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="yOffset">The vertican space between each ship.</param>
        /// <param name="actionFloat1">A float used to indicate behavior for the enemies based on their type.</param>
        /// <param name="actionFloat2">A float used to indicate behavior for the enemies based on their type.</param>
        public List<ObjEnemy> CreatePointLine(EnemyType type, float xPos, int num, int yOffset, float actionFloat1, float actionFloat2)
        {
            List<ObjEnemy> enemyList = new List<ObjEnemy>();

            for (int i = 0; i < num; i++)
            {
                ObjEnemy enemy = new ObjEnemy(game, this, type);
                enemy.sprite.rectDest.X = xPos;
                enemy.sprite.rectDest.Y = -(i * enemy.spriteAtlas.frameHeight) - (i * yOffset);
                enemy.actionFloat1 = actionFloat1;
                enemy.actionFloat2 = actionFloat2;
                enemy.SetCharacteristics();
                enemyIndex.Add(enemy);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        /// <summary>
        /// Creates a bunch of enemies of one type positionally relative to each other.
        /// </summary>
        /// <param name="type">The type of ship.</param>
        /// <param name="xPos">The initial x-position.</param>
        /// <param name="xOffset">The x-offset.</param>
        /// <param name="yOffset">The y-offset.</param>
        /// <param name="num">The number of ships created.</param>
        /// <param name="actionFloat1">A float used to indicate behavior for the enemies based on their type.</param>
        /// <param name="actionFloat2">A float used to indicate behavior for the enemies based on their type.</param>
        public List<ObjEnemy> CreateRelative(EnemyType type, int num, float xPos, float xOffset, float yOffset, float actionFloat1, float actionFloat2)
        {
            List<ObjEnemy> enemyList = new List<ObjEnemy>();

            for (int i = 0; i < num; i++)
            {
                ObjEnemy enemy = new ObjEnemy(game, this, type);
                enemy.sprite.rectDest.X = xPos + (xOffset * i);
                enemy.sprite.rectDest.Y = -(yOffset * i);
                enemy.actionFloat1 = actionFloat1;
                enemy.actionFloat2 = actionFloat2;
                enemy.SetCharacteristics();
                enemyIndex.Add(enemy);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        /// <summary>
        /// Draws the player and enemies.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            objPlayer.Draw(spriteBatch);
            foreach (ObjEnemy enemy in enemyIndex)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (ObjBullet bullet in bulletIndex)
            {
                bullet.Draw(spriteBatch);
            }
            foreach (ObjMissile missile in missileIndex)
            {
                missile.Draw(spriteBatch);
            }
            foreach (ObjFragment fragment in fragmentIndex)
            {
                fragment.Draw(spriteBatch);
            }
            foreach (ObjExplosion explosion in explosionIndex)
            {
                explosion.Draw(spriteBatch);
            }
            if (waveNameTime > 0)
            {
                string waveText = "WAVE   " + (wave + 1);
                spriteBatch.DrawString(game.fntHeadline, waveText,
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width / 2 -
                        (game.fntHeadline.MeasureString(waveText).X / 2),
                        game.GraphicsDevice.Viewport.Height / 2),
                    Color.Silver);
            }
        }
    }
}
