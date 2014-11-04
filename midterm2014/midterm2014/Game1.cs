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
using Sprites;

namespace midterm2014
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimatedSprite _player;
        SimpleSprite _background;
        collectable[] _collectables;
        int _playerScore  = 0;
        private SpriteFont _scoreFont;
        private Vector2 _playerStartPosition;
        private float speed = 5.0f;
        private Song _backingTrack;
        private Song _Success;
        private SoundEffect _collecting;

        System.Timers.Timer t = new System.Timers.Timer();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 728;
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
            // Part 4
            _background = new SimpleSprite(Content.Load<Texture2D>(@"images/background"),Vector2.Zero);
            _scoreFont = Content.Load<SpriteFont>("score");
            // Part 6
            _player = new AnimatedSprite(Content.Load<Texture2D>(@"images/player"),
                                            Vector2.Zero,14);
            // part 5 
            ResetCollectables(Content.Load<Texture2D>(@"images/collectable"));

            // Move the player to the Centre
            _player.Move(new Vector2(GraphicsDevice.Viewport.Width/2 - _player.SpriteWidth/2,
                GraphicsDevice.Viewport.Height/2 - _player.SpriteHeight/2));

            _playerStartPosition = _player.position;
            _backingTrack = Content.Load<Song>(@"sounds/backing track");
            _Success = Content.Load<Song>(@"sounds/success");
            _collecting = Content.Load<SoundEffect>(@"sounds/collecting");

            MediaPlayer.Play(_backingTrack);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // Part 6
            _player.UpdateAnimation(gameTime);
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _player.Move(new Vector2(-1, 0) * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _player.Move(new Vector2(0, -1) * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _player.Move(new Vector2(0, 1) * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _player.Move(new Vector2(1, 0) * speed);
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                    ResetGame();
            
            // Part 8
            foreach (collectable c in _collectables)
                if (c.Visible && _player.BoundingRect.Intersects(c.BoundingRect))
                {
                    _playerScore += c.Value;
                    c.Visible = false;
                    _collecting.Play();
                    _player.position = _playerStartPosition;
                }

            //Part 11
            bool gameOver = true;
            foreach(collectable c in _collectables)
                if (c.Visible == true)
                {
                    gameOver = false;
                    break;
                }
            if (gameOver)
            {
                    MediaPlayer.Play(_Success);
                    while(MediaPlayer.State == MediaState.Playing) 
                        ;
                     ResetGame();
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
            spriteBatch.Begin();
            // Part 4
            _background.draw(spriteBatch);
            // Part 5
            spriteBatch.DrawString(_scoreFont, "Score " + _playerScore.ToString(), new Vector2(20, 20), Color.White);
            foreach (collectable item in _collectables)
                item.draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here
            _player.Draw(spriteBatch, _scoreFont);
            base.Draw(gameTime);
        }

        public Vector2 RandomVector(Rectangle bounds)
        {
            Random r = new Random();
            return new Vector2(r.Next(bounds.Left, bounds.Right), 
                r.Next(bounds.Top, bounds.Bottom));
        }

        public void ResetGame()
        {
            ResetCollectables(Content.Load<Texture2D>(@"images/collectable"));
            _playerScore = 0;
            _player.position = _playerStartPosition;
            MediaPlayer.Play(_backingTrack);
            
        }

        public void ResetCollectables(Texture2D tx)
        {
            // Part 5 and Part 11
            Random r = new Random();
            int noOfCollectables = r.Next(4, 10);
            _collectables = new collectable[noOfCollectables];
            for (int i = 0; i < noOfCollectables; i++)
            {
                _collectables[i] = new collectable(tx, Vector2.Zero);
                // Set the position of the collectables randomly
                _collectables[i].Move(new Vector2(r.Next(_collectables[i].Image.Width, GraphicsDevice.Viewport.Width - _collectables[i].Image.Width),
                                                        r.Next(_collectables[i].Image.Height, GraphicsDevice.Viewport.Height - _collectables[i].Image.Height)));

            }
            
        }
    }
}
