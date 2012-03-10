using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class Laser : GameEntity
    {
        protected int splashRadius;
        protected bool exploded;

        public Laser(ContentManager content, Vector3 startPos, Vector3 velocity)
        {
            splashRadius = 50;
            exploded = false;

            this.velocity = velocity;
            model = content.Load<Model>(@"models/laser");
            texture = content.Load<Texture2D>(@"textures/lasertexture");
            position = startPos;
        
            rotate(new Vector3(MathHelper.ToRadians(90), -MathHelper.ToRadians(45), 0));
        }

        #region Update and Draw
        public void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        {
                position += velocity;
                sphere = new BoundingSphere(position, 1.2f);
                worldMat = Matrix.CreateScale(0.2f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
            
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }
        #endregion
        public override void Collision(GameEntity target)
        {

        }
    }
}
