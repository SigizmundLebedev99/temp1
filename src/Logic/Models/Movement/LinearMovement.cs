using Microsoft.Xna.Framework;

namespace temp1.Models.Movement
{
    class LinearMovement : Movement
    {
        private float _speed;
        private float k;

        public LinearMovement(Vector2 from, Vector2 to, float speed) : base(from, to)
        {
            this._speed = speed / (to - from).Length();
        }

        public override bool IsCompleted => k >= 1f;

        public override Vector2 Move()
        {
            k += _speed;
            var result = this.To * k + this.From * (1 - k);
            return result;
        }
    }
}