using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;
using System;
using System.IO;

namespace FleetFighter
{
    public class ObjHighscoreData
    {
        public string[] PlayerName;
        public int[] Score, Wave;

        public int numHighScores;
        public string filePath, fileName;

        /// <summary>
        /// Instantiates a highscores object.
        /// </summary>
        /// <param name="numHighScores">The number of highscores.</param>
        /// <param name="highScoreFilename">The name of the highscore file.</param>
        public ObjHighscoreData(int numHighScores, string highScoreFilename)
        {
            PlayerName = new string[numHighScores];
            Score = new int[numHighScores];
            Wave = new int[numHighScores];

            //Set the number of highscores.
            this.numHighScores = numHighScores;

            // Set the path of the highscores to be saved, opened, or written.
            fileName = highScoreFilename;
            filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            //Creates an empty highscore list.
            for (int i = 0; i < numHighScores; i++)
            {
                PlayerName[i] = "nobody";
                Wave[i] = 0;
                Score[i] = 0;
            }

            //Loads a highscore list if one exists.
            if (FileExists())
            {
                LoadHighScores();
            }

            //Saves all highscores to create a list otherwise.
            else
            {
                SaveHighScores();
            }
        }

        /// <summary>
        /// Returns whether or not the file exists.
        /// </summary>
        public bool FileExists()
        {
            // Check to see if the save exists
            if (!File.Exists(filePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Saves the highscores to the specified file.
        public void SaveHighScores()
        {
            // Open the file, creating it if necessary
            FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                //Creates a new stream writer.
                StreamWriter sw = new StreamWriter(stream);

                //Writes everything to the file (replacing it).
                for (int i = 0; i < numHighScores; i++)
                {
                    sw.WriteLine(PlayerName[i]);
                    sw.WriteLine(Score[i].ToString());
                    sw.WriteLine(Wave[i].ToString());
                }

                stream.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("in the highscore saving method: " + e.Message);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        //Loads the highscores from a specified file.
        public void LoadHighScores()
        {
            // Open the file
            FileStream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read);

            try
            {
                //Creates a new stream reader.
                StreamReader sr = new StreamReader(stream);
                
                //Sets an array containing the number of lines.
                string[] line = new string[3 * numHighScores];

                //Gets the lines in the file.
                for (int i = 0; i < line.Length; i++)
                {
                    if (!sr.EndOfStream)
                    {
                        line[i] = sr.ReadLine();
                    }
                    else
                    {
                        throw new Exception("Unexpected end of stream reached.");
                    }
                }

                //Throws an error if there aren't the right number of lines.
                if (line.Length % 3 != 0 || line.Length == 0)
                {
                    throw new Exception("Mismatching number of lines in highscore file.");
                }

                //Sets all of the attributes.
                for (int i = 0; i < (line.Length / 3); i++)
                {
                    PlayerName[i] = line[i * 3];
                    Score[i] = Convert.ToInt16(line[i * 3 + 1]);
                    Wave[i] = Convert.ToInt16(line[i * 3 + 2]);
                }

                stream.Flush();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("in the highscore loading method: " + e.Message);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        //Returns whether or not your score is a highscore.
        public bool IsHighscore(int score)
        {
            int scoreIndex = -1;
            for (int i = 0; i < numHighScores; i++)
            {
                if (score > this.Score[i])
                {
                    scoreIndex = i;
                    return true;
                }
            }

            return false;
        }

        //Adds a new highscore into the list and saves it.
        public void AddScore(int wave, int score, string name)
        {
            int scoreIndex = -1;
            for (int i = 0; i < numHighScores; i++)
            {
                if (score > Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            //If a new score was achieved, cascade scores downwards for room.
            if (scoreIndex > -1)
            {
                for (int i = numHighScores - 1; i > scoreIndex; i--)
                {
                    PlayerName[i] = PlayerName[i - 1];
                    Score[i] = Score[i - 1];
                    Wave[i] = Wave[i - 1];
                }

                PlayerName[scoreIndex] = name;
                Score[scoreIndex] = score;
                Wave[scoreIndex] = wave + 1;

                SaveHighScores();
            }
        }

        //Draws the current highscore list.
        public void DrawHighScores(MainLoop game, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < numHighScores; i++)
            {
                String formattedText = PlayerName[i] + " | score: " + Score[i] + "... wave " + Wave[i];
                spriteBatch.DrawString(game.fntRegular,
                formattedText,
                    new Vector2(game.GraphicsDevice.Viewport.Width / 2 -
                        (game.fntRegular.MeasureString(formattedText).X / 2),
                        200 + 20 * i),
                    Color.White);
            }
        }

        //Draws the user as the user enters a new highscore (and handles input processing).
        //Returns whether or not the operation has finished.
        public bool DrawCurrentHighscore(MainLoop game, KeyboardState keyState, KeyboardState keyStateOld, SpriteBatch spriteBatch, ref string userInput)
        {
            //Gets the current keys being pressed and iterated through them.
            Keys[] keysPressed = keyState.GetPressedKeys();
            for (int i = 0; i < (keysPressed.Length); i++)
            {
                //Gets the states of the keyboard.
                if (keyState.IsKeyDown(keysPressed[i]) && keyStateOld.IsKeyUp(keysPressed[i]))
                {
                    if (keysPressed[i] == Keys.Back && userInput.Length > 0)
                    {
                        userInput = userInput.Remove(userInput.Length - 1, 1);
                    }
                    //else if (keysPressed[i] == Keys.Space)
                    //{
                    //    userInput = userInput.Insert(userInput.Length, " ");
                    //}
                    else if (keysPressed[i] == Keys.Delete)
                    {
                        userInput = "";
                    }
                    //Sets a maximum character cap.
                    if (userInput.Length < 30)
                    {
                        //Gets the smart string representation of the key.
                        if (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))
                        {
                            userInput += CtrlKeyboard.KeyToString(keysPressed[i], true);
                        }
                        else
                        {
                            userInput += CtrlKeyboard.KeyToString(keysPressed[i], false);
                        }
                    }
                }
            }
            //Draws the current highscore.
            string text = "New highscore! Type your name:";
            spriteBatch.DrawString(game.fntRegular, text,
                new Vector2(game.GraphicsDevice.Viewport.Width / 2 - (game.fntRegular.MeasureString(text).X / 2), 160),
                Color.White);

            spriteBatch.DrawString(game.fntRegular, userInput,
                new Vector2(game.GraphicsDevice.Viewport.Width / 2 - (game.fntRegular.MeasureString(userInput).X / 2),
                    180),
                Color.Lime);
                
            //Checks if a confirmation key is pressed to finalize the values.
            if (keyState.IsKeyDown(Keys.Enter))
            {
                if (userInput == "")
                {
                    userInput = "no name";
                }
                Sound.Play(game.sfxButtonConfirm, Convert.ToInt16(!game.isMuted) * 0.5f);
                AddScore(game.HighscoreWave, game.HighscoreScore, userInput);
                return true;
            }
            else
            {
                return false;
            }
        }     
    }
}
