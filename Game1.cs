using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace schiebespiel
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Spielobjekte
    {
        private int x;
        private int y;

        private bool zeichneBoden;

        Texture2D Textur;


        public Spielobjekte(int pX,int pY, Texture2D pTextur,bool pZeichne)
        {
            this.x = pX;
            this.y = pY;
            Textur = pTextur;
            this.zeichneBoden = pZeichne;
        }

        public int getX()
        {
            return this.x;
        }
        public int getY()
        {
            return this.y;
        }
        public bool getZeichneBoden()
        {
            return this.zeichneBoden;
        }
        public Texture2D getTextur()
        {
            return this.Textur;
        }
        public void setPosition(int pX,int pY)
        {
            this.x = pX;
            this.y = pY;
        }
    }



    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D TexturTisch;
        Texture2D TexturWand;
        Texture2D TexturBox;
        Texture2D TexturSpieler;
        Texture2D TexturBoden;

        List<Spielobjekte> lSpielobjekte=new List<Spielobjekte>();

        SpriteFont Anzeige;

        String Spielfeld;
        string[] SpielArray;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Spielfeld = "wwwwwww\n" +
                "w**p**w\n" +
                "w**t**w\n" +
                "w**x**w\n" +
                "wwwwwww";
            SpielArray=Spielfeld.Split('\n');
            
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Anzeige = Content.Load<SpriteFont>("Text");
            TexturBox = Content.Load<Texture2D>("box");
            TexturBoden = Content.Load<Texture2D>("boden");
            TexturWand = Content.Load<Texture2D>("wand");
            TexturSpieler = Content.Load<Texture2D>("spieler");
            TexturTisch = Content.Load<Texture2D>("tisch");

            Spielobjekte tmpObjekt;
            for (int i = 0; i < SpielArray.Length; i++)
            {
                for (int j = 0; j < SpielArray[i].Length; j++)
                {
                    switch (SpielArray[i].ToCharArray()[j])
                    {
                        case 'w':
                            tmpObjekt = new Spielobjekte(j, i, TexturWand,false);
                            break;
                        case '*':
                            tmpObjekt = new Spielobjekte(j, i, TexturBoden,false);
                            break;
                        case 'p':
                            tmpObjekt = new Spielobjekte(j,i, TexturSpieler,true);
                            break;
                        case 'x':
                            tmpObjekt = new Spielobjekte(j, i, TexturBox,true);
                            break;
                        case 't':
                            tmpObjekt = new Spielobjekte(j, i, TexturTisch, true);
                            break;
                        default:
                            tmpObjekt = new Spielobjekte(j, i, TexturBoden,false);
                            break;
                    }

                    lSpielobjekte.Add(tmpObjekt);
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for(int i=0;i<lSpielobjekte.Count;i++)
            {
                if(lSpielobjekte[i].getZeichneBoden())
                {
                    spriteBatch.Draw(TexturBoden, new Rectangle(lSpielobjekte[i].getX() * 80, lSpielobjekte[i].getY() * 80, 80, 80), Color.White);
                }
                spriteBatch.Draw(lSpielobjekte[i].getTextur(),new Rectangle(lSpielobjekte[i].getX()*80,lSpielobjekte[i].getY()*80,80,80),Color.White);
            }
            //spriteBatch.DrawString(Anzeige, Spielfeld, new Vector2(50, 50), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
