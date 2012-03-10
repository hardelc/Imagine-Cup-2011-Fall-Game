using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class BotExplosion : GameEntity //explosion caused by enemy bot. Does damage to player only.
    {
        #region Class Variables
        protected int damage;
        protected int radius;
        protected float speed;
        protected float size;
        protected SoundEffect radiusSound;
        protected SoundEffect impactSound;
        protected bool hitPlayerOnce;
        double timer;
        double lifetimer;
        #endregion

        public BotExplosion(int damage, int radius, float speed, Vector3 pos, ContentManager content)
        {
            this.damage = 0;
            this.radius = radius;
            this.speed = speed;
            position = pos;
            size = 0.15f;
            model = content.Load<Model>(@"models/sphere");
            texture = content.Load<Texture2D>(@"textures/explosiontexture");
            radiusSound = content.Load<SoundEffect>(@"audio/sfx/misslewhoosh");
            impactSound = content.Load<SoundEffect>(@"audio/sfx/MissleExplosion");
            impactSound.Play(0.1f, 1, 0);
            radiusSound.Play(0.5f, 1, 0);
            sphere = new BoundingSphere(position, 0.125f);
            hitPlayerOnce = false;
            timer = 0;
            lifetimer = 500;
        }

        public override void Update(GameTime gameTime)
        {
            size = sphere.Radius / 18.0f;
            sphere = new BoundingSphere(position, sphere.Radius + 0.2f);
            worldMat = Matrix.CreateScale(sphere.Radius) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);

            timer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > lifetimer)
            {
                Die();
            }
        }

        public override void Draw()
        {
            BasicEffect be;
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                for (int j = 0; j < model.Meshes[i].Effects.Count; j++)
                {
                    be = (BasicEffect)model.Meshes[i].Effects[j];
                    be.Projection = Camera.Instance.Projection;
                    be.View = Camera.Instance.View;
                    be.World = worldMat;
                    be.EnableDefaultLighting();
                    be.LightingEnabled = true;
                    be.AmbientLightColor = new Vector3(0.9f, 0.2f, 0.2f);
                    be.EmissiveColor = new Vector3(1, 0, 0);
                    be.DirectionalLight0.Enabled = true;
                }
                model.Meshes[i].Draw();
            }
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player && !hitPlayerOnce)
            {
                Player p = target as Player;
                p.takeDamage(damage);
                hitPlayerOnce = true;
            }
        }
    }
}
