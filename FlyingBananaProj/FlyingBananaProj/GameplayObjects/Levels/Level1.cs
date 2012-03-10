using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace FlyingBananaProj
{
    class Level1 : Level
    {
        protected TimeSpan timeToChargeShot = TimeSpan.FromSeconds(1);

        protected bool clotsDestroyed;
        protected bool lastShot;
        protected bool lastCharge;
        protected ChargeBall cb;
        protected Laser laser;
        protected bool enemiesReenabled;
        protected bool switchedToStart;
        protected bool switchedToBoss;
        protected bool infectorAdded;
        protected bool clotsAdded;
        protected bool switchedToMiniBoss;
        protected bool switchedToClot;
        protected float beginDescentZ;
        protected bool movePreInfector;
        protected float startGameZ;
        protected float clotZ;
        protected bool reachedClotZ;
        protected float infectorZ;
        protected bool reachedInfectorZ;
        protected bool infectorDestroyed;
        protected bool thirdMove;
        protected bool fourthMove;
        protected bool fifthMove;
        protected bool rotatedBehind;
        protected bool bossAdded;
        protected bool firstStart;
        protected bool introRot1;
        protected bool introRot2;
        protected bool introRot3;
        protected bool introRot4;
        protected bool thirdPersonRot;
        protected bool thirdPersonTrans;
        protected bool setRot;
        protected bool setTrans;
        protected bool returnedToTopDown;

        protected bool endingMove1, endingMove2, endingMove3;

        protected Infector miniBoss;

        protected TokenPickup preloadedTokenPickup;
        protected Explosion preloadedExplosion;
        protected Laser preloadedLaser;
        protected ChargeBall preloadedChargeBall;
        protected StraightCell preloadedStraightCell;
        protected LatchingCell preloadedLatchingCell;
        protected ChargingCell preloadedChargingCell;
        protected Missle preloadedMissile;
        protected BotMissile preloadedBotMissile;
        protected MeleeBot preloadedMeleeBot;
        protected MissileBot preloadedMissileBot;
        protected Needle preloadedNeedle;
        protected ClotSide leftClot;
        protected ClotSide rightClot;
        protected double timeToResumeSeconds = 1;
        protected TimeSpan timeToResume;
        public Level1(ContentManager content, SpriteBatch sb)
        {
            levelEnded = false;
            lastShot = false;
            lastCharge = false;
            this.content = content;
            spriteBatch = sb;
            geometry = this.content.Load<Model>(@"levels/level1");
            levelTexture = this.content.Load<Texture2D>(@"textures/levelTexture");
            position = new Vector3(-45, -755, 8200);
            timeToResume = TimeSpan.FromSeconds(timeToResumeSeconds);

            switchedToBoss = false;
            switchedToMiniBoss = false;
            switchedToStart = false;
            switchedToClot = false;

            startGameZ = 7645;
            infectorZ = 2800;
            clotZ = 2200;
            thirdPersonZ = 650;
            preBossZ = 1300;

            reachedClotZ = false;
            infectorAdded = false;
            reachedPreBossZ = false;
            infectorDestroyed = false;
            movePreInfector = false;

            endingMove1 = endingMove2 = endingMove3 = false;

            velocity = Vector3.Zero;
            firstMove = false;
            secondMove = false;
            thirdMove = false;
            fourthMove = false;
            rotatedBehind = false;
            fifthMove = false;
            beginDescentZ = -430;

            reachedBoss = false;
            enemySpawner = new EnemySpawner(1, content);
            bossAdded = false;
            clotsAdded = false;
            firstStart = false;

            introRot1 = false;
            introRot2 = false;
            introRot3 = false;
            introRot4 = false;

            thirdPersonRot = false;
            thirdPersonTrans = false;
            setRot = false;
            setTrans = false;
            returnedToTopDown = false;
            enemiesReenabled = false;

            miniBoss = new Infector(content, new Vector3(0, 0, 250));
            rightClot = new ClotSide(content, new Vector3(-60, 0, 20), Side.Right);
            leftClot = new ClotSide(content, new Vector3(60, 0, 20), Side.Left);

            preloadedStraightCell = new StraightCell(content, new Vector3(0, 500, 0));
            preloadedLatchingCell = new LatchingCell(content, new Vector3(0, 500, 0), Vector3.Zero);
            preloadedChargingCell = new ChargingCell(content, new Vector3(0, 500, 0), Vector3.Zero);
            preloadedMeleeBot = new MeleeBot(content, new Vector3(0, 500, 0), Vector3.Zero);
            preloadedMissileBot = new MissileBot(content, new Vector3(0, 500, 0), Vector3.Zero);
            preloadedTokenPickup = new TokenPickup(content, new Vector3(0, 500, 0));
            preloadedMissile = new Missle(content, Vector3.Zero, Vector3.Zero);
            preloadedBotMissile = new BotMissile(content, Vector3.Zero, Vector3.Zero);
            preloadedNeedle = new Needle(content, Vector3.Zero, Vector3.Zero);
            preloadedLaser = new Laser(content, Vector3.Zero, Vector3.Zero);
            preloadedExplosion = new Explosion(0, 0, 0, Vector3.Zero, content, false);
            preloadedChargeBall = new ChargeBall(content, Vector3.Zero);

            clotsDestroyed = false;

            Camera.Instance.setPosition(new Vector3(-7, 7, 7));

        }

        public void UpdateClot(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            velocity = Vector3.Zero;
            //clot logic in here
            if (!clotsAdded)
            {
                Add(leftClot);
                Add(rightClot);
                clotsAdded = true;
            }
            else
            {
                if (leftClot.Seal || rightClot.Seal)
                {
                    p.setShield(0); //game over
                }
                if (leftClot.getHealth() < rightClot.getHealth())
                {
                    rightClot.setHealth(leftClot.getHealth());
                    if (rightClot.getHealth() <= 0)
                    {
                        rightClot.Die();
                        clotsDestroyed = true;
                        velocity = -Vector3.UnitZ;
                    }
                }
                if (rightClot.getHealth() < leftClot.getHealth())
                {
                    leftClot.setHealth(rightClot.getHealth());
                    if (leftClot.getHealth() <= 0)
                    {
                        leftClot.Die();
                        clotsDestroyed = true;
                        velocity = -Vector3.UnitZ;
                    }
                }
            }
            
        }

        public void UpdatePreMiniBoss(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            p.controls.Locked = true;
            Vector3 newPos = new Vector3(0, 0, -30);
            if (!movePreInfector) // prevent jitter
                movePreInfector = movePlayerToPos(p, newPos);

            velocity = velocity * 0.99f;
            if (velocity.Z > -0.01f) //slow the level down to a halt
            {
                velocity = Vector3.Zero;
            }

            if (velocity == Vector3.Zero)
            {
                if (!enemiesReenabled)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        if (entities[i] is MeleeBot || entities[i] is MissileBot)
                        {
                            EnemiesAndPlayer ep = entities[i] as EnemiesAndPlayer;
                            ep.Enabled = true;
                        }
                    }
                    enemiesReenabled = true;
                }
                //have the infector come in
                if (!infectorAdded)
                {
                    Add(miniBoss);
                    infectorAdded = true;
                }
                else
                {
                    if (miniBoss.Activated)
                    {
                        p.controls.Locked = false;
                    }
                }
                if (infectorAdded && miniBoss.isDead)
                {
                    infectorDestroyed = true;
                    velocity = -Vector3.UnitZ;
                }
            }
        }

        public void UpdatePreBoss(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            if (!reachedBoss)
            {
                switchedToBoss = false;
                destroyAllEnemies();
                //MediaPlayer.Stop(); //should occur slowly


                if (switchedToBoss && !rotatedBehind)
                {
                    velocity = -Vector3.UnitZ * 8;
                }

                p.controls.Locked = true;

                Vector3 newPos = new Vector3(0, 0, 0);
                if (!firstMove) // prevent jitter
                    firstMove = movePlayerToPos(p, newPos);

                if (position.Z > thirdPersonZ)
                {
                    enemySpawner.Update(gameTime, entities[0].Position, getZ(), false, true); //add cells only
                }

                if (position.Z < thirdPersonZ) // time to go third person
                {
                    if (!thirdPersonRot)
                    {
                        thirdPersonRot = Camera.Instance.translateCameraToTarget3(new Vector3(p.Position.X, p.Position.Y + 10, p.Position.Z - 20), 0.25f);
                       // Camera.Instance.setPosition(new Vector3(p.Position.X, p.Position.Y + 10, p.Position.Z - 20));
                    }
                }

                if (position.Z < beginDescentZ) //time to descend into the heart
                {
                    p.changeToBossMode(Vector3.Zero);
                    if (firstMove && !rotatedBehind) //first rotate by 180 degrees
                    {
                        velocity = Vector3.Zero;
                        rotatedBehind = rotatePlayerToRot(p, new Vector3(0, MathHelper.Pi, 0));
                    }
                    if (firstMove && rotatedBehind && !fourthMove) //then go down
                    {
                        newPos = new Vector3(p.Position.X, -300, 0);
                        fourthMove = movePlayerToPos(p, newPos);
                    }
                    if (firstMove && rotatedBehind && fourthMove && !fifthMove) //then go inside
                    {
                        newPos = new Vector3(p.Position.X, p.Position.Y, -100);
                        fifthMove = movePlayerToPos(p, newPos);
                        if (fifthMove)
                        {
                            //p.changeToBossMode(new Vector3(0, -300, -50));
                            
                            //p.controls.Locked = false;
                            reachedBoss = true;
                        }
                    }
                }
            }
        }

        public bool moveBotToPos(EnemiesAndPlayer b, Vector3 newPos)
        {
            Vector3 newVel = newPos - b.Position;
            newVel.Normalize();

            float error = 4;
            if (Vector3.Distance(newPos, b.Position) > error) // prevent jitter
            {
                if (switchedToBoss || switchedToMiniBoss)
                    b.Velocity = newVel * 8;
                else
                    b.Velocity = newVel;
            }
            else
            {
                b.Velocity = Vector3.Zero;
                b.Position = newPos;
                return true;
            }
            return false;
        }

        public bool movePlayerToPos(Player p, Vector3 newPos)
        {
            Vector3 newVel = newPos - p.Position;
            newVel.Normalize();

            float error = 4;
            if (Vector3.Distance(newPos, p.Position) > error) // prevent jitter
            {
                if (switchedToBoss || switchedToMiniBoss)
                    p.Velocity = newVel * 8;
                else
                    p.Velocity = newVel;
            }
            else
            {
                p.Velocity = Vector3.Zero;
                p.Position = newPos;
                return true;
            }
            return false;
        }
        public bool rotatePlayerToRot(Player p, Vector3 newRot)
        {
            Vector3 newVel = newRot - p.Rotation;
            newVel.Normalize();

            float error = 0.01f;
            if (Vector3.Distance(newRot, p.Rotation) > error) // prevent jitter
            {
                if (switchedToBoss)
                    p.rotate(new Vector3(0, MathHelper.ToRadians(1), 0));
                else
                    p.rotate(new Vector3(0, MathHelper.ToRadians(1), 0));
            }
            else
            {
                p.Rotation = newRot;
                return true;
            }
            return false;
        }

        public void UpdateBoss(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            if (!endingMove1)
            {
                if (p.Rotation.Y > MathHelper.ToRadians(140))
                {
                    p.rotate(new Vector3(0, -MathHelper.ToRadians(1), 0));
                }
                else endingMove1 = true;
            }
            if (endingMove1 && !endingMove2)
            {
                endingMove2 = movePlayerToPos(p, new Vector3(p.Position.X, -850, p.Position.Z));
            }
            if (endingMove1 && endingMove2 && !lastCharge)
            {
                timeToChargeShot = timeToChargeShot.Subtract(gameTime.ElapsedGameTime);
                if (timeToChargeShot.TotalSeconds <= 0)
                {
                    cb = new ChargeBall(content, new Vector3(p.Position.X + 5, p.Position.Y, p.Position.Z - 5));
                    Add(cb);
                    lastCharge = true;
                }
            }
            if (endingMove1 && endingMove2 && lastCharge && !lastShot)
            {
                if (cb.isDead)
                {
                    Vector3 dir = new Vector3(285, p.Position.Y, -400) - p.Position;
                    dir.Normalize();
                    dir /= 2;
                    laser = new Laser(content, new Vector3(p.position.X, p.position.Y, p.position.Z), dir);
                    Add(laser);
                    p.CameraEnabled = false;
                    Camera.Instance.rotateCameraPos(new Vector3(MathHelper.ToRadians(60), 0, 0));
                    lastShot = true;
                }
            }
            if (lastShot)
            {
                p.CameraEnabled = false;
                 
                Camera.Instance.UpdateCamera(laser.Position, laser.Rotation);

                if (Vector3.Distance(new Vector3(285, p.Position.Y, -400), laser.Position) <  200)
                {
                    levelEnded = true; 
                }
                    
            }
            //p.Position = new Vector3(285, -850, -200);
            CheckCollision();
            levelWorldMat = Matrix.CreateScale(10) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(position);
            base.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            
            #region Intro Handling
            if (!firstStart) // Intro sequence
            {
                if (position.Z <= startGameZ) //unlock the player's controls at a certain point
                {
                    //Camera.Instance.translateCameraToTarget3(Camera.Instance.TopDownPosition, 0.5f);
                    p.controls.Locked = false;
                    firstStart = true;
                    velocity = -Vector3.UnitZ; //level speeds up as soon as player gains control at start
                    //velocity = Vector3.Zero;
                    //position.Z = 3999;
                }
                else //still in intro to level - do some interesting things with the camera here
                {
                    if (!p.InitialLock)
                    {
                        velocity = -Vector3.UnitZ;
                        if (!introRot1 && position.Z <= 7820)
                        {
                            introRot1 = Camera.Instance.translateCameraToTarget3(new Vector3(p.Position.X - 10, p.Position.Y, p.Position.Z), 0.20f);
                        }
                        if (introRot1 && !introRot2)
                        {
                            introRot2 = Camera.Instance.translateCameraToTarget3(new Vector3(p.Position.X, p.Position.Y + 20, p.Position.Z - 30), 0.25f);
                        }
                        if (introRot2)
                        {
                            returnedToTopDown = Camera.Instance.translateCameraToTarget3(Camera.Instance.TopDownPosition, 0.20f);
                        }
                    }
                }
            }
            else
            {
                if (!returnedToTopDown)
                {
                    returnedToTopDown = Camera.Instance.translateCameraToTarget3(Camera.Instance.TopDownPosition, 0.5f);
                }
            }
            #endregion
            #region Shortcuts
            if (Game1.debug)
            {
                KeyboardState k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.W) && !switchedToBoss && position.Z > preBossZ)
                {
                    destroyAllEnemies();
                    setRot = false;
                    setTrans = false;
                    position = new Vector3(-45, -700, preBossZ);
                    switchedToBoss = true;
                }
                if (k.IsKeyDown(Keys.E) && !switchedToMiniBoss && position.Z > infectorZ)
                {
                    //destroyAllEnemies();
                    setRot = false;
                    setTrans = false;
                    position = new Vector3(-45, -700, infectorZ);
                    switchedToMiniBoss = true;
                }
                if (k.IsKeyDown(Keys.C) && !switchedToClot && position.Z > clotZ)
                {
                    if (!miniBoss.isDead)
                        miniBoss.Die();
                    infectorDestroyed = true;
                    setRot = false;
                    setTrans = false;
                    position = new Vector3(-45, -700, clotZ);
                    switchedToClot = true;
                }
                if (k.IsKeyDown(Keys.D) && !switchedToStart && position.Z > 7600)
                {
                    setRot = false;
                    setTrans = false;
                    position = new Vector3(-45, -700, startGameZ);
                    switchedToStart = true;
                }
            }
            #endregion
            if (!bossAdded)
            {
                Add(new Boss(content, new Vector3(285, -1200, -400)));
                bossAdded = true;
            }
            if (reachedBoss)
            {
                UpdateBoss(gameTime);
                return;
            }
            if (!reachedInfectorZ)
            {
                if (position.Z < infectorZ)
                {
                    reachedInfectorZ = true;
                    for (int i = 0; i < entities.Count; i++)
                    {
                        if (entities[i] is MeleeBot || entities[i] is MissileBot)
                        {
                            EnemiesAndPlayer ep = entities[i] as EnemiesAndPlayer;
                            ep.Enabled = false;
                            ep.Velocity = Vector3.UnitZ;
                        }
                    }
                }
            }
            else
            {
                if (!reachedPreBossZ && !infectorDestroyed)
                {
                    UpdatePreMiniBoss(gameTime);
                }
            }
            if (!reachedClotZ)
            {
                if (position.Z < clotZ)
                {
                    reachedClotZ = true;
                }
            }
            else
            {
                if (!reachedPreBossZ && infectorDestroyed && !clotsDestroyed)
                {
                    UpdateClot(gameTime);
                }
            }
            if (!reachedPreBossZ)
            {
                if (position.Z < preBossZ)
                {
                    reachedPreBossZ = true;
                }
            }
            else
                UpdatePreBoss(gameTime);

            position += velocity;
            CheckCollision();

            bool fightingInfector = position.Z < infectorZ && !infectorDestroyed && miniBoss.Activated;
            bool inClotSection = position.Z < clotZ && infectorDestroyed;

            if (!reachedPreBossZ && (position.Z > infectorZ || infectorDestroyed))
            {
                if (position.Z > clotZ)
                    enemySpawner.Update(gameTime, entities[0].Position, getZ(), true, false); //add enemies along with cells
                else
                {
                    enemySpawner.Update(gameTime, entities[0].Position, getZ(), false, false); //add cells only
                }
            }
            else if (!reachedPreBossZ && (fightingInfector || inClotSection))
            {
                enemySpawner.Update(gameTime, entities[0].Position, getZ(), false, false); //add cells only
            }           

            levelWorldMat = Matrix.CreateScale(10) * Matrix.CreateRotationY(0) * Matrix.CreateTranslation(position);
            base.Update(gameTime);
        }

        public override void DrawLevel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            ModelMesh mesh;
            BasicEffect be;
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                mesh = model.Meshes[i];
                for (int j = 0; j < mesh.Effects.Count; j++)
                {
                    be = (BasicEffect)mesh.Effects[j];
                    be.Projection = projection;
                    be.View = view;
                    be.World = world;
                    be.TextureEnabled = true;
                    be.Texture = levelTexture;
                    be.EnableDefaultLighting();
                    be.LightingEnabled = true;
                    be.AmbientLightColor = new Vector3(0.9f, 0.2f, 0.2f);
                    be.EmissiveColor = new Vector3(1, 0, 0);
                    be.DirectionalLight0.Enabled = true;
                }
                mesh.Draw();
            }
        }

        public override void Draw()
        {
            DrawLevel(geometry, levelWorldMat, Camera.Instance.View, Camera.Instance.Projection);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Draw();
            }
        }

        public void destroyAllEnemies()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is EnemiesAndPlayer && !(entities[i] is Player) && !(entities[i] is Boss) && !(entities[i] is StraightCell))
                {
                    entities.Remove(entities[i]);
                }
            }
        }
    }
}
