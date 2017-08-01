using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FleetFighter
{
    /// <summary>
    /// Draws the titles and controls the menus
    /// </summary>
    public class ObjTitleScreen
    {
        public MainLoop game;
        public Texture2D[] texture = new Texture2D[12];
        public ObjMenuItem[] menuItem = new ObjMenuItem[12];
        public List<ObjMenuItem> buttonIndex;
        public string highscoreName = ""; //so that it doesn't get reset.

        public ObjTitleScreen(MainLoop game)
        {
            this.game = game;
            buttonIndex = new List<ObjMenuItem>();
        }

        public void LoadContent(ContentManager Content)
        {
            texture[0] = Content.Load<Texture2D>("TexTitleLogo");
            texture[1] = Content.Load<Texture2D>("TexTitleCopyright");
            texture[2] = Content.Load<Texture2D>("TexTitleBack");
            texture[3] = Content.Load<Texture2D>("TexTitlePlay");
            texture[4] = Content.Load<Texture2D>("TexTitleHowtoPlay");
            texture[5] = Content.Load<Texture2D>("TexTitleHelpInstructions");
            texture[6] = Content.Load<Texture2D>("TexTitleOptions");
            texture[7] = Content.Load<Texture2D>("TexTitleOpsToggleAudio");
            texture[8] = Content.Load<Texture2D>("TexTitleHighscores");
            texture[9] = Content.Load<Texture2D>("TexTitleClearHighscores");
            texture[10] = Content.Load<Texture2D>("TexTitleCredits");
            texture[11] = Content.Load<Texture2D>("TexTitleCreditations");

            menuItem[0] = new ObjMenuItem(texture[0], 0, 0);
            menuItem[1] = new ObjMenuItem(texture[1], 0, game.GraphicsDevice.Viewport.Height - texture[1].Height);
            menuItem[2] = new ObjMenuItem(texture[2], 0, 0);
            menuItem[3] = new ObjMenuItem(texture[3], 0, 64 + texture[0].Height);
            menuItem[4] = new ObjMenuItem(texture[4], 0, 108 + texture[0].Height);
            menuItem[5] = new ObjMenuItem(texture[5], 0, texture[0].Height);
            menuItem[6] = new ObjMenuItem(texture[6], 0, 160 + texture[0].Height);
            menuItem[7] = new ObjMenuItem(texture[7], 0, 64 + texture[0].Height);
            menuItem[8] = new ObjMenuItem(texture[8], 0, 208 + texture[0].Height);
            menuItem[9] = new ObjMenuItem(texture[9], 0, 108 + texture[0].Height);
            menuItem[10] = new ObjMenuItem(texture[10], 0, 252 + texture[0].Height);
            menuItem[11] = new ObjMenuItem(texture[11], 0, 64 + texture[0].Height);

            //Horizontally centers each menu item.
            for (int i = 0; i < menuItem.Count<ObjMenuItem>(); i++)
            {
                //Don't center the back button.
                if (i == 2)
                {
                    continue;
                }
                menuItem[i].sprite.rectDest.X = game.GraphicsDevice.Viewport.Width / 2 -
                    (menuItem[i].sprite.texture.Width / 2);
            }

            //Sets the list of current buttons.
            UpdateButtons(RoomIndex.rmTitle);
        }

        /// <summary>
        /// Updates the buttons in use for checking in the Update() method.
        /// </summary>
        public void UpdateButtons(RoomIndex room)
        {
            buttonIndex = new List<ObjMenuItem>();

            switch (room)
            {
                case (RoomIndex.rmInfo):
                {
                    buttonIndex.Add(menuItem[0]);
                    buttonIndex.Add(menuItem[1]);
                    buttonIndex.Add(menuItem[2]);
                    buttonIndex.Add(menuItem[5]);
                    break;
                }
                case (RoomIndex.rmOps):
                {
                    buttonIndex.Add(menuItem[0]);
                    buttonIndex.Add(menuItem[1]);
                    buttonIndex.Add(menuItem[2]);
                    buttonIndex.Add(menuItem[7]);
                    buttonIndex.Add(menuItem[9]);
                    break;
                }
                case (RoomIndex.rmTitle):
                {
                    buttonIndex.Add(menuItem[0]);
                    buttonIndex.Add(menuItem[1]);
                    buttonIndex.Add(menuItem[3]);
                    buttonIndex.Add(menuItem[4]);
                    buttonIndex.Add(menuItem[6]);
                    buttonIndex.Add(menuItem[8]);
                    buttonIndex.Add(menuItem[10]);
                    break;
                }
                case (RoomIndex.rmHighscore):
                {
                    buttonIndex.Add(menuItem[0]);
                    buttonIndex.Add(menuItem[1]);
                    buttonIndex.Add(menuItem[2]);
                    break;
                }
                case (RoomIndex.rmCredits):
                {
                    buttonIndex.Add(menuItem[2]);
                    buttonIndex.Add(menuItem[11]);
                    break;
                }
            }
        }

        /// <summary>
        /// Responds to user actions.
        /// </summary>
        public void Update(MainLoop game, MouseState mouseStateOld)
        {
            //Checks for input and acts accordingly
            foreach (ObjMenuItem button in buttonIndex)
            {
                if (button == menuItem[0] ||
                    button == menuItem[1] ||
                    button == menuItem[5])
                {
                    continue;
                }
                button.Update(game.objCrosshair);
                if (button.isHovered)
                {
                    //Determines button color when highlighted.
                    switch (game.room)
                    {
                        case (RoomIndex.rmTitle):
                            button.sprite.color = Color.Yellow;
                            break;
                        case (RoomIndex.rmOps):
                            button.sprite.color = Color.Red;
                            break;
                    }

                    //The back button is always blue when highlighted.
                    if (button == menuItem[2])
                    {
                        button.sprite.color = Color.Blue;
                    }

                    //If the button is just pressed
                    if (game.objCrosshair.mouseState.LeftButton == ButtonState.Pressed &&
                        mouseStateOld.LeftButton == ButtonState.Released)
                    {
                        //Back button
                        if (button == menuItem[2])
                        {
                            if (game.room == RoomIndex.rmInfo ||
                                game.room == RoomIndex.rmOps ||
                                game.room == RoomIndex.rmHighscore ||
                                game.room == RoomIndex.rmCredits)
                            {
                                Sound.Play(game.sfxButtonExit, Convert.ToInt16(!game.isMuted) * 0.5f);
                                game.room = RoomIndex.rmTitle;
                                game.Window.Title = "Fleet Fighter: Menu";
                                UpdateButtons(game.room);
                            }
                        }
                        //Play button
                        if (button == menuItem[3])
                        {
                            Sound.Play(game.sfxButtonClick, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmPlay;
                            game.Window.Title = "Fleet Fighter";
                            UpdateButtons(game.room);
                        }
                        //How to play button
                        if (button == menuItem[4])
                        {
                            Sound.Play(game.sfxButtonClick, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmInfo;
                            game.Window.Title = "Fleet Fighter: Instructions";
                            UpdateButtons(game.room);
                        }
                        //Options button
                        else if (button == menuItem[6])
                        {
                            Sound.Play(game.sfxButtonClick, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmOps;
                            game.Window.Title = "Fleet Fighter: Options";
                            UpdateButtons(game.room);
                        }
                        //Toggle Audio button
                        else if (button == menuItem[7])
                        {
                            game.isMuted = !game.isMuted;
                            game.sfxPlaylist.Pause();
                            if (!game.isMuted)
                            {
                                Sound.Play(game.sfxButtonConfirm, Convert.ToInt16(!game.isMuted) * 0.5f);
                                game.SetMusic();
                            }
                        }
                        //Highscores button
                        else if (button == menuItem[8])
                        {
                            Sound.Play(game.sfxButtonClick, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmHighscore;
                            game.Window.Title = "Fleet Fighter: Highscores";
                            UpdateButtons(game.room);
                        }
                        //Clear Highscores button
                        else if (button == menuItem[9])
                        {
                            Sound.Play(game.sfxButtonConfirm, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmTitle;
                            game.Window.Title = "Fleet Fighter: Menu";
                            for (int i = 0; i < game.highscores.numHighScores; i++)
                            {
                                game.highscores.PlayerName[i] = "nobody";
                                game.highscores.Score[i] = 0;
                                game.highscores.Wave[i] = 0;
                                game.highscores.SaveHighScores();
                            }
                            UpdateButtons(game.room);
                        }
                        //Credits button
                        else if (button == menuItem[10])
                        {
                            Sound.Play(game.sfxButtonClick, Convert.ToInt16(!game.isMuted) * 0.5f);
                            game.room = RoomIndex.rmCredits;
                            game.Window.Title = "Fleet Fighter: Credits";
                            UpdateButtons(game.room);
                        }
                    }
                }
                else
                {
                    button.sprite.color = Color.White;
                }
            }
        }

        /// <summary>
        /// Call this in the main draw loop.
        /// </summary>
        public void Draw(
            SpriteBatch spriteBatch,
            RoomIndex room,
            KeyboardState keyState,
            KeyboardState keyStateOld)
        {
            foreach (ObjMenuItem item in buttonIndex)
            {
                item.Draw(spriteBatch);
            }
            if (room == RoomIndex.rmHighscore)
            {
                game.highscores.DrawHighScores(game, spriteBatch);

                if (game.hasHighscore)
                {
                    game.hasHighscore = !game.highscores.DrawCurrentHighscore
                        (game, keyState, keyStateOld, spriteBatch, ref highscoreName);
                }
            }
        }
    }
}