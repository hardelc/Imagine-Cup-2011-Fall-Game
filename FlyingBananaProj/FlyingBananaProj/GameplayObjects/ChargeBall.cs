using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class ChargeBall : GameEntity
    {
        public ChargeBall(ContentManager content, Vector3 startPos)
        {
            model = content.Load<Model>(@"models/sphere");
            texture = content.Load<Texture2D>(@"textures/lasertexture");
            position = startPos;
            sphere = new BoundingSphere(position, 0);
        }

        #region Update and Draw
        public void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        {
            sphere = new BoundingSphere(position, sphere.Radius + 0.5f);
            worldMat = Matrix.CreateScale(sphere.Radius) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);

            if (sphere.Radius >= 5)
            {
                Die();
            }
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
