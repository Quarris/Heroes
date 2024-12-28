using Microsoft.Xna.Framework;
using MonoGame.Extended.ViewportAdapters;

namespace Heroes.Client
{
    public class Camera
    {
        private Matrix _transform;
        private Vector2 _position;
        private float _zoom;
        private readonly ViewportAdapter _viewportAdapter;
        private Vector2 _targetPosition;
        private float _followSpeed = 5f;
        private Rectangle? _bounds;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = ClampToBounds(value);
                UpdateMatrix();
            }
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = MathHelper.Clamp(value, 0.1f, 5f);
                UpdateMatrix();
            }
        }

        public Matrix Transform => _transform;

        public Rectangle? Bounds
        {
            get => _bounds;
            set
            {
                _bounds = value;
                // Ensure current position respects new bounds
                Position = _position;
            }
        }

        public float FollowSpeed
        {
            get => _followSpeed;
            set => _followSpeed = MathHelper.Max(0.1f, value);
        }

        public Camera(ViewportAdapter viewportAdapter)
        {
            _viewportAdapter = viewportAdapter;
            _position = Vector2.Zero;
            _targetPosition = Vector2.Zero;
            _zoom = 1f;
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            // Get the center point of the virtual viewport
            Vector2 virtualCenter = new(_viewportAdapter.VirtualWidth * 0.5f, _viewportAdapter.VirtualHeight * 0.5f);

            _transform = Matrix.CreateTranslation(new Vector3(-_position * Constants.TileSize, 0.0f)) *
                         Matrix.CreateScale(new Vector3(_zoom, _zoom, 1.0f)) *
                         Matrix.CreateTranslation(new Vector3(virtualCenter, 0.0f)) *
                         _viewportAdapter.GetScaleMatrix();
            //* 
            //             Matrix.CreateTranslation(new Vector3(virtualCenter, 0.0f)) *
            //             
        }

        private Vector2 ClampToBounds(Vector2 position)
        {
            if (_bounds.HasValue)
            {
                float virtualViewWidth = _viewportAdapter.VirtualWidth / _zoom;
                float virtualViewHeight = _viewportAdapter.VirtualHeight / _zoom;

                // Calculate the visible area edges
                float minX = _bounds.Value.Left + virtualViewWidth / 2;
                float maxX = _bounds.Value.Right - virtualViewWidth / 2;
                float minY = _bounds.Value.Top + virtualViewHeight / 2;
                float maxY = _bounds.Value.Bottom - virtualViewHeight / 2;

                // If the bounds are too small for the current zoom level, center on the bounds
                if (minX > maxX)
                {
                    position.X = _bounds.Value.Center.X;
                }
                else
                {
                    position.X = MathHelper.Clamp(position.X, minX, maxX);
                }

                if (minY > maxY)
                {
                    position.Y = _bounds.Value.Center.Y;
                }
                else
                {
                    position.Y = MathHelper.Clamp(position.Y, minY, maxY);
                }
            }

            return position;
        }

        public void Follow(Vector2 target)
        {
            _targetPosition = target;
        }

        public void Update(GameTime gameTime)
        {
            if (_targetPosition != _position)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position = Vector2.Lerp(_position, _targetPosition, _followSpeed * delta);
            }
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(_transform));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, _transform);
        }

        public void LookAt(Vector2 position)
        {
            Position = position;
            _targetPosition = position; // Reset the target to prevent smooth movement
        }

        public void SetBounds(int width, int height)
        {
            Bounds = new Rectangle(0, 0, width, height);
        }
    }
}