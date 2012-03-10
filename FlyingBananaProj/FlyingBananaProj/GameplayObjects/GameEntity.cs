using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class GameEntity
    {
        #region Class Variables
        public Vector3 position;
        protected Vector3 velocity;
        protected Vector3 acceleration;
        protected Vector3 rotation;
        protected Model model;
        public BoundingSphere sphere { get; set; }
        protected Texture2D texture;
        protected Matrix worldMat;
        protected bool dead = false;
        #endregion

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        public Model Model
        {
            get { return model; }
            set { model = value; }
        }

        public Matrix World
        {
            get { return worldMat; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public bool isDead
        {
            get { return dead; }
        }

        public virtual void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            BasicEffect be;
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                for (int j = 0; j < model.Meshes[i].Effects.Count; j++)
                {
                    be = (BasicEffect)model.Meshes[i].Effects[j];
                    be.Projection = projection;
                    be.View = view;
                    be.World = world;
                    be.TextureEnabled = true;
                    be.Texture = texture;
                    be.EnableDefaultLighting();
                    be.LightingEnabled = true;
                    be.AmbientLightColor = new Vector3(0.9f, 0.2f, 0.2f);
                    be.EmissiveColor = new Vector3(1, 0, 0);
                    be.DirectionalLight0.Enabled = true;
                }
                model.Meshes[i].Draw();
            }
        }

        public virtual void Update(GameTime gameTime, List<GameEntity> entities)
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void checkBounds()
        {
            if (position.Z < -270 || position.Z > 250)
            {
                Die();
            }
        }

        public virtual void Draw()
        {
        }

        public void Die()
        {
            dead = true;
        }
        public virtual void Collision(GameEntity target)
        {
        }
        public void rotate(Vector3 newRot)
        {
            rotation += newRot;
            if (rotation.X >= (Math.PI * 2))
            {
                rotation.X -= (float)(Math.PI * 2);
            }
            if (rotation.X < 0)
            {
                rotation.X += (float)(Math.PI * 2);
            }
            if (rotation.Y >= (Math.PI * 2))
            {
                rotation.Y -= (float)(Math.PI * 2);
            }
            if (rotation.Y < 0)
            {
                rotation.Y += (float)(Math.PI * 2);
            }
            if (rotation.Z >= (Math.PI * 2))
            {
                rotation.Z -= (float)(Math.PI * 2);
            }
            if (rotation.Z < 0)
            {
                rotation.Z += (float)(Math.PI * 2);
            }
        }
    }
}