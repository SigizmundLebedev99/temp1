using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace temp1.Systems
{
    class PossibleMovementDrawSystem : EntityDrawSystem
    {
        Mapper<PossibleMoves> _moveMapper;
        SpriteBatch _spriteBatch;
        Texture2D _texture;

        public PossibleMovementDrawSystem(SpriteBatch sb, ContentManager content) : base(Aspect.All(typeof(PossibleMoves)).Exclude(typeof(IMovement)))
        {
            _spriteBatch = sb;
            _texture = content.Load<Texture2D>("images/possible_move");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _moveMapper = mapperService.Get<PossibleMoves>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (ActiveEntities.Count == 0)
                return;
            var id = ActiveEntities[0];
            var move = _moveMapper.Get(id);
            for (var i = 0; i < move.Moves.Count; i++)
            {
                var p = move.Moves[i].point;
                var dist = move.Moves[i].distance;
                _spriteBatch.Draw(_texture, new Vector2(p.X * 32, p.Y * 32), Color.White);
            }
        }
    }
}