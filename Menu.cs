using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MeuJogo
{
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        private Texture2D Textura;

        public Menu(Game game)
            : base(game)
        { 
        }

        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Textura = Game.Content.Load<Texture2D>("menu");
            base.LoadContent();
        }
            
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                this.Textura, 
                new Vector2(0,0), 
                null,
                Color.White, 
                0, 
                new Vector2(0,0),
                1, 
                SpriteEffects.None, 
                0f
            );
            spriteBatch.End();
        }
    }
}
