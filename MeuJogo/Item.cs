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
     * Atributos do Item
     * --------------------------------------------------------------- */
    public class Item : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Estados { Show, Hide }

        SpriteBatch spriteBatch;
        private Texture2D Textura;
        public Vector2 Posicao;
        private Estados Estado;
        private Vector2 Frame;
        public Rectangle BoundingBox;
        private Vector2 Tamanho;
        private string Nome;
        private int Qtde;
        //private SoundEffect SomColisao;

        /* ---------------------------------------------------------------
         * Construtores do Item
         * --------------------------------------------------------------- */
        public Item(Game game, Vector2 posicao, Vector2 tamanho, string nome)
            : base(game)
        {
            this.Posicao = posicao;
            this.Tamanho = tamanho;
            this.Frame = new Vector2(0, 0);
            this.Nome = nome;
            this.Qtde = 0;
            this.BoundingBox = new Rectangle((int)this.Posicao.X,
                                             (int)this.Posicao.Y,
                                             (int)this.Tamanho.X,
                                             (int)this.Tamanho.Y);
        }

        /* ---------------------------------------------------------------
         * Inicializacao do Item
         * --------------------------------------------------------------- */
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega elementos do Item
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Textura = Game.Content.Load<Texture2D>(this.Nome);
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza Item
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            // atualiza frame do sprite
            if (this.Estado == Estados.Show)
            {
                this.Frame.X = 0;
                this.Frame.Y = 0;
            }
            else if (this.Estado == Estados.Hide)
            {
                this.Frame.X = 0;
                this.Frame.Y = 0;
            }

            this.BoundingBox = new Rectangle((int)Posicao.X, 
                                             (int)Posicao.Y, 
                                             (int)Tamanho.X, 
                                             (int)Tamanho.Y);
            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenha Item
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
                1f
            );

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Demais funcoes auxiliares
         * --------------------------------------------------------------- */
        public void SemAcao()
        {
            this.Estado = Estados.Show;
        }

        public void RollLeft(float r)
        {
            this.Posicao.X += r;
        }

        public void RollRight(float r)
        {
            this.Posicao.X -= r;
        }

        public void RollUp(float r)
        {
            this.Posicao.Y += r;
        }

        public void RollDown(float r)
        {
            this.Posicao.Y -= r;
        }

        public void NovaPosicao(int andaX, int y)
        {
            this.Posicao.Y  = y;
            this.Posicao.X += andaX;
            if (this.Qtde > 3)
                this.Esconder();
            this.Qtde++;
        }

        public void Esconder()
        {
            this.Posicao = new Vector2(-500, -500);
        }
    }
}
