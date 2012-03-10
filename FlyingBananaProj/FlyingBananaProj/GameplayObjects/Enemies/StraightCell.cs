using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class StraightCell : EnemiesAndPlayer
    {
        protected bool absorbed;
        protected Random rand;
        protected int rot;
        public StraightCell(ContentManager content, Vector3 position)
        {
            this.position = position;
            velocity = new Vector3(0, 0, -0.5f);
            health = 10;
            Model = content.Load<Model>(@"models/redbloodcell");
            texture = content.Load<Texture2D>(@"textures/redBloodCellTexture");
            name = RealName.StraightCell;
            score = -20;
            absorbed = false;

            rand = new Random();

            rot = rand.Next(6);

        }

        public void Absorb()
        {
            absorbed = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!absorbed)
                velocity = new Vector3(0, 0, -0.5f);
            position += velocity;
            sphere = new BoundingSphere(position, 2);
            worldMat = Matrix.CreateScale(0.8f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
            checkBounds();
            switch (rot)
            {
                case 0:
                    rotate(new Vector3(MathHelper.ToRadians(2), 0, 0));
                    break;
                case 1:
                    rotate(new Vector3(0, MathHelper.ToRadians(2), 0));
                    break;
                case 2:
                    rotate(new Vector3(0, 0, MathHelper.ToRadians(2)));
                    break;
                case 3:
                    rotate(new Vector3(-MathHelper.ToRadians(2), 0, 0));
                    break;
                case 4:
                    rotate(new Vector3(0, -MathHelper.ToRadians(2), 0));
                    break;
                case 5:
                    rotate(new Vector3(0, 0, -MathHelper.ToRadians(2)));
                    break;
            }
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player || target is Shield)
            {
                velocity = position - target.Position;
                velocity.Normalize();
                position += velocity;
            }
            else if (target is PlayerWeapon)
            {
                target.Collision(this);
            }
            else if (target is Explosion)
            {
                target.Collision(this);
            }
        }
    }
}
