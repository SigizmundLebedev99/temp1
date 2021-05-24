using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.UI;

namespace temp1.Systems
{
    [With(typeof(Cursor))]
    class CursorSystem : AEntitySetSystem<GameTime>
    {
        Vector2 position;
        AnimatedSprite mark;
        SpriteBatch _spriteBatch;

        MapContext _map;

        public CursorSystem(SpriteBatch batch, World world) : base(world)
        {
            _map = GameContext.Map;
            _spriteBatch = batch;
            mark = GameContext.Content.GetAnimatedSprite("images/mark.sf");
        }

        protected override void Update(GameTime gameTime, ReadOnlySpan<Entity> entities)
        {
            if (GameContext.Hud.State != HUDState.Default || GameContext.Hud.IsMouseOnHud || !GameContext.Player.Has<AllowedToAct>())
                return;

            var state = MouseExtended.GetState();
            var worldPos = GameContext.Camera.ScreenToWorld(state.Position.X, state.Position.Y);
            var point = (worldPos / 32).ToPoint();
            position = point.ToVector2() * 32 + new Vector2(16);
            GameContext.PointedEntity = null;
            if (GameContext.GameState == GameState.Peace)
            {
                if (!_map.MovementGrid.Contains(point))
                    return;
                

                if (!HandlePoint(worldPos, entities))
                {
                    if (_map.MovementGrid.IsWalkableAt(point.X, point.Y))
                        mark.Play("idle");
                    else
                        mark.Play("no");
                }
            }
            else
            {
                var possibleMoves = GameContext.Player.Get<PossibleMoves>();
                if (possibleMoves.Contains(point))
                {
                    if (!HandlePoint(worldPos, entities))
                        mark.Play("idle");
                }
                else
                {
                    mark.Play("no");
                }
            }
            Draw();
            mark.Update(gameTime.ElapsedGameTime.Milliseconds);
        }

        private bool HandlePoint(Vector2 pos, ReadOnlySpan<Entity> entities)
        {
            for (var i = 0; i < entities.Length; i++)
            {
                var cursor = entities[i];
                var sprite = cursor.Get<RenderingObject>();
                var mo = cursor.Get<MapObject>();
                var bounds = new Rectangle(0, 0, sprite.Bounds.Width, sprite.Bounds.Height);
                var pointable = cursor.Get<Cursor>();
                if (bounds.Contains(pos - mo.Position + sprite.Origin))
                {
                    mark.Play(pointable.SpriteName);
                    position = pos;
                    GameContext.PointedEntity = cursor;
                    return true;
                }
            }
            return false;
        }

        void Draw()
        {
            _spriteBatch.Draw(
                mark.TextureRegion.Texture,
                position,
                mark.TextureRegion.Bounds,
                Color.White,
                0,
                new Vector2(16),
                1, SpriteEffects.None, 0f);
        }
    }
}