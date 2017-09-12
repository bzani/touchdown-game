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
     * Atributos do Fundo Animado
     * --------------------------------------------------------------- */
    // -- screen   800 x 480
    // -- backgr  2048 x 456
    public class ScrollingBackground : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Estados { Parado, Correndo }
        public enum Animacao { Para, Corre }

        SpriteBatch spriteBatch;
        private Texture2D Textura;
        private Vector2 PosicaoAtual;
        private Vector2 PosicaoOrigem;
        private Vector2 Velocidade;

        /* ---------------------------------------------------------------
         * Carrega Fundo Animado
         * --------------------------------------------------------------- */
        public ScrollingBackground(Game game)
            : base(game)
        {
            this.PosicaoOrigem = new Vector2(Constante.BackgroundOrigemX, Constante.BackgroundOrigemY);
            this.PosicaoAtual = new Vector2(-50, -80);
            this.Velocidade = new Vector2(2, 1);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega Fundo Animado
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            // Device
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Textura
            this.Textura = Game.Content.Load<Texture2D>("footballbg");
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza Fundo animado
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            // ...
        }

        /* ---------------------------------------------------------------
         * Desenha Fundo animado
         * --------------------------------------------------------------- */
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            spriteBatch.Draw(
                this.Textura, 
                this.PosicaoAtual,
                null,
                Color.White, 
                0,
                this.PosicaoOrigem,
                1, 
                SpriteEffects.None, 
                0f
            );

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Acoes de Movimento do Fundo
         * --------------------------------------------------------------- */
        public void RollUp(int v)
        {
            this.PosicaoAtual.Y += v;
        }

        public void RollDown(int v)
        {
            this.PosicaoAtual.Y -= v;
        }

        public void RollLeft(int v)
        {
            this.PosicaoAtual.X += v;
        }
                    
        public void RollRight(int v)
        {
            this.PosicaoAtual.X -= v;
        }
    }
}
