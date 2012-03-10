using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class Infector : EnemiesAndPlayer
    {
        protected List<TimeSpan> convertingCells;
        protected bool activated;
        protected BotWeapon weapon;
        protected BoundingSphere cellSphere; //sphere at which straightcells are absorbed
        protected ContentManager content;
        public Infector(ContentManager content, Vector3 position)
        {
            this.content = content;
            this.position = position;
            velocity = Vector3.Zero;
            health = 250;
            Model = content.Load<Model>(@"models/infector");
            texture = content.Load<Texture2D>(@"models/BossTexture");
            name = RealName.Boss1;
            score = 15;
            weapon = new BotMissile(content);
            activated = false;
            cellSphere = new BoundingSphere(position, 80);
            convertingCells = new List<TimeSpan>();
            rotate(new Vector3(0, MathHelper.ToRadians(90), 0));
        }

        public BoundingSphere CellSphere
        {
            get { return cellSphere; }
        }

        public bool Activated
        {
            get { return activated; }
        }

        public void Activate()
        {
            activated = true;
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            if (position.Z < 50)
            {
                activated = true;
            }
            if (!activated)
            {
                velocity = new Vector3(0, 0, -1);
            }
            else
            {
                velocity = Vector3.Zero;
            }

            for (int i = 0; i < convertingCells.Count; i++)
            {
                if (convertingCells[i].TotalSeconds <= 0)
                {
                    convertingCells.Remove(convertingCells[i]);
                    Level.Add(new LatchingCell(content, position, Vector3.Zero));
                }
                else
                {
                    convertingCells[i] = convertingCells[i].Subtract(gameTime.ElapsedGameTime);
                }
            }

            position += velocity;
            sphere = new BoundingSphere(position, 5);
            cellSphere = new BoundingSphere(new Vector3(position.X, position.Y, position.Z+40), 80);
            worldMat = Matrix.CreateScale(1.25f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
            //infector will never move while fighting - no need to call checkBounds()
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public void inRange(GameEntity target)
        {
            if (target is StraightCell)
            {
                StraightCell sc = target as StraightCell;
                sc.Absorb();
                Vector3 newVel = position - sc.Position;
                newVel.Normalize();
                sc.Velocity = newVel;
            }
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player)
            {
                Vector3 newPos = target.Position + target.Velocity;
                BoundingSphere bs = new BoundingSphere(newPos, target.sphere.Radius);
                if (bs.Intersects(this.sphere))
                {
                    target.Position -= target.Velocity;
                }              
            }
            else if (target is PlayerWeapon)
            {
                target.Collision(this);
            }
            else if (target is StraightCell)
            {
                target.Die();
                TimeSpan ts = TimeSpan.FromSeconds(1);
                convertingCells.Add(ts);
            }
        }
    }
}