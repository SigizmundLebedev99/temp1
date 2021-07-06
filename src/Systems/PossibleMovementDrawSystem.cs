using Microsoft.Xna.Framework;
using temp1.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DefaultEcs.System;
using DefaultEcs;
using System;

namespace temp1.Systems
{
    [With(typeof(AllowedToAct))]
    [With(typeof(PossibleMoves))]
    [Without(typeof(BaseAction))]
    [Without(typeof(TurnOccured))]
    class PossibleMovementDrawSystem : AEntitySetSystem<GameTime>
    {
        SpriteBatch _spriteBatch;
        Texture2D _texture;

        public PossibleMovementDrawSystem(World world, SpriteBatch sb) : base(world)
        {
            _spriteBatch = sb;
            _texture = Content.Load<Texture2D>("images/possible_move");
        }

        protected override void Update(GameTime state, in Entity entity)
        {
            var moves = entity.Get<PossibleMoves>();
            for (var i = 0; i < moves.Value.Count; i++)
            {
                var p = moves.Value[i].point;
                var dist = moves.Value[i].distance;
                _spriteBatch.Draw(_texture, new Vector2(p.X * 32, p.Y * 32), Color.White);
            }
        }
    }
}