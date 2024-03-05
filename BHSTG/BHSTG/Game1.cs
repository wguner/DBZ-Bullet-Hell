using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BHSTG.Constants;
using BHSTG.Enemies;
using BHSTG.Player;
using BHSTG.Controller;
using System;
using Microsoft.Xna.Framework.Media;
using BHSTG.Backgrounds;
using System.IO;
using Newtonsoft.Json;

namespace BHSTG
{
    public class Game1 : Game
    {
        Song song;

        //Background
        Scrolling scroll;
        Scrolling scroll2;
        myCamera camera;

        // All textures
        Texture2D projectileTexture;
        Texture2D projTexture2;
        Texture2D basicEnemyTexture;
        Texture2D basicEnemyTexture2;
        Texture2D midBossTexture;
        Texture2D playerTexture;
        Texture2D finalBossTexture;
        Texture2D WinTexture, LoseTexture;
        Texture2D playerInvincible;
        Texture2D easyModeUI;
        Texture2D hardModeUI;
        Texture2D abilityUI;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        ProjectileController projectileController;
        List<IEnemyFactory> waves;
        // Master list of all enemies.
        List<IEnemy> enemies;
        PlayerClass player;
        //lives and win/lose controller
        LivesController LController;
        //number of lives the player has
        int lives = 3000;
        double activeTime = 0.0;
        bool abilityUsed = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            GameSettings settings = ParseJSON();
            SetKeyboardControls(settings);
            // Create factories and lists
            projectileController = new ProjectileController(ref projectileTexture, ref projTexture2, new Vector2(settings.player.startPosition.x, settings.player.startPosition.y));
            player = new PlayerClass(
                new Vector2(settings.player.startPosition.x, settings.player.startPosition.y),
                ref GetTexture(settings.player.texture),
                settings.player.health,
                (float)settings.player.movement.speed,
                settings.player.attackSpeed,
                ref projectileController,
                GetMovement(
                    settings.player.movement.style,
                    0,
                    settings.player.movement.speed,
                    new Vector2(settings.player.startPosition.x, settings.player.startPosition.y),
                    new Vector2(0,0)),
                GetAttack(settings.player.attackStyle),
                LController);
            lives = settings.player.lives;
            LController = new LivesController(
                ref player,
                lives,
                270,
                ref WinTexture,
                ref LoseTexture);
            player.setLivesController(LController);
            waves = new List<IEnemyFactory>();
            enemies = new List<IEnemy>();
            CreateWaves(settings);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            camera = new myCamera(GraphicsDevice.Viewport);

            projectileTexture = Content.Load<Texture2D>("ball");
            projTexture2 = Content.Load<Texture2D>("b_flame");
            basicEnemyTexture = Content.Load<Texture2D>("Saibamen");
            basicEnemyTexture2 = Content.Load<Texture2D>("SaibamenBlue");
            midBossTexture = Content.Load<Texture2D>("frieza");
            playerTexture = Content.Load<Texture2D>("goku back sprite");
            finalBossTexture = Content.Load<Texture2D>("vegetaX1");
            WinTexture = Content.Load<Texture2D>("You're Winner");
            LoseTexture = Content.Load<Texture2D>("You're Loser");
            playerInvincible = Content.Load<Texture2D>("invincegoku");
            easyModeUI = Content.Load<Texture2D>("easymode");
            hardModeUI = Content.Load<Texture2D>("hardmode");
            abilityUI = Content.Load<Texture2D>("ability");

            // song
            song = Content.Load<Song>("Song");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            // background
            scroll = new Scrolling(Content.Load<Texture2D>("mines_BG"), new Rectangle(0, 0, 800, 500));
            scroll2 = new Scrolling(Content.Load<Texture2D>("mines_BG"), new Rectangle(0, -500, 800, 500));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var ZoomIn = Keyboard.GetState().IsKeyDown(Keys.T);
            var ZoomOut = Keyboard.GetState().IsKeyDown(Keys.G);
            var RotateRight = Keyboard.GetState().IsKeyDown(Keys.U);
            var RotateLeft = Keyboard.GetState().IsKeyDown(Keys.J);


            if (ZoomIn)
                camera.ZoomValue += 0.01f;
            else if (ZoomOut)
                camera.ZoomValue -= 0.01f;
            if (RotateRight)
                camera.RotateValue += 0.01f;
            else if (RotateLeft)
                camera.RotateValue -= 0.01f;

            //camera.Update(player.position); // camera follows the player's position
            camera.Update();

