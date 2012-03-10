using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    enum Side
    {
        Left, Right
    }
    class ClotSide : EnemiesAndPlayer
    {
        protected Side side;
        protected bool activated;
        protected bool seal;
        protected BoundingBox clotBox;
        protected BoundingSphere smallSphere;
        List<Vector3> points;
        public ClotSide(ContentManager content, Vector3 position, Side side)
        {
            this.position = position;
            this.side = side;
            health = 1;
            Model = content.Load<Model>(@"models/clot");
            texture = content.Load<Texture2D>(@"textures/redBloodCellTexture");
            name = RealName.ClotSide;
            score = 200;
            activated = false;
            points = new List<Vector3>();
            seal = false;
            if (side == Side.Left)
            {
                rotate(new Vector3(0, MathHelper.ToRadians(180), 0));
                smallSphere = new BoundingSphere(new Vector3(position.X - 50, position.Y, position.Z), health / 50);
                points.Add(new Vector3(position.X, 50, position.Z));
            }
            else
            {
                smallSphere = new BoundingSphere(new Vector3(position.X + 50, position.Y, position.Z), health / 50);
                points.Add(new Vector3(position.X, 50, position.Z));
            }
        }

        public BoundingBox ClotBox
        {
            get { return clotBox; }
        }

        public BoundingSphere SmallSphere
        {
            get { return smallSphere; }
        }

        public bool Seal
        {
            get { return seal; }
        }

        public new void Update(GameTime gameTime, Vector3 levelVel)
        {
            if (activated)
            {
                position += levelVel;
                health += 1;
            }
            health += 1;
            sphere = new BoundingSphere(position, health / 10);
            if (side == Side.Left)
            {
                smallSphere = new BoundingSphere(new Vector3(position.X - health/15, position.Y, position.Z), health / 20);
            }
            else
            {
                smallSphere = new BoundingSphere(new Vector3(position.X + health / 15, position.Y, position.Z), health / 20);
            }
            worldMat = Matrix.CreateScale(new Vector3((float)health / 60, 1, (float)health / 60)) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
            if (points.Count == 2)
            {
                points.RemoveAt(1);
            }
            if (side == Side.Left)
                points.Add(new Vector3(position.X - health / 6, -50, position.Z + 5));
            else
                points.Add(new Vector3(position.X + health / 6, -50, position.Z + 5));
            clotBox = BoundingBox.CreateFromPoints(points);
            checkBounds();
        }

        public void Activate()
        {
            activated = true;
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player)
            {
                Vector3 newPos = target.Position + target.Velocity;
                BoundingSphere bs = new BoundingSphere(newPos, target.sphere.Radius);
                if (bs.Intersects(clotBox))
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
                if (side == Side.Left)
                {
                    target.Position -= Vector3.UnitX;
                }
                else target.Position += Vector3.UnitX;
            }
            else if (target is ClotSide)
            {
                //game over
                seal = true;
            }
        }
    }
}
