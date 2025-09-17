using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MeuJogo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D walkSheet;
        private Texture2D idleSheet;

        private Vector2 position = new Vector2(200, 200);
        private float speed = 100f;

        private int frame = 0;
        private double timer = 0;
        private double interval = 0.15; // velocidade da animação

        private bool isWalking = false;

        private Rectangle sourceRect;
        private SpriteEffects spriteEffect = SpriteEffects.None;

        private enum Direction { Down, Up, Right, Left }
        private Direction direction = Direction.Down;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            walkSheet = Content.Load<Texture2D>("Walk");
            idleSheet = Content.Load<Texture2D>("Idle");
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            isWalking = false;

            if (kstate.IsKeyDown(Keys.Escape))
                Exit();

            Vector2 move = Vector2.Zero;

            if (kstate.IsKeyDown(Keys.Up))
            {
                move.Y -= 1;
                isWalking = true;
                direction = Direction.Up;
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                move.Y += 1;
                isWalking = true;
                direction = Direction.Down;
            }
            if (kstate.IsKeyDown(Keys.Left))
            {
                move.X -= 1;
                isWalking = true;
                direction = Direction.Left;
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
                move.X += 1;
                isWalking = true;
                direction = Direction.Right;
            }

            if (move != Vector2.Zero)
            {
                move.Normalize();
                position += move * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // animação (tanto andando quanto idle)
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer > interval)
            {
                frame++;
                timer = 0;
            }

            // Determinar a linha e efeito
            int row = 0;
            spriteEffect = SpriteEffects.None;

            switch (direction)
            {
                case Direction.Down: row = 0; break;
                case Direction.Up: row = 1; break;
                case Direction.Right: row = 2; break;
                case Direction.Left:
                    row = 2; // mesma linha da direita
                    spriteEffect = SpriteEffects.FlipHorizontally; // espelha
                    break;
            }

            // Sempre 4 frames por linha
            if (frame > 3) frame = 0;

            sourceRect = new Rectangle(frame * 32, row * 32, 32, 32);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // escolher spritesheet baseado em movimento
            Texture2D currentSheet = isWalking ? walkSheet : idleSheet;

            _spriteBatch.Draw(currentSheet, position, sourceRect, Color.White, 0f, Vector2.Zero, 2f, spriteEffect, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}