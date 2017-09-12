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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Atributos do dispositivo
        GraphicsDeviceManager graphics;

        // Estados do Jogo
        public enum Estado { Menu, Jogando, Fim}
        public enum EstadoM { Playing, Stopped }
        Estado GameState;
        EstadoM MusicState;

        // Auxiliares
        private int deslocamentoX;
        private int deslocamentoY;
        private int tela_largura;
        private int tela_altura;

        // Sprites
        SpriteBatch spriteBatch;
        Mensagem Texto;
        Life HP;

        // Personagens
        Personagem Yoshi;
        List<Inimigo> Inimigos;
        private int qtde_inimigos;

        // Itens
        Menu TelaMenu;
        Item Toad;
        Item Clock;

        // Background
        ScrollingBackground Background;

        // Audio
        Song Musica;
        

        /* ---------------------------------------------------------------
         * funcao do Jogo
         * --------------------------------------------------------------- */
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /* ---------------------------------------------------------------
         * Inicializacao do Jogo
         * --------------------------------------------------------------- */
        protected override void Initialize()
        {
            // Propriedades do display
            this.tela_altura = GraphicsDevice.Viewport.Height;
            this.tela_largura = GraphicsDevice.Viewport.Width;
            
            // Auxiliares
            this.deslocamentoX = 2;
            this.deslocamentoY = 1;

            // Menu
            TelaMenu = new Menu(this);
            TelaMenu.Initialize();
            this.GameState = Estado.Menu;

            // Background
            Background = new ScrollingBackground(this);
            Background.Initialize();

            // Personagem
            Yoshi = new Personagem(this);
            Yoshi.Initialize();
            
            // Inimigos
            Inimigos = new List<Inimigo>();
            this.qtde_inimigos = 11;
            Random r = new Random();
            for (int i = 0; i < this.qtde_inimigos; i++)
            {
                Inimigos.Add(
                    new Inimigo(this, new Vector2(AleatorioX(r, i), AleatorioY(r, i)), new Vector2(3, 3))
                );
                Inimigos[i].Initialize();
            }

            // Item
            Toad = new Item(this, 
                            new Vector2((int)(this.tela_largura / 2), AleatorioY(r)),
                            new Vector2(20, 27),
                            "lifeup"
                            );
            Toad.Initialize();
            Clock = new Item(this,
                            new Vector2((int)(Constante.LimiteCampoFim / 2) , AleatorioY(r)),
                            new Vector2(24, 24),
                            "clock"
                            );
            Clock.Initialize();

            // Mensagem
            
            Texto = new Mensagem(this);
            Texto.Initialize();

            // HP
            HP = new Life(this);
            HP.Initialize();

            base.Initialize();
        }

        /* ---------------------------------------------------------------
         * Carrega Elementos do Jogo
         * --------------------------------------------------------------- */
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Menu
            TelaMenu.LoadContent(this);

            // Mensagem
            Texto.LoadContent(this);
            
            // Personagem
            Yoshi.LoadContent(this);
            
            // Inimigos
            for (int i = 0; i < qtde_inimigos; i++)
                Inimigos[i].LoadContent(this);

            // Background
            Background.LoadContent(this);

            // Item
            Toad.LoadContent(this);
            Clock.LoadContent(this);

            // HP
            HP.LoadContent(this);

            // Musica de fundo
            this.Musica = Content.Load<Song>("thunderstruck");
        }

        protected override void UnloadContent()
        {
        }

        /* ---------------------------------------------------------------
         * Atualizacao do jogo
         * --------------------------------------------------------------- */
        protected override void Update(GameTime gameTime)
        {
            // Verifica sair de jogo
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Seta tempo de jogo
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Deixa tocando musica
            if (elapsed == 0f)
                MediaPlayer.Play(Musica);

            // Verifica estados do Jogo
            if (this.GameState == Estado.Menu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    this.GameState = Estado.Jogando;
                else
                    TelaMenu.Update(gameTime);
            }
                
            if (this.GameState == Estado.Fim) 
            {
                // Mostra mensagem de fim de jogo
                for (int i = 0; i < qtde_inimigos; i++)
                    Inimigos[i].Acao(Inimigo.Comandos.Para);

                if (Yoshi.TemVida())
                {
                    Yoshi.Acao(Personagem.Comandos.Comemora);
                                                                                 
                    Texto.SetaTextoCentro((tela_largura / 2), (tela_altura / 2), "Ganhou!");
                }
                else
                {
                    Yoshi.Acao(Personagem.Comandos.Para);
                    Texto.SetaTextoCentro((tela_largura / 2), (tela_altura / 2), "Perdeu!");
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    TelaMenu.Initialize();
                    Yoshi.Initialize();
                    for (int i = 0; i < this.qtde_inimigos; i++)
                        Inimigos[i].Initialize();
                    Texto.Initialize();
                    this.GameState = Estado.Jogando;
                }
            }

            else if (this.GameState == Estado.Jogando)
            {

                // Verifica perseguicao dos inimigos
                for (int i = 0; i < qtde_inimigos; i++)
                {
                    Inimigos[i].PosicaoPersonagemY(Yoshi.PegaPosicaoY());
                    Inimigos[i].PosicaoPersonagemX(Yoshi.PegaPosicaoX());
                    Inimigos[i].PerseguePersonagem();
                }

                // Verifica estado do personagem (bloqueado)
                if (Yoshi.VerificaEstado())
                {
                    // Desconta tempo de vida do Personagem
                    Yoshi.DescontaTempoVida();

                    bool temAcao = false;

                    // Le entrada e executa acao
                    if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                    {
                        if (this.MusicState == EstadoM.Playing)
                        {
                            MediaPlayer.Stop();
                            this.MusicState = EstadoM.Stopped;
                        }
                        else
                        {
                            MediaPlayer.Play(Musica);
                            this.MusicState = EstadoM.Playing;
                        }
                    }
                        
                    /*
                        if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        {
                            Yoshi.Acao(Personagem.Comandos.Pula);
                            temAcao = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                        {
                            Yoshi.Acao(Personagem.Comandos.Comemora);
                            temAcao = true;
                        }
                    */
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        if (Yoshi.PegaPosicaoX() > 
                            (Constante.LimiteEsqPersonagem + 
                            (Yoshi.PegaPosicaoY() - Constante.LimiteSupPersonagem) * 0.4f))

                        {
                            Yoshi.Acao(Personagem.Comandos.AndaTras);
                            Background.RollLeft(deslocamentoX);
                            Toad.RollLeft(deslocamentoX);
                            Clock.RollLeft(deslocamentoX);
                            for (int i = 0; i < qtde_inimigos; i++)
                                Inimigos[i].RollLeft(deslocamentoX);
                            temAcao = true;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        Yoshi.Acao(Personagem.Comandos.AndaFrente);
                        Background.RollRight(deslocamentoX);
                        Toad.RollRight(deslocamentoX);
                        Clock.RollRight(deslocamentoX);
                        for (int i = 0; i < qtde_inimigos; i++)
                            Inimigos[i].RollRight(deslocamentoX);
                        temAcao = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        if (Yoshi.PegaPosicaoY() > Constante.LimiteSupPersonagem)
                        {
                            Yoshi.Acao(Personagem.Comandos.Sobe);
                            Background.RollUp(deslocamentoY);
                            Toad.RollUp(deslocamentoY);
                            Clock.RollUp(deslocamentoY);
                            for (int i = 0; i < qtde_inimigos; i++)
                                Inimigos[i].RollUp(deslocamentoY);
                        }
                        temAcao = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        if (Yoshi.PegaPosicaoY() < Constante.LimiteInfPersonagem)
                        {
                            Yoshi.Acao(Personagem.Comandos.Desce);
                            Background.RollDown(deslocamentoY);
                            Toad.RollDown(deslocamentoY);
                            Clock.RollDown(deslocamentoY);
                            for (int i = 0; i < qtde_inimigos; i++)
                                Inimigos[i].RollDown(deslocamentoY);
                        }
                        temAcao = true;
                    }
                    if (!(temAcao))
                    {
                        Yoshi.SemAcao();
                    }
                }
                
                // Teste de Colisão
                for (int i = 0; i < qtde_inimigos; i++)
                {
                    if (Yoshi.BoundingBox.Intersects(Inimigos[i].BoundingBox))
                    {
                        Yoshi.SofrerDano(1);
                    }
                }
                if (Yoshi.BoundingBox.Intersects(Toad.BoundingBox))
                {
                    Yoshi.GanhaVida(40);
                    Random newY = new Random();
                    Toad.NovaPosicao(+200, (int)AleatorioY(newY));
                }
                if (Yoshi.BoundingBox.Intersects(Clock.BoundingBox))
                {
                    for (int i = 0; i < qtde_inimigos; i++)
                        Inimigos[i].MudarVelocidade(2);
                    Random newY = new Random();
                    Clock.Esconder();
                }

                //HP.SetaPosicao(new Vector2(Yoshi.PegaPosicaoX(), Yoshi.PegaPosicaoY()));
            }

            // Atualiza personagem
            Yoshi.Update(gameTime);

            // Atualiza inimigos
            for (int i = 0; i < qtde_inimigos; i++)
            {
                Inimigos[i].Update(gameTime);
            }

            // Atualiza itens
            Toad.Update(gameTime);
            Clock.Update(gameTime);

            // Background autoscroll
            Background.Update(gameTime);

            // HP
            HP.AtualizaVida(Yoshi.Vida);
            HP.Update(gameTime);

            // Verifica fim da fase
            if ( (Yoshi.PegaPosicaoX() > 
                        (Constante.LimiteDirPersonagem + 
                        (Yoshi.PegaPosicaoY() - Constante.LimiteSupPersonagem) * 0.6f))
                || !Yoshi.TemVida())
            {
                this.GameState = Estado.Fim;
            }

            base.Update(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenho dos elementos do Jogo
         * --------------------------------------------------------------- */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (this.GameState == Estado.Menu) 
                TelaMenu.Draw(gameTime);
            
            else
            {
                Background.Draw(gameTime);
                Texto.Draw(gameTime);
                HP.Draw(gameTime);
                Yoshi.Draw(gameTime);
                Toad.Draw(gameTime);
                Clock.Draw(gameTime);
                for (int i = 0; i < qtde_inimigos; i++)
                    Inimigos[i].Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        /* ---------------------------------------------------------------
         * Desenho dos elementos do Jogo
         * --------------------------------------------------------------- */
        private float AleatorioX(Random r, int i)
        {
            int next;
            int ini = (int)(this.tela_largura / 2.3);
            int part = (int)((Constante.LimiteCampoFim - ini) / 4);
            switch (i%4) {
                case 0:
                    next = r.Next(
                        ini + (part * 0), 
                        ini + (part * 1));
                    break;
                case 1:
                    next = r.Next(
                        ini + (part * 1), 
                        ini + (part * 2));
                    break;
                case 2:
                    next = r.Next(
                        ini + (part * 2), 
                        ini + (part * 3));
                    break;
                case 3:
                    next = r.Next(
                        ini + (part * 3), 
                        Constante.LimiteCampoFim);
                    break;
                default:
                    next = r.Next(
                        ini, 
                        Constante.LimiteCampoFim);
                    break;
            }
            return next;
        }

        private float AleatorioY(Random r, int i)
        {
            int next;
            int part = (int)((Constante.LimiteCampoInf - Constante.LimiteCampoSup) / 3);
            switch (i%3) {
                case 0:
                    next = r.Next(
                        Constante.LimiteCampoSup + (part * 0), 
                        Constante.LimiteCampoSup + (part * 1));
                    break;
                case 1:
                    next = r.Next(
                        Constante.LimiteCampoSup + (part * 1), 
                        Constante.LimiteCampoSup + (part * 2));
                    break;
                case 2:
                    next = r.Next(
                        Constante.LimiteCampoSup + (part * 2), 
                        Constante.LimiteCampoInf);
                    break;
                default:
                    next = r.Next(
                        Constante.LimiteCampoSup, 
                        Constante.LimiteCampoInf);
                    break;
            }
            return next;
        }
        private float AleatorioY(Random r)
        {
            return r.Next((int)(Constante.LimiteCampoSup * 1.5), Constante.LimiteCampoInf);
        }

        private void TrataTeclaTab()
        {

        }

        private void TrataTeclaSpace(){

        }

        private void TrataTeclaLeftControl(){

        }

        private void TrataTeclaLeft(){

        }

        private void TrataTeclaRight(){

        }

        private void TrataTeclaUp(){

        }

        private void TrataTeclaDown()
        {

        }

    }
}
