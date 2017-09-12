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
     * Atributos do Life
     * --------------------------------------------------------------- */
    public class Life : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        private Texture2D Textura;
        public Vector2 Posicao;
        private Vector2 Frame;
        private Vector2 Tamanho;
        private int Vida;

        /* ---------------------------------------------------------------
         * Construtores do Life
         * --------------------------------------------------------------- */
        public Life(Game game)
            : base(game)
        {
            this.Posicao = new Vector2(Constante.PosicaoBarraVidaX, Constante.PosicaoBarraVidaY);
            this.Tamanho = new Vector2(100, 15.53f);
            this.Frame = new Vector2(0, 0);
            this.Vida = 0;
        }

        /* ---------------------------------------------------------------
         * Inicializacao do Life
         * --------------------------------------------------------------- */
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega elementos do Life
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Textura = Game.Content.Load<Texture2D>("hp_bar");
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza Life
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            // atualiza frame do sprite
            int BarraHP = (int)((Constante.TotalVida - this.Vida) / (Constante.TotalVida / 10));
            this.Frame.X = 0;
            this.Frame.Y =  BarraHP * this.Tamanho.Y;

            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenha Life
         * --------------------------------------------------------------- */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                this.Textura,
                new Rectangle((int)this.Posicao.X, (int)this.Posicao.Y, (int)this.Tamanho.X, (int)this.Tamanho.Y),
                new Rectangle((int)this.Frame.X, (int)this.Frame.Y, (int)this.Tamanho.X, (int)this.Tamanho.Y),
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Demais funcoes auxiliares
         * --------------------------------------------------------------- */
        public void AtualizaVida(int vida)
        {
            this.Vida = vida;
        }
        public void SetaPosicao(Vector2 posicao)
        {
            this.Posicao = posicao;
            this.Posicao.X -= 15;
            this.Posicao.Y -= 30;
        }
    }
}
