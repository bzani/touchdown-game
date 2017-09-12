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
     * Atributos do Inimigo
     * --------------------------------------------------------------- */
    public class Inimigo : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Comandos { Pula, Abaixa, Ataca, Sobe, Desce, Para }
        public enum Estados { Correndo, Pulando, Rasteira, Atacando, Parado}

        SpriteBatch spriteBatch;
        //SpriteFont FonteVida;
        private Texture2D Textura;
        public Vector2 Posicao;
        private Vector2 Velocidade;
        private Estados Estado;
        private Vector2 Frame;
        private Comandos Comando;
        //private int Vida;
        private int DelaySprite;
        private int DelayAcao;
        private float PersonagemY;
        private float PersonagemX;
        private bool flip;
        public Rectangle BoundingBox;
        private Vector2 Tamanho;

        /* ---------------------------------------------------------------
         * Construtores do Inimigo
         * --------------------------------------------------------------- */
        public Inimigo(Game game, Vector2 posicao, Vector2 velocidade)
            : base(game)
        {
            this.Posicao = posicao;
            this.Velocidade = velocidade;
            this.Tamanho = new Vector2(40, 50);
            this.Estado = Estados.Parado;
            this.Frame = new Vector2(0, 0);
            //this.Vida = 100;
            this.BoundingBox = new Rectangle(BoundingCentroX() - 1,
                                             BoundingCentroY() - 1,
                                             BoundingCentroX() + 1,
                                             BoundingCentroY() + 1);
        }

        /* ---------------------------------------------------------------
         * Inicializacao do Inimigo
         * --------------------------------------------------------------- */
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega elementos do Inimigo
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Textura = Game.Content.Load<Texture2D>("yoshi_verm");
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza Inimigo
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            // atualiza frame do sprite
            if (this.AplicaDelaySprite(3))
                this.Frame.X += this.Tamanho.X;

            if (this.Estado == Estados.Correndo)
            {
                if (this.Frame.X > 2 * this.Tamanho.X)
                    this.Frame.X = 0;
                this.Frame.Y = 2 * this.Tamanho.Y;
            }
            else if (this.Estado == Estados.Parado)
            {
                this.Frame.X = 2 * this.Tamanho.X;
                this.Frame.Y = 0;
            }

            this.BoundingBox = new Rectangle((int)Posicao.X, (int)Posicao.Y, (int)Tamanho.X, (int)Tamanho.Y);
            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenha Inimigo
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
                (flip) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                1f
            );

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Captura Acao do Personagem pela entrada (keyboard)
         * --------------------------------------------------------------- */
        public void Acao(Comandos comando)
        {
            this.Comando = comando;
            switch(comando)
            {
                case Comandos.Pula:
                    this.Estado = Estados.Pulando;
                    this.Posicao.Y -= this.Velocidade.Y;
                    break;
                case Comandos.Abaixa:
                    this.Estado = Estados.Rasteira;
                    this.Posicao.Y += this.Velocidade.Y;
                    break;
                case Comandos.Ataca:
                    this.Estado = Estados.Atacando;
                    this.Posicao.Y += this.Velocidade.Y;
                    break;
                case Comandos.Sobe:
                    this.Posicao.Y -= this.Velocidade.Y;
                    break;
                case Comandos.Desce:
                    this.Posicao.Y += this.Velocidade.Y;
                    break;
                case Comandos.Para:
                    this.Estado = Estados.Parado;
                    break;
            }
        }

        /* ---------------------------------------------------------------
         * Demais funcoes auxiliares
         * --------------------------------------------------------------- */
        public void SemAcao()
        {
            this.Estado = Estados.Parado;

        }

        public void MudarVelocidade(int v)
        {
            this.Velocidade = new Vector2(v, v);
        }

        public void MudarVelocidade(Vector2 velocidade)
        {
            this.Velocidade = velocidade;
        }

        public void PosicaoPersonagemY(float yP)
        {
            this.PersonagemY = yP;
        }

        public void PosicaoPersonagemX(float xP)
        {
            this.PersonagemX = xP;
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

        public bool AplicaDelayAcao(int d)
        {
            if (this.DelayAcao > d)
            {
                this.DelayAcao = 0;
                return true;
            }
            this.DelayAcao++;
            return false;
        }

        public bool AplicaDelaySprite(int d)
        {
            if (this.DelaySprite > d)
            {
                this.DelaySprite = 0;
                return true;
            }
            this.DelaySprite++;
            return false;
        }

        public void PerseguePersonagem()
        {
            // atualiza acao de perseguir personagem
            if (this.AplicaDelayAcao(0))
            {
                if (    ( ((this.Posicao.X - PersonagemX) < Constante.DistanciaPerseguicaoX) && 
                          ((this.Posicao.X - PersonagemX) > ((-1)*(Constante.DistanciaPerseguicaoX))) )
                            &&
                        (((this.Posicao.Y - PersonagemY) < Constante.DistanciaPerseguicaoY) && 
                          ((this.Posicao.Y - PersonagemY) > ((-1)*(Constante.DistanciaPerseguicaoY))) )
                   ) 
                {
                    this.Estado = Estados.Correndo;

                    if ((uint)(this.Posicao.X - PersonagemX) > 2)
                    {
                        if (PersonagemX < this.Posicao.X)
                        {
                            this.flip = false;
                            this.Posicao.X -= this.Velocidade.X;
                        }
                        else if (PersonagemX > this.Posicao.X)
                        {
                            this.flip = true;
                            this.Posicao.X += this.Velocidade.X;
                        }
                    }

                    if ((uint)(this.Posicao.Y - PersonagemY) > 2)
                    {
                        if (this.Posicao.Y > PersonagemY)
                            this.Posicao.Y -= this.Velocidade.Y;
                        else if (this.Posicao.Y < PersonagemY)
                            this.Posicao.Y += this.Velocidade.Y;
                    }
                }
                else this.Estado = Estados.Parado;
            }
        }

        public int BoundingCentroX()
        {
            return (int)(this.Posicao.X + (this.Tamanho.X / 2));
        }

        public int BoundingCentroY()
        {
            return (int)(this.Posicao.Y + (this.Tamanho.Y / 2));
        }
    }
}
