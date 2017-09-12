using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MeuJogo
{
    /* ---------------------------------------------------------------
     * Atributos da Mensagem
     * --------------------------------------------------------------- */
    public class Mensagem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteFont FonteTexto;
        SpriteBatch spriteBatch;
        private string texto;
        private float posX;
        private float posY;

        /* ---------------------------------------------------------------
         * Construtor da Mensagem
         * --------------------------------------------------------------- */
        public Mensagem(Game game)
            : base(game)
        {
            this.texto = "";
            this.posX = 0;
            this.posY = 0;
        }

        /* ---------------------------------------------------------------
         * Carrega elementos da Mensagem
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.FonteTexto = Game.Content.Load<SpriteFont>("Mensagem");
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza Mensagem
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenha Mensagem
         * --------------------------------------------------------------- */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                this.FonteTexto,
                this.texto.ToString(),
                new Vector2(this.posX, this.posY),
                Color.WhiteSmoke
            );
            spriteBatch.End();
        }

        /* ---------------------------------------------------------------
         * Seta texto da mensagem
         * --------------------------------------------------------------- */
        public void SetaPosicao(float x, float y)
        {
            this.posX = x;
            this.posY = y;
        }
        public void SetaTextoCentro(float x, float y, string t)
        {
            Vector2 tamanhoTexto = this.FonteTexto.MeasureString(t);
            this.posX = x - ((int)tamanhoTexto.X / 2.1f);
            this.posY = y - ((int)tamanhoTexto.Y / 2.1f);
            this.texto = t.ToUpper();
        }
        public void SetaTexto(string t)
        {
            this.texto = t;
        }
    }
}