            LoadEnemies(gameTime);
            projectileController.Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime, player.position);
            UpdateEnemies((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime);

            if (LController.getLives() > 0)
            {
                UpdatePlayer((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime);
            }
            // Don't know what this does. Uncomment if things are breaking.
            //base.Update(gameTime);
            UpdateLivesController((float)gameTime.TotalGameTime.TotalSeconds);

            // background
            if (scroll.rectangle.Y > Numbers.WINDOW_HEIGHT)
            {
                scroll.rectangle.Y = scroll2.rectangle.Y - (scroll.texture.Height-140);
            }
            if (scroll2.rectangle.Y > Numbers.WINDOW_HEIGHT)
            {
                scroll2.rectangle.Y = scroll.rectangle.Y - (scroll2.texture.Height-140);
            }
            scroll.Update();
            scroll2.Update();
        }

        public void LoadEnemies(GameTime gameTime)
        {
            if (waves.Count > 0)
            {
                waves[0].Update((float)gameTime.ElapsedGameTime.TotalSeconds, gameTime.TotalGameTime);
                if (waves[0].IsFinished(gameTime.TotalGameTime))
                {
                    waves.RemoveAt(0);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //_spriteBatch.Begin();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Flip);

            // background
            scroll.Draw(_spriteBatch);
            scroll2.Draw(_spriteBatch);
            
            if(player.hard == false)
            {
                _spriteBatch.Draw(easyModeUI, new Vector2(10,400), Color.White);
            }
            if (player.hard == true)
            {
                _spriteBatch.Draw(hardModeUI, new Vector2(10, 400), Color.White);
            }
            if (abilityUsed == false)
            {
                _spriteBatch.Draw(abilityUI, new Vector2(90, 400), Color.White);
            }



            // gaming
            foreach (var enemy in enemies)
            {
                _spriteBatch.Draw(enemy.texture, enemy.position, Color.White);
            }

            foreach (var projectile in projectileController.enemyProjectiles)
            {
                _spriteBatch.Draw(projectile.texture, projectile.position, Color.White);
            }

            foreach (var projectile in projectileController.playerProjectiles)
            {
                _spriteBatch.Draw(projectile.texture, projectile.position, Color.White);
            }

            if (!player.isDead())
            {
                _spriteBatch.Draw(player.texture, player.position, Color.White);
            }
            
            if (player.invincible == true)
            {
                abilityUsed = true;
                activeTime += 0.1;
                player.texture = playerInvincible;
                player.abilityTimer(activeTime);
                _spriteBatch.Draw(player.texture, player.position, Color.White);
            }
            
            else if (player.invincible == false)
            {
                activeTime = 0;
                player.texture = playerTexture;
                _spriteBatch.Draw(player.texture, player.position, Color.White);
            }

            if (LController.win)
            {
                _spriteBatch.Draw(LController.WinImage, new Vector2(270, 100), Color.White);
                EndGame();
                player.invincible = true;
            }else if (LController.lose)
            {
                _spriteBatch.Draw(LController.LoseImage, new Vector2(270, 100), Color.White);
                EndGame();
            }

            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        private void UpdateEnemies(float elapsedTime, TimeSpan currentTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (IsOutOfBounds(enemies[i].position, enemies[i].texture.Width, enemies[i].texture.Height))
                {
                    enemies.RemoveAt(i);
                }
                else
                {
                    if (player.hard == true)
                    {
                        enemies[i].hard = true;
                    }
                    if (player.hard == false)
                    {
                        enemies[i].hard = false;
                    }
                    enemies[i].Update(elapsedTime, currentTime);
                }
            }
            for (int i = 0; i < projectileController.playerProjectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    try
                    {
                        if (projectileController.playerProjectiles[i].Rectangle.Intersects(enemies[j].Rectangle))
                        {
                            projectileController.playerProjectiles.RemoveAt(i);
                            enemies[j].TakeDamage(1);
                            if (enemies[j].isDead())
                            {
                                enemies.RemoveAt(j);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // List ran out of enemies
                    }
                }
            }
        }

        private void UpdatePlayer(float elapsedTime, TimeSpan currentTime)
        {
            player.Update(elapsedTime, currentTime);

            for (int i = 0; i < projectileController.enemyProjectiles.Count; i++)
            {
                if (player.Rectangle.Intersects(projectileController.enemyProjectiles[i].Rectangle))
                { 
                    this.projectileController.enemyProjectiles.RemoveAt(i);
                    player.TakeDamage(1);
                }
            }
        }

        private void UpdateLivesController(float time)
        {
            LController.Update(time, waves.Count == 0 && enemies.Count == 0);
        }

        private bool IsOutOfBounds(Vector2 position, double width, double height)
        {
            Vector2 midpoint = new Vector2((float)width / 2, (float)height / 2);
            midpoint += position;
            return midpoint.X < 0 || midpoint.X > Numbers.WINDOW_WIDTH || midpoint.Y < 0 || midpoint.Y > Numbers.WINDOW_HEIGHT;
        }

        private GameSettings ParseJSON()
        {
            string jsonText = File.ReadAllText(@"Content\settings.json");
            var newGame = JsonConvert.DeserializeObject<Root>(jsonText);
            return newGame.game;
        }

        private void CreateWaves(GameSettings settings)
        {
            foreach (var wave in settings.waves)
            {
                CreateWave(wave);
            }
        }

        private void SetKeyboardControls(GameSettings settings)
        {
            var controls = settings.KeyboardControls;
            bool success;
            success = Enum.TryParse(typeof(Keys), controls.UP, out var upKey);
            if (success)
            {
                Numbers.up = (Keys)upKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.DOWN, out var downKey);
            if (success)
            {
                Numbers.down = (Keys)downKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.RIGHT, out var rightKey);
            if (success)
            {
                Numbers.right = (Keys)rightKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.LEFT, out var leftKey);
            if (success)
            {
                Numbers.left = (Keys)leftKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.FIRE, out var fireKey);
            if (success)
            {
                Numbers.fire = (Keys)fireKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.SLOW, out var slowKey);
            if (success)
            {
                Numbers.slow = (Keys)slowKey;
            }
            success = Enum.TryParse(typeof(Keys), controls.INVINCIBLE, out var invincibleKey);
            if (success)
            {
                Numbers.invincible = (Keys)invincibleKey;
            }
        }

        private void CreateWave(Wave waveSettings)
        {
            // enemy type needs to be parsed
            // texture needs to be pased

            AttackWrapper attack = GetAttack(waveSettings.attackStyle);

            MovementWrapper movement = GetMovement(
                waveSettings.movement.style,
                waveSettings.movement.minCompletionTime,
                waveSettings.movement.speed,
                new Vector2(waveSettings.startPosition.x, waveSettings.startPosition.y),
                new Vector2(waveSettings.target.x, waveSettings.target.y));

            switch (waveSettings.enemyType)
            {
                case "basicEnemy":
                    waves.Add(
                        new BasicEnemyFactory(
                            ref GetTexture(waveSettings.texture),
                            ref enemies,
                            ref projectileController,
                            new Vector2(waveSettings.startPosition.x, waveSettings.startPosition.y),
                            waveSettings.hp,
                            waveSettings.atkSpeed,
                            waveSettings.enemyAmount,
                            waveSettings.interval,
                            waveSettings.duration,
                            movement,
                            attack));
                    break;
                case "basicEnemy2":
                    waves.Add(
                        new AvgBearFactory(
                            ref GetTexture(waveSettings.texture),
                            ref enemies,
                            ref projectileController,
                            new Vector2(waveSettings.startPosition.x, waveSettings.startPosition.y),
                            waveSettings.hp,
                            waveSettings.atkSpeed,
                            waveSettings.enemyAmount,
                            waveSettings.interval,
                            waveSettings.duration,
                            movement,
                            attack));
                    break;
                case "midBoss":
                    waves.Add(
                        new MidBossFactory(
                            ref GetTexture(waveSettings.texture),
                            ref enemies,
                            ref projectileController,
                            new Vector2(waveSettings.startPosition.x, waveSettings.startPosition.y),
                            waveSettings.hp,
                            waveSettings.atkSpeed,
                            waveSettings.enemyAmount,
                            waveSettings.interval,
                            waveSettings.duration,
                            movement,
                            attack));
                    break;
                case "finalBoss":
                    waves.Add(
                        new FinalBossFactory(
                            ref GetTexture(waveSettings.texture),
                            ref enemies,
                            ref projectileController,
                            new Vector2(waveSettings.startPosition.x, waveSettings.startPosition.y),
                            waveSettings.hp,
                            waveSettings.atkSpeed,
                            waveSettings.enemyAmount,
                            waveSettings.interval,
                            waveSettings.duration,
                            movement,
                            attack));
                    break;
                default:
                    break;
            }
        }

        private AttackWrapper GetAttack(string attackStyle)
        {
            AttackWrapper attack = null;
            switch (attackStyle)
            {
                case "straight":
                    attack = new straightShot();
                    break;
                case "tracking":
                    attack = new trackingShot();
                    break;
                case "midBoss":
                    attack = new MidBossAttack();
                    break;
                case "finalBoss":
                    attack = new FinalBossAttack();
                    break;
                case "player":
                    attack = new PlayerAttack();
                    break;
                default:
                    break;
            }
            return attack;
        }

        private MovementWrapper GetMovement(string movementStyle, int minTime, double speed, Vector2 startPosition, Vector2 target)
        {
            MovementWrapper movement = null;
            switch (movementStyle)
            {
                case "linear":
                    movement = new linearMove(target, minTime, speed);
                    break;
                case "zigzag":
                    movement = new zigZagMove(startPosition, minTime, speed);
                    break;
                case "midBoss":
                    movement = new MidBossMovement();
                    break;
                case "finalBoss":
                    movement = new FinalBossMovement();
                    break;
                case "player":
                    movement = new MovePlayer(5);
                    break;
                default:
                    break;
            }
            return movement;
        }

        private ref Texture2D GetTexture(string texture)
        {
            switch (texture)
            {
                case "basicEnemy":
                    return ref basicEnemyTexture;
                case "basicEnemy2":
                    return ref basicEnemyTexture2;
                case "midBoss":
                    return ref midBossTexture;
                case "finalBoss":
                    return ref finalBossTexture;
                case "player":
                    return ref playerTexture;
                default:
                    return ref basicEnemyTexture;
            }
        }

        private void EndGame()
        {
            waves.RemoveRange(0, waves.Count);
            enemies.RemoveRange(0, enemies.Count);
        }
    }
}
