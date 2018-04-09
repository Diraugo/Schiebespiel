using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

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

        private Vector2 vPosition;

        private bool zeichneBoden;

        Texture2D Textur;


        public Spielobjekte(int pX,int pY, Texture2D pTextur,bool pZeichne)
        {
            this.x = pX;
            this.y = pY;
            Textur = pTextur;
            this.zeichneBoden = pZeichne;
            this.vPosition = new Vector2(pX * 60, pY * 60);
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
        public void setVPosition(Vector2 pV)
        {
            this.vPosition = pV;
        }
        public Vector2 getVPosition()
        {
            return this.vPosition;
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
        Texture2D TexturZiel;

        Spielobjekte Spieler;
        Spielobjekte Box;
        Spielobjekte Ziel;
        List<Spielobjekte> lSpielobjekte=new List<Spielobjekte>();

        SpriteFont Anzeige;

        String Spielfeld;
        string[] SpielArray;

        bool bMovement;
        bool bMoveBox;
        bool bEnd;

        Keys kMove;
        
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

            Spielfeld = "wwwwwwwwww~";
            StreamReader sr = File.OpenText("test.txt");
            string s = "";
            while((s=sr.ReadLine())!=null)
            {
                Spielfeld = Spielfeld + "w" + s + "w~";
            }
            Spielfeld+= "wwwwwwwwww";
            SpielArray =Spielfeld.Split('~');
            bMovement = false;
            bMoveBox = false;
            bEnd = false;

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
            TexturZiel = Content.Load<Texture2D>("ziel");

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
                            Spieler = new Spielobjekte(j,i, TexturSpieler,true);
                            tmpObjekt = new Spielobjekte(j, i, TexturBoden, false);
                            break;
                        case 'x':
                            Box = new Spielobjekte(j, i, TexturBox,true);
                            tmpObjekt = new Spielobjekte(j, i, TexturBoden, false);
                            break;
                        case 'z':
                            Ziel = new Spielobjekte(j, i, TexturZiel, true);
                            tmpObjekt = new Spielobjekte(j, i, TexturBoden, false);
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
            if (!bEnd)
            {
                if (!bMovement)
                {
                    var kstate = Keyboard.GetState();
                    Keys kButton = new Keys();
                    if (kstate.IsKeyDown(Keys.Up))
                    {
                        kButton = Keys.Up;
                    }
                    if (kstate.IsKeyDown(Keys.Down))
                    {
                        kButton = Keys.Down;
                    }
                    if (kstate.IsKeyDown(Keys.Left))
                    {
                        kButton = Keys.Left;
                    }
                    if (kstate.IsKeyDown(Keys.Right))
                    {
                        kButton = Keys.Right;
                    }
                    switch(kButton)
                    {
                        case Keys.Up:
                            if (this.CanMove(Keys.Up))
                                {
                                    if (Spieler.getX() == Box.getX() && Spieler.getY() - 1 == Box.getY())
                                    {
                                        if (CanBoxMove(Keys.Up))
                                        {
                                            Spieler.setPosition(Spieler.getX(), Spieler.getY() - 1);
                                            kMove = Keys.Up;
                                            bMovement = true;
                                        }
                                    }
                                    else
                                    {
                                        Spieler.setPosition(Spieler.getX(), Spieler.getY() - 1);
                                        kMove = Keys.Up;
                                        bMovement = true;
                                    }
                                }
                            break;
                        case Keys.Down:
                            if (this.CanMove(Keys.Down))
                            {
                                if (Spieler.getX() == Box.getX() && Spieler.getY() + 1 == Box.getY())
                                {
                                    if (CanBoxMove(Keys.Down))
                                    {
                                        Spieler.setPosition(Spieler.getX(), Spieler.getY() + 1);
                                        kMove = Keys.Down;
                                        bMovement = true;
                                    }
                                }
                                else
                                {
                                    Spieler.setPosition(Spieler.getX(), Spieler.getY() + 1);
                                    kMove = Keys.Down;
                                    bMovement = true;
                                }
                            }
                            break;
                        case Keys.Left:
                            if (this.CanMove(Keys.Left))
                            {
                                if (Spieler.getX() - 1 == Box.getX() && Spieler.getY() == Box.getY())
                                {
                                    if (CanBoxMove(Keys.Left))
                                    {
                                        Spieler.setPosition(Spieler.getX() - 1, Spieler.getY());
                                        kMove = Keys.Left;
                                        bMovement = true;
                                    }
                                }
                                else
                                {
                                    Spieler.setPosition(Spieler.getX() - 1, Spieler.getY());
                                    kMove = Keys.Left;
                                    bMovement = true;
                                }
                            }
                            break;
                        case Keys.Right:
                        if (this.CanMove(Keys.Right))
                        {
                            if (Spieler.getX() + 1 == Box.getX() && Spieler.getY() == Box.getY())
                            {
                                if (CanBoxMove(Keys.Right))
                                {
                                    Spieler.setPosition(Spieler.getX() + 1, Spieler.getY());
                                    kMove = Keys.Right;
                                    bMovement = true;
                                }
                            }
                            else
                            {
                                Spieler.setPosition(Spieler.getX() + 1, Spieler.getY());
                                kMove = Keys.Right;
                                bMovement = true;
                            }
                        }
                            break;
                        default:
                            break;
                    }
                    if (Spieler.getX() == Box.getX() && Spieler.getY() == Box.getY() && bMovement)
                    {
                        bMoveBox = true;
                        switch (kMove)
                        {
                            case Keys.Up:
                                Box.setPosition(Box.getX(), Box.getY() - 1);
                                break;
                            case Keys.Down:
                                Box.setPosition(Box.getX(), Box.getY() + 1);
                                break;
                            case Keys.Right:
                                Box.setPosition(Box.getX() + 1, Box.getY());
                                break;
                            case Keys.Left:
                                Box.setPosition(Box.getX() - 1, Box.getY());
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Vector2 tmpVektor = Spieler.getVPosition();
                    Vector2 tmpBox = Box.getVPosition();
                    switch (kMove)
                    {
                        case Keys.Up:
                            tmpVektor.Y = tmpVektor.Y - 2;
                            if (bMoveBox)
                            {
                                tmpBox.Y = tmpBox.Y - 2;
                            }
                            if (tmpVektor.Y % 60 == 0 || tmpVektor.Y == 0)
                            {
                                bMovement = false;
                                bMoveBox = false;
                            }
                            break;
                        case Keys.Down:
                            tmpVektor.Y = tmpVektor.Y + 2;
                            if (bMoveBox)
                            {
                                tmpBox.Y = tmpBox.Y + 2;
                            }
                            if (tmpVektor.Y % 60 == 0 || tmpVektor.Y == 0)
                            {
                                bMovement = false;
                                bMoveBox = false;
                            }
                            break;
                        case Keys.Right:
                            tmpVektor.X = tmpVektor.X + 2;
                            if (bMoveBox)
                            {
                                tmpBox.X = tmpBox.X + 2;
                            }
                            if (tmpVektor.X % 60 == 0 || tmpVektor.X == 0)
                            {
                                bMovement = false;
                                bMoveBox = false;
                            }
                            break;
                        case Keys.Left:
                            tmpVektor.X = tmpVektor.X - 2;
                            if (bMoveBox)
                            {
                                tmpBox.X = tmpBox.X - 2;
                            }
                            if (tmpVektor.X % 60 == 0 || tmpVektor.X == 0)
                            {
                                bMovement = false;
                                bMoveBox = false;
                            }
                            break;
                        default:
                            break;
                    }
                    Spieler.setVPosition(tmpVektor);
                    Box.setVPosition(tmpBox);
                }
            }
            if(Box.getVPosition()==Ziel.getVPosition())
            {
                bEnd = true;
            }
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
                    spriteBatch.Draw(TexturBoden, new Rectangle(lSpielobjekte[i].getX() * 60, lSpielobjekte[i].getY() * 60, 60, 60), Color.White);
                }
                spriteBatch.Draw(lSpielobjekte[i].getTextur(),new Rectangle(lSpielobjekte[i].getX()*60,lSpielobjekte[i].getY()*60,60,60),Color.White);
            }
            spriteBatch.Draw(Ziel.getTextur(), new Rectangle(Ziel.getX() * 60, Ziel.getY() * 60, 60, 60), Color.White);
            spriteBatch.Draw(Spieler.getTextur(), new Rectangle((int)Spieler.getVPosition().X, (int)Spieler.getVPosition().Y, 60, 60), Color.White);
            spriteBatch.Draw(Box.getTextur(), new Rectangle((int)Box.getVPosition().X, (int)Box.getVPosition().Y, 60, 60), Color.White);
            if (bEnd)
            {
                spriteBatch.DrawString(Anzeige, "Ziel erreicht", new Vector2(50, 50), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected bool CanMove(Keys pKey)
        {
            bool bMove=false;
            for(int i=0;i<lSpielobjekte.Count;i++)
            {
                if(lSpielobjekte[i].getX()==Spieler.getX()-1&&lSpielobjekte[i].getTextur().Name=="boden"&&pKey==Keys.Left&& lSpielobjekte[i].getY() == Spieler.getY())
                {
                    bMove=true;
                }
                if (lSpielobjekte[i].getX() == Spieler.getX() + 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Right&& lSpielobjekte[i].getY() == Spieler.getY())
                {
                    bMove = true;
                }
                if (lSpielobjekte[i].getY() == Spieler.getY() - 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Up&& lSpielobjekte[i].getX() == Spieler.getX())
                {
                    bMove = true;
                }
                if (lSpielobjekte[i].getY() == Spieler.getY() + 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Down&& lSpielobjekte[i].getX() == Spieler.getX())
                {
                    bMove = true;
                }
            }
            return bMove;
        }

        protected bool CanBoxMove(Keys pKey)
        {
            bool bMove = false;
            for (int i = 0; i < lSpielobjekte.Count; i++)
            {
                if (lSpielobjekte[i].getX() == Box.getX() - 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Left && lSpielobjekte[i].getY() == Box.getY())
                {
                    bMove = true;
                }
                if (lSpielobjekte[i].getX() == Box.getX() + 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Right && lSpielobjekte[i].getY() == Box.getY())
                {
                    bMove = true;
                }
                if (lSpielobjekte[i].getY() == Box.getY() - 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Up && lSpielobjekte[i].getX() == Box.getX())
                {
                    bMove = true;
                }
                if (lSpielobjekte[i].getY() == Box.getY() + 1 && lSpielobjekte[i].getTextur().Name == "boden" && pKey == Keys.Down && lSpielobjekte[i].getX() == Box.getX())
                {
                    bMove = true;
                }
            }
            return bMove;
        }

    }
}
