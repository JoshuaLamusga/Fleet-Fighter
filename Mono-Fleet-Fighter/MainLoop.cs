using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SimpleXnaFramework;
using SimpleXnaFramework.Framework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// The class containing the main game loop.
    /// </summary>
    public class MainLoop : Game
    {
        public GraphicsDeviceManager graphics; //Handles graphic display.
        public SpriteFont fntRegular; //The font for highscores.
        public SpriteFont fntBold; //The font for the hud in-game.
        public SpriteFont fntHeadline; //The large variant of the hud font.
        public Texture2D texHud; //The texture used for a hud.
        public Texture2D texHealth; //The texture of the healthbar.
        public Texture2D texBullet; //The texture of a bullet.
        public Texture2D texMissile; //The texture of a missile.
        public Texture2D texFragment; //The texture of a fragment.
        public Texture2D texExplosion; //The texture of an explosion.
        public Texture2D texPaused; //The texture for saying 'paused'.
        public Texture2D texDead; //The texture used when the player is dead.
        public Texture2D texMissileExplosion; //The texture used for an exploding missile.
        
        //The enemy textures.
        public Texture2D texEnemy01;
        public Texture2D texEnemy02;
        public Texture2D texEnemy03;
        public Texture2D texEnemy04;
        public Texture2D texEnemy05;
        public Texture2D texEnemy06;
        public Texture2D texEnemy07;
        
        //The music.
        public Song sfxMusicBattle1, sfxMusicBattle2;
        public Song sfxMusicBattle3, sfxMusicBattle4;
        public SoundPlaylist sfxPlaylist;

        public SoundEffect sfxBullet, sfxCollBullet, sfxMissile;
        public SoundEffect sfxCollPlayerEnemy; //also used for missile explosions.
        public SoundEffect sfxEnemyHit, sfxEnemyDestroyed;
        public SoundEffect sfxFragmentCollected, sfxFragmentCollected2;
        public SoundEffect sfxPlayerDies, sfxPlayerHit;
        public SoundEffect sfxWaveEnd, sfxWaveStart;
        public SoundEffect sfxButtonClick, sfxButtonExit;
        public SoundEffect sfxButtonConfirm, sfxHighscore;
        
        //Everything else.
        public SpriteBatch spriteBatch; //Draws everything in sync.
        public RoomIndex room; //Stores which room is active.
        public ObjCrosshair objCrosshair; //The cursor.
        public ObjStars objStars; //Background.
        public ObjTitleScreen objTitleScreen; //Controls titles.
        public ObjSpawnControl objSpawnControl; //Controls creation of player and enemies.
        public bool isPaused = false; //If the game is paused.
        public bool isDead = false; //If the player is dead.
        public bool hasHighscore = false; //If the player has a highscore.
        public int HighscoreScore = 0; //Necessary to preserve the score when everything is reset.
        public int HighscoreWave = 0; //Necessary to preserve the wave number when everything is reset.

        public ObjHighscoreData highscores; //The highscores.

        public bool isMuted = false; //Whether or not the audio is muted.

        public Random chance; //A random variable for easy usage by other classes.

        KeyboardState keyState, keyStateOld; //The old and current states of the keyboard.
        MouseState mouseStateOld; //The old state of the mouse (last frame).

        public MainLoop()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Centers the window.
            Window.Position = Window.ClientBounds.Center;
            Window.AllowUserResizing = true;

            //Initializes most of the objects.
            objCrosshair = new ObjCrosshair();
            objStars = new ObjStars(this);
            objTitleScreen = new ObjTitleScreen(this);
            objSpawnControl = new ObjSpawnControl(this);

            //Sets window properties.
            Window.Title = "Fleet Fighter: Menu";

            //Creates the highscores.
            highscores = new ObjHighscoreData(5, "highscores.lst");
            HighscoreScore = 0;
            HighscoreWave = 0;     

            //Creates the random value.
            chance = new Random();

            //Sets the keyboard and mouse state so the lines in update don't throw an error.
            keyState = Keyboard.GetState();
            mouseStateOld = Mouse.GetState();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            room = RoomIndex.rmTitle;
            fntRegular = Content.Load<SpriteFont>("FntRegular");
            fntBold = Content.Load<SpriteFont>("FntBold");
            fntHeadline = Content.Load<SpriteFont>("FntHeadline");
            texHud = Content.Load<Texture2D>("TexHud");
            texHealth = Content.Load<Texture2D>("TexHealthbar");
            texBullet = Content.Load<Texture2D>("TexBullet");
            texMissile = Content.Load<Texture2D>("TexMissile");
            texFragment = Content.Load<Texture2D>("TexFragments");
            texExplosion = Content.Load<Texture2D>("TexExplosion");
            texPaused = Content.Load<Texture2D>("TexPaused");
            texDead = Content.Load<Texture2D>("TexGameOver");
            texEnemy01 = Content.Load<Texture2D>("TexEnemy01");
            texEnemy02 = Content.Load<Texture2D>("TexEnemy02");
            texEnemy03 = Content.Load<Texture2D>("TexEnemy03");
            texEnemy04 = Content.Load<Texture2D>("TexEnemy04");
            texEnemy05 = Content.Load<Texture2D>("TexEnemy05");
            texEnemy06 = Content.Load<Texture2D>("TexEnemy06");
            texEnemy07 = Content.Load<Texture2D>("TexEnemy07");
            texMissileExplosion = Content.Load<Texture2D>("TexMissileExplosion");
            sfxBullet = Content.Load<SoundEffect>("SfxBullet");
            sfxCollBullet = Content.Load<SoundEffect>("SfxCollBullet");
            sfxMissile = Content.Load<SoundEffect>("SfxMissile");
            sfxCollPlayerEnemy = Content.Load<SoundEffect>("SfxCollPlayerEnemy");
            sfxEnemyHit = Content.Load<SoundEffect>("SfxEnemyHit");
            sfxEnemyDestroyed = Content.Load<SoundEffect>("SfxEnemyDestroyed");
            sfxFragmentCollected = Content.Load<SoundEffect>("SfxFragmentCollected");
            sfxFragmentCollected2 = Content.Load<SoundEffect>("SfxFragmentCollected2");
            sfxPlayerDies = Content.Load<SoundEffect>("SfxPlayerDies");
            sfxPlayerHit = Content.Load<SoundEffect>("SfxPlayerHit");
            sfxWaveEnd = Content.Load<SoundEffect>("SfxWaveEnd");
            sfxWaveStart = Content.Load<SoundEffect>("SfxWaveStart");
            /* TODO: Monogame doesn't support loading mp3 files.
            
            sfxMusicBattle1 = Content.Load<Song>("SfxMusicBattle1");
            sfxMusicBattle2 = Content.Load<Song>("SfxMusicBattle2");
            sfxMusicBattle3 = Content.Load<Song>("SfxMusicBattle3");
            sfxMusicBattle4 = Content.Load<Song>("SfxMusicBattle4");*/
            sfxButtonClick = Content.Load<SoundEffect>("SfxBttnClick");
            sfxButtonExit = Content.Load<SoundEffect>("SfxBttnExit");
            sfxButtonConfirm = Content.Load<SoundEffect>("SfxBttnConfirm");
            sfxHighscore = Content.Load<SoundEffect>("SfxHighscore");
            objCrosshair.LoadContent(Content);
            objStars.LoadContent(Content);
            objTitleScreen.LoadContent(Content);
            objSpawnControl.LoadContent(Content);

            sfxPlaylist = new SoundPlaylist();
            sfxPlaylist.music.Add(sfxMusicBattle1);
            sfxPlaylist.music.Add(sfxMusicBattle2);
            sfxPlaylist.music.Add(sfxMusicBattle3);
            sfxPlaylist.music.Add(sfxMusicBattle4);
            sfxPlaylist.Begin();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            //Gets the state of the keyboard and mouse.
            keyStateOld = keyState;
            mouseStateOld = objCrosshair.mouseState;
            keyState = Keyboard.GetState();
            objCrosshair.Update();

            //Allows music to shuffle to the next when done.
            sfxPlaylist.Update();

            //If the player is dead, checks if they press keys.
            //If they press back or click, the game is 'reset'.
            if (isDead)
            {
                if ((keyState.IsKeyDown(Keys.Back)) ||
                    (mouseStateOld.LeftButton == ButtonState.Pressed &&
                    objCrosshair.mouseState.LeftButton == ButtonState.Released))
                {
                    isDead = false;

                    //Gets whether or not the player achieved a highscore.
                    if (highscores.IsHighscore(objSpawnControl.objPlayer.score))
                    {
                        Sound.Play(sfxHighscore, Convert.ToInt16(!isMuted) * 0.25f);
                        hasHighscore = true;
                    }

                    HighscoreScore = objSpawnControl.objPlayer.score;
                    HighscoreWave = objSpawnControl.wave;

                    //Resets all of the properties so far by simply recreating most objects.
                    //The old objects are garbage collected.
                    objCrosshair = new ObjCrosshair();
                    objStars = new ObjStars(this);
                    objTitleScreen = new ObjTitleScreen(this);
                    objSpawnControl = new ObjSpawnControl(this);

                    objCrosshair.LoadContent(Content);
                    objStars.LoadContent(Content);
                    objTitleScreen.LoadContent(Content);
                    objSpawnControl.LoadContent(Content);

                    //Loads the highscore room if there was a new 
                    //highscore, otherwise returns to the title screen.
                    if (!hasHighscore)
                    {
                        Window.Title = "Fleet Fighter: Menu";
                        room = RoomIndex.rmTitle;
                        //Don't need to update buttons because they're there by default.
                    }
                    else
                    {
                        Window.Title = "Fleet Fighter: Highscores";
                        room = RoomIndex.rmHighscore;
                        objTitleScreen.UpdateButtons(room);
                    }
                }
            }

            //If P is released and the player isn't on the death title screen, then pause the game.
            if (!isDead && room == RoomIndex.rmPlay)
            {
                if (keyStateOld.IsKeyDown(Keys.P) && keyState.IsKeyUp(Keys.P))
                {
                    isPaused = !isPaused;
                    if (isPaused)
                    {
                        sfxPlaylist.Pause();
                    }
                    else
                    {
                        sfxPlaylist.Resume();
                    }
                }
            }

            if (!isPaused && !isDead)
            {
                //Allows the player to exit rooms consecutively, and the game if on the title screen.
                if (keyStateOld.IsKeyDown(Keys.Back) && keyState.IsKeyUp(Keys.Back) && hasHighscore == false)
                {
                    Sound.Play(sfxButtonExit, Convert.ToInt16(!isMuted) * 0.5f);

                    switch (room)
                    {
                        case RoomIndex.rmTitle:
                            highscores.SaveHighScores();
                            this.Exit();
                            break;
                        case RoomIndex.rmOps:
                            room = RoomIndex.rmTitle;
                            break;
                        case RoomIndex.rmInfo:
                            room = RoomIndex.rmTitle;
                            break;
                        case RoomIndex.rmPlay:
                            room = RoomIndex.rmTitle;
                            break;
                        case RoomIndex.rmHighscore:
                            room = RoomIndex.rmTitle;
                            break;
                    }
                    objTitleScreen.UpdateButtons(room);
                }

                //Updates all objects
                objCrosshair.Update();
                objStars.Update();

                switch (room)
                {
                    case (RoomIndex.rmPlay):
                    {
                        objSpawnControl.Update(keyState, keyStateOld, objCrosshair.mouseState, mouseStateOld);
                        break;
                    }
                    default:
                    {
                        objTitleScreen.Update(this, mouseStateOld);
                        break;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            objStars.Draw(spriteBatch);            
            switch (room)
            {
                case (RoomIndex.rmPlay):
                {
                    objSpawnControl.Draw(spriteBatch);
                    break;
                }
                default:
                {
                    objTitleScreen.Draw(spriteBatch, room, keyState, keyStateOld);
                    break;
                }
            }
            objCrosshair.Draw(spriteBatch);

            if (isPaused)
            {
                spriteBatch.Draw(texPaused,
                    new Rectangle(
                        (GraphicsDevice.Viewport.Width / 2),
                        (GraphicsDevice.Viewport.Height / 2),
                        texPaused.Width,
                        texPaused.Height),
                    new Rectangle(0, 0, texPaused.Width, texPaused.Height),
                    Color.White,
                    0,
                    new Vector2(texPaused.Width / 2, texPaused.Height / 2),
                    SpriteEffects.None,
                    0);
            }
            else if (isDead)
            {
                spriteBatch.Draw(texDead,
                    new Rectangle(
                        (GraphicsDevice.Viewport.Width / 2),
                        (GraphicsDevice.Viewport.Height / 2),
                        texDead.Width,
                        texDead.Height),
                    new Rectangle(0, 0, texDead.Width, texDead.Height),
                    Color.White,
                    0,
                    new Vector2(texDead.Width / 2, texDead.Height / 2),
                    SpriteEffects.None,
                    0);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets the music to play.
        /// </summary>
        internal void SetMusic()
        {
            //Sets the background music.
            sfxPlaylist.Begin();
        }
    }
}
