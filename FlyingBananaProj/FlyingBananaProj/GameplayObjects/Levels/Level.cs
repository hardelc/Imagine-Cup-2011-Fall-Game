using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace FlyingBananaProj
{
    abstract class Level
    {
        #region Class Variables
        protected bool levelEnded;
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        protected static List<GameEntity> entities = new List<GameEntity>();
        protected Random random = new Random();
        protected Texture2D levelTexture;
        protected Vector3 position;
        protected Vector3 velocity;
        protected Matrix levelWorldMat;
        protected Model geometry;
        protected EnemySpawner enemySpawner;
        protected float preBossZ; //the Z value the level needs to reach before locking the player's controls
        protected bool reachedPreBossZ; //indicates whether we have reached this Z value.
        protected bool reachedBoss; //indicates when we can return the player's control to begin the boss fight
        protected float thirdPersonZ; //the Z value the level needs to reach before changing the player's perspective
        protected bool firstMove, secondMove; //did the player make the initial move when getting ready for the boss? the next?
        #endregion

        public bool isFinished()
        {
            return levelEnded;
        }

        public virtual void Update(GameTime gameTime)
        {
            Player p = entities[0] as Player;
            if (p.SwitchedFromShield)
            {
                for (int j = 0; j < entities.Count; j++)
                {
                    if (entities[j] is Shield)
                    {
                        entities.Remove(entities[j]);
                        j = 0;
                    }
                }
                p.SwitchedFromShield = false;
            }
            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].isDead)
                {
                    if (entities[i] is ChargeBall)
                    {
                        ChargeBall cb = entities[i] as ChargeBall;
                        cb.Update(gameTime, p.Position, p.Rotation);
                    }
                    if (entities[i] is Laser)
                    {
                        Laser l = entities[i] as Laser;
                        l.Update(gameTime, p.Position, p.Rotation);
                    }
                    if (entities[i] is Boss)
                    {
                        Boss b = entities[i] as Boss;
                        b.Update(gameTime, p.Position);
                    }
                    if (entities[i] is ClotSide)
                    {
                        ClotSide cs = entities[i] as ClotSide;
                        cs.Update(gameTime, velocity);
                    }
                    if (entities[i] is Infector)
                    {
                        Infector inf = entities[i] as Infector;
                        inf.Update(gameTime, p.Position);
                    }
                    if (entities[i] is TokenPickup)
                    {
                        TokenPickup tp = entities[i] as TokenPickup;
                        tp.Update(gameTime);
                    }
                    if (entities[i] is PlayerWeapon)
                    {
                        PlayerWeapon w = entities[i] as PlayerWeapon;
                        w.Update(gameTime, p.Position, p.Rotation);
                    }
                    if (entities[i] is BotWeapon)
                    {
                        BotWeapon bw = entities[i] as BotWeapon;
                        bw.Update(gameTime, p.Position, p.Rotation);
                    }
                    if (entities[i] is Shield)
                    {
                        Shield s = entities[i] as Shield;
                        s.Update(gameTime, p.Velocity, p.controls.isPlayerFiring());
                    }
                    if (entities[i] is LatchingCell || entities[i] is MeleeBot || entities[i] is MissileBot)
                    {
                        if (entities[i] is LatchingCell)
                        {
                            LatchingCell lc = entities[i] as LatchingCell;
                            lc.Update(gameTime, p.Position);
                        }
                        else if (entities[i] is MeleeBot)
                        {
                            MeleeBot mb = entities[i] as MeleeBot;
                            mb.Update(gameTime, p.Position);
                        }
                        else
                        {
                            MissileBot missBot = entities[i] as MissileBot;
                            missBot.Update(gameTime, p.Position);
                        }
                    }
                    entities[i].Update(gameTime);
                }
                else
                {
                    if (entities[i] is EnemiesAndPlayer)
                    {
                        EnemiesAndPlayer ep = entities[i] as EnemiesAndPlayer;
                        if (ep.killedBy())
                        {
                            p.addScore(ep.Score);
                            p.AddPowerup(15);
                            if (ep is MeleeBot || ep is MissileBot)
                            {
                                if (random.Next(2) == 0)
                                    Add(new TokenPickup(content, ep.Position));
                            }
                        }
                    }
                    entities.Remove(entities[i]);
                }
            }
        }

        public virtual void Draw() 
        {
 
        }

        public virtual void DrawLevel(Model model, Matrix world, Matrix view, Matrix projection)
        {
 
        }

        public static void Add(GameEntity newEnt)
        {
            entities.Add(newEnt);
        }

        public void Destroy()
        {
            entities.Clear();
        }

        public bool ReachedPreBossZ
        {
            get { return reachedPreBossZ; }
        }

        public float getZ()
        {
            return position.Z;
        }

        public void StopCurrentMusic()
        {
            MediaPlayer.Stop();
        }

        public static void CheckCollision()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                for (int j = i + 1; j < entities.Count; j++)
                {
                    if (entities[i] is ClotSide || entities[j] is ClotSide)
                    {
                        if (entities[i] is ClotSide)
                        {
                            ClotSide cs = entities[i] as ClotSide;
                            if (cs.SmallSphere.Intersects(entities[j].sphere) && !entities[i].isDead && !entities[j].isDead)
                            {
                                entities[i].Collision(entities[j]);
                            }
                        }
                        else
                        {
                            ClotSide cs = entities[j] as ClotSide;
                            if (cs.SmallSphere.Intersects(entities[i].sphere) && !entities[i].isDead && !entities[j].isDead)
                            {
                                entities[j].Collision(entities[i]);
                            }
                        }
                    }

                    if (entities[i] is Infector || entities[j] is Infector)
                    {
                        if (entities[i] is Infector)
                        {
                            Infector inf = entities[i] as Infector;
                            if (inf.CellSphere.Intersects(entities[j].sphere) && !inf.isDead && !entities[j].isDead)
                            {
                                inf.inRange(entities[j]);
                            }
                        }
                        else
                        {
                            Infector inf = entities[j] as Infector;
                            if (inf.CellSphere.Intersects(entities[i].sphere) && !inf.isDead && !entities[i].isDead)
                            {
                                inf.inRange(entities[i]);
                            }
                        }
                    }
                    if (entities[i].sphere.Intersects(entities[j].sphere) && !entities[i].isDead && !entities[j].isDead)
                    {
                        entities[i].Collision(entities[j]);
                    }
                }
            }
        }

    }
}
