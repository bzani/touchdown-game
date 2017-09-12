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
     * Atributos do Personagem
     * --------------------------------------------------------------- */
    public class Personagem : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Comandos { 
            Pula, Rasteira, 
            Sobe, Desce, 
            AndaTras, AndaFrente,
            Ataca, 
            Para, Comemora }
        public enum Estados 
        {
            Correndo, Pulando, 
            Subindo, Descendo, 
            AndandoTras, AndandoFrente,
            Rasteira, Atacando,
            Caindo, Parado, Comemorando
        }

        SpriteBatch spriteBatch;
        SpriteFont FonteVida;
        private Texture2D Textura;
        private Vector2 Posicao;
        private Vector2 Velocidade;
        private Estados Estado;
        private Vector2 Frame;
        private Comandos Comando;
        public int Vida;
        private int Delay;
        private int LifeTime;
        private int VelocidadePulo;
        private float AlturaPuloInicial;
        public bool flip;
        public Rectangle BoundingBox;
        private Vector2 Tamanho;
        private SoundEffect SomVida;
        private SoundEffectInstance SoundInstance;

        /* ---------------------------------------------------------------
         * Construtor do Personagem
         * --------------------------------------------------------------- */
        public Personagem(Game game)
            : base(game)
        {
            this.Posicao = new Vector2(150, 300);
            this.Velocidade = new Vector2(1, 1);
            this.VelocidadePulo = 3;
            this.Tamanho = new Vector2(40, 50);
            this.Estado = Estados.Parado;
            this.Frame = new Vector2(0, 0);
            this.Vida = Constante.TotalVida;
            this.BoundingBox = new Rectangle(BoundingCentroX() - 1,
                                             BoundingCentroY() - 1,
                                             BoundingCentroX() + 1,
                                             BoundingCentroY() + 1);
        }

        /* ---------------------------------------------------------------
         * Inicializacao do Personagem
         * --------------------------------------------------------------- */
        public override void Initialize()
        {
            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega elementos do Personagem
         * --------------------------------------------------------------- */
        public void LoadContent(Game game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Textura = Game.Content.Load<Texture2D>("yoshi_azul");
            this.SomVida = Game.Content.Load<SoundEffect>("som_lifeup");
            this.SoundInstance = SomVida.CreateInstance();
            this.SoundInstance.Volume = 0.1f;
            this.FonteVida = Game.Content.Load<SpriteFont>("Vida");
            base.LoadContent();
        }

        /* ---------------------------------------------------------------
         * Atualiza personagem
         * --------------------------------------------------------------- */
        public override void Update(GameTime gameTime)
        {
            // atualiza frame do sprite
            if (this.AplicaDelay(3))
                this.Frame.X += this.Tamanho.X;

            // verifica e seta sprite correspondente ao estado
            if (this.Estado == Estados.Correndo ||
                this.Estado == Estados.Subindo  ||
                this.Estado == Estados.Descendo ||
                this.Estado == Estados.AndandoTras ||
                this.Estado == Estados.AndandoFrente
               )
            {
                if (this.Frame.X > 3 * this.Tamanho.X)
                    this.Frame.X = 0;
                this.Frame.Y = 2 * this.Tamanho.Y;
            }
            else if (this.Estado == Estados.Parado)
            {
                //if (this.Frame.X > 3 * this.Tamanho.X)
                    this.Frame.X = 1 * this.Tamanho.X;
                this.Frame.Y = 0;
            }
            else if (this.Estado == Estados.Comemorando)
            {
                if (this.Frame.X > 6 * this.Tamanho.X)
                    this.Frame.X = this.Tamanho.X;
                this.Frame.Y = 3 * this.Tamanho.Y;
            }
            this.BoundingBox = new Rectangle((int)Posicao.X, (int)Posicao.Y, (int)Tamanho.X, (int)Tamanho.Y);
            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenha Personagem
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
                (this.Estado == Estados.AndandoTras) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f
            );

            spriteBatch.DrawString(
                this.FonteVida,
                "HP", //this.Vida.ToString(),
                new Vector2(Constante.PosicaoBarraVidaX - 25, Constante.PosicaoBarraVidaY),
                Color.White
            );
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Verifica estado do Personagem 
         *  => pulando/caindo bloqueia acoes
         *  => pulando/caindo atualiza 
         * --------------------------------------------------------------- */
        public bool VerificaEstado()
        {
            if (this.Estado == Estados.Pulando) {
                this.Posicao.Y -= this.VelocidadePulo;
                if (flip) 
                    this.Posicao.X -= this.VelocidadePulo;
                else 
                    this.Posicao.X += this.VelocidadePulo;
                if (this.Posicao.Y <= this.AlturaPuloInicial - Constante.AlturaPulo)
                    this.Estado = Estados.Caindo;
                return false;
            }
            else if (this.Estado == Estados.Caindo)
            {
                this.Posicao.Y += this.VelocidadePulo;
                if (flip) 
                    this.Posicao.X -= this.VelocidadePulo;
                else 
                    this.Posicao.X += this.VelocidadePulo;
                if (this.Posicao.Y >= this.AlturaPuloInicial)
                    this.Estado = Estados.Correndo;
                return false;
            }
            return true;
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
                    this.AlturaPuloInicial = this.Posicao.Y;
                    break;
                case Comandos.Sobe:
                    this.Estado = Estados.Subindo;
                    this.Posicao.Y -= this.Velocidade.Y;
                    break;
                case Comandos.Desce:
                    this.Estado = Estados.Descendo;
                    this.Posicao.Y += this.Velocidade.Y;
                    break;
                case Comandos.AndaFrente:
                    this.Estado = Estados.AndandoFrente;
                    this.Posicao.X += this.Velocidade.X;
                    this.flip = false;
                    break;
                case Comandos.AndaTras:
                    this.Estado = Estados.AndandoTras;
                    this.Posicao.X -= this.Velocidade.X;
                    this.flip = true;
                    break;
                case Comandos.Ataca:
                    this.Estado = Estados.Atacando;
                    break;
                case Comandos.Para:
                    this.Estado = Estados.Parado;
                    break;
                case Comandos.Comemora:
                    this.Estado = Estados.Comemorando;
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

        public float PegaTamanhoY()
        {
            return this.Tamanho.Y;
        }

        public float PegaPosicaoY()
        {
            return this.Posicao.Y;
        }

        public float PegaPosicaoX()
        {
            return this.Posicao.X;
        }

        public void SofrerDano(int d)
        {
            if ((this.Vida > 0) && !(this.Estado == Estados.Pulando || this.Estado == Estados.Caindo || Constante.modoTeste))
                this.Vida -= d;
        }

        public void GanhaVida(int d)
        {
            this.Vida = this.Vida + d;
            if (this.Vida > Constante.TotalVida)
                this.Vida = Constante.TotalVida;
            this.SoundInstance.Play();
        }

        public bool AplicaDelay(int d)
        {
            if (this.Delay > d)
            {
                this.Delay = 0;
                return true;
            }
            this.Delay++;
            return false;
        }

        public bool DescontaTempoVida()
        {
            if (this.LifeTime > 60)
            {
                this.LifeTime = 0;
                this.SofrerDano(1);
                return true;
            }
            this.LifeTime++;
            return false;
        }

        public bool TemVida()
        {
            return (this.Vida > 0) ? true : false;
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
