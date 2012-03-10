using Microsoft.Xna.Framework;
namespace FlyingBananaProj
{
    enum CameraAction
    {
        translateCameraToTarget3,
        Rotating
    }
    class Camera
    {
        public static Camera Instance;

        public Matrix View;
        public Matrix Projection;
        protected Vector3 topDownPos;
        protected CameraAction currentAction;
        public Vector3 _cameraPosition;
        protected float degreeAmount;
        protected float transAmount;
        private float _viewAngle;
        private Vector3 vectorStep, vectorTarget;
        private readonly float _aspectRatio;
        private readonly float _nearClip;
        private readonly float _farClip;

        public Camera(Game game)
        {
            topDownPos = new Vector3(0, 80, -1);
            Instance = this;
            _nearClip = 1.0f;
            _farClip = 2000.0f;
            _viewAngle = MathHelper.Pi / 3;
            _cameraPosition = topDownPos;
            _aspectRatio = (float)game.GraphicsDevice.Viewport.Width / game.GraphicsDevice.Viewport.Height;
        }

        public Vector3 TopDownPosition
        {
            get { return topDownPos; }
        }

        public void resetPosition()
        {
            _cameraPosition = new Vector3(0, 95, -1);
        }
        public void resetTopDownPosition()
        {
            _cameraPosition = topDownPos;
        }

		public void setPosition(Vector3 newPos)
        {
            _cameraPosition = newPos;
        }
        public void changeZoom(float angleChange)
        {
            if (_viewAngle + angleChange >= 0 && _viewAngle + angleChange <= MathHelper.Pi) 
                _viewAngle += angleChange;
        }

        public void translateCameraPosStep(Vector3 transStep)
        {
            _cameraPosition += transStep;
        }
        public void setRotationAmountInDegrees1(float amountInDegrees) //used before every call to RotateCamera3StepAmount
        {
            degreeAmount = amountInDegrees;
        }
        public void setTranslationAmount(float amount) //used before every call to TranslateCamera3StepAmount
        {
            transAmount = amount;
        }
        public bool RotateCamera3StepAmount(Vector3 rotStep)
        {
            if (degreeAmount <= 0) return true;
            else
            {
                rotateCameraPos(rotStep);
                degreeAmount--;
                return false;
            }
        }

        public bool TranslateCamera3StepAmount(Vector3 transStep)
        {
            if (transAmount <= 0) return true;
            else
            {
                translateCameraPosStep(transStep);
                transAmount--;
                return false;
            }
        }

        public void rotateCameraPos(Vector3 rotStep)
        {
            _cameraPosition = Vector3.Transform(_cameraPosition, UpdatedCameraRotation(rotStep));
        }

        public void rotateCamera3(Vector3 rotStep, Vector3 rotTarget)
        {
            vectorStep = rotStep;
            vectorTarget = rotTarget;
            bool xIsReachable = isReachableTrans(_cameraPosition.X, rotStep.X, rotTarget.X);
            bool yIsReachable = isReachableTrans(_cameraPosition.Y, rotStep.Y, rotTarget.Y);
            bool zIsReachable = isReachableTrans(_cameraPosition.Z, rotStep.Z, rotTarget.Z);
            float error = 2;
            if (Vector3.Distance(_cameraPosition, rotTarget) < error)
            {
                return;
            }
            else
            {
                currentAction = CameraAction.Rotating;
                rotateCameraPos(rotStep);
            }
        }

        public Vector3 getPosition()
        {
            return _cameraPosition;
        }

        public bool translateCameraToTarget3(Vector3 posTarget, float factor)
        {
            Vector3 step = posTarget - _cameraPosition;
            step.Normalize();
            step *= factor;

            if (Vector3.Distance(posTarget, _cameraPosition) < 1)
            {
                _cameraPosition = posTarget;
                return true;
            }
            else
            {
                currentAction = CameraAction.translateCameraToTarget3;
                translateCameraPosStep(step);
                return false;
            }
        }

        private bool isReachableTrans(float orig, float step, float target)
        {
            if (step == 0.0f)
            {
                if (orig != target)
                    return false;
                else return true;
            }
            else if (step > 0.0f)
            {
                if (orig > target)
                    return false;
                else
                    return true;
            }
            else
            {
                if (orig < target)
                    return false;
                else
                    return true;
            }
        }

        private bool isReachableRot(float orig, float step, float target)
        {
            if (step == 0.0f)
            {
                if (orig != target)
                    return false;
                else return true;
            }
            else if (step > 0.0f)
            {
                if (orig > target)
                    return false;
                else
                    return true;
            }
            else
            {
                if (orig < target)
                    return false;
                else
                    return true;
            }
        }

        public void flipScreen()
        {
            _cameraPosition.Z = 0 - _cameraPosition.Z;
        }

        private Matrix UpdatedCameraRotation(Vector3 rotation)
        {
            return Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z);
        }

        private Vector3 UpdatedTransformedReference(Matrix rotationMatrix)
        {
            return Vector3.Transform(_cameraPosition, rotationMatrix);
        }

        private Vector3 UpdatedCameraPosition(Vector3 transformedReference, Vector3 playerPosition)
        {
            return transformedReference + playerPosition;
        }

        private void UpdatedViewMatrix(Vector3 cameraPosition, Vector3 playerPosition)
        {
            View = Matrix.CreateLookAt(cameraPosition, playerPosition, new Vector3(0.0f, 1.0f, 0.0f));
        }

        private void UpdateProjectionMatrix()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(_viewAngle, _aspectRatio, _nearClip, _farClip);
        }

        public void UpdateCamera(Vector3 playerPosition, Vector3 rotation)
        {
            var tempCameraRotation = UpdatedCameraRotation(rotation);
            var tempTransformedReference = UpdatedTransformedReference(tempCameraRotation);
            var tempCameraPosition = UpdatedCameraPosition(tempTransformedReference, playerPosition);
            UpdatedViewMatrix(tempCameraPosition, playerPosition);
            UpdateProjectionMatrix();
        }
    }
}