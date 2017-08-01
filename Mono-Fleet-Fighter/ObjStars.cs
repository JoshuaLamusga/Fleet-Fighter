using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// Generates the stars that scroll by in the background.
    /// </summary>
    public class ObjStars
    {
        public Texture2D texture;
        public Random random;
        private Game game;

        public int starsNum = 100;
        public Color[] color;
        public Sprite[] sprite;
        public SpriteAtlas[] spriteAtlas;
        public SpritePhysics[] spritePhysics;

        public ObjStars(Game game)
        {
            this.game = game;
            random = new Random();
            sprite = new Sprite[100];
            spriteAtlas = new SpriteAtlas[100];
            spritePhysics = new SpritePhysics[100];
            color = new Color[100];
        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("TexStars");

            //Generates stars
            for (int i = 0; i < starsNum; i++)
            {
                sprite[i] = new Sprite(false, texture);
                spriteAtlas[i] = new SpriteAtlas(sprite[i], 8, 8, 8, 1, 8);
                spriteAtlas[i].frame = random.Next(0, spriteAtlas[i].frames);
                spriteAtlas[i].Update(true);
                spritePhysics[i] = new SpritePhysics(sprite[i]);
                sprite[i].color = new Color(random.Next(0, 128),
                    random.Next(0, 128),
                    random.Next(0, 128));
                sprite[i].rectDest = new SmoothRect(
                    random.Next(0, game.GraphicsDevice.Viewport.Width),
                    -spriteAtlas[i].frameHeight - random.Next(0, 100),
                    spriteAtlas[i].frameWidth,
                    spriteAtlas[i].frameHeight);
                sprite[i].origin = new Vector2(
                    spriteAtlas[i].frameWidth / 2,
                    spriteAtlas[i].frameHeight / 2);
                sprite[i].drawBehavior = SpriteDraw.all;
                spritePhysics[i].YMove = (float)(1 + random.NextDouble() * 5); 
            }
        }

        /// <summary>
        /// Call this in the main update loop.
        /// </summary>
        public void Update()
        {
            //Moves stars and updates position
            for (int i = 0; i < starsNum; i++)
            {
                spritePhysics[i].Update();

                if (sprite[i].rectDest.Y > game.GraphicsDevice.Viewport.Height + texture.Height)
                {
                    //Makes the star seem like a different star
                    spritePhysics[i].YMove = (float)(2 + random.NextDouble() * 5); 
                    sprite[i].rectDest = new SmoothRect(
                        random.Next(0, game.GraphicsDevice.Viewport.Width),
                        -texture.Height,
                        spriteAtlas[i].frameWidth,
                        spriteAtlas[i].frameHeight);
                    sprite[i].color = new Color(random.Next(0, 128),
                        random.Next(0, 128),
                        random.Next(0, 128));                    
                }
            }
        }

        /// <summary>
        /// Call this in the main draw loop.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Generates stars
            for (int i = 0; i < starsNum; i++)
            {
                sprite[i].Draw(spriteBatch);
            }
        }
    }
}