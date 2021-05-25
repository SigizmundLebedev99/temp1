using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;
using temp1.UI;

namespace temp1.Systems
{
    class CursorSystem : AEntitySetSystem<GameTime>
    {
        Vector2 position;
        AnimatedSprite mark;
        SpriteBatch _spriteBatch;

        public CursorSystem(SpriteBatch batch, World world) : base(world)
        {
            _spriteBatch = batch;
            mark = GameContext.Content.GetAnimatedSprite("images/mark.sf");
        }

        protected override void Update(GameTime gameTime, ReadOnlySpan<Entity> _)
        {
            if (GameContext.Hud.State != HUDState.Default || GameContext.Hud.IsMouseOnHud || !GameContext.Player.Has<AllowedToAct>())
                return;
            var map = GameContext.Map;
            var state = MouseExtended.GetState();
            var worldPos = GameContext.Camera.ScreenToWorld(state.Position.X, state.Position.Y);
            var point = (worldPos / 32).ToPoint();
            position = point.ToVector2() * 32 + new Vector2(16);
            GameContext.PointedEntity = null;
            if (GameContext.GameState == GameState.Peace)
            {
                if (!map.MovementGrid.Contains(point))
                    return;


                if (!HandlePoint(worldPos))
                {
                    if (map.MovementGrid.IsWalkableAt(point.X, point.Y))
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
                    if (!HandlePoint(worldPos))
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

        private bool HandlePoint(Vector2 pos)
        {
            var entities = GameContext.EntitySets.Cursors.GetEntities();
            for (var i = 0; i < entities.Length; i++)
            {
                var cursor = entities[i];

                var position = cursor.Get<Position>();
                var pointable = cursor.Get<Cursor>();
                if (pointable.Bounds.Contains(pos - position.Value))
                {
                    mark.Play(pointable.SpriteName);
                    this.position = pos;
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