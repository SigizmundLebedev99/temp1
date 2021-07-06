using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using temp1.Components;
using temp1.Models;
using temp1.PathFinding;
using temp1.UI;

namespace temp1.Systems
{
    [With(typeof(Player))]
    [With(typeof(AllowedToAct))]
    class PlayerControlSystem : AEntitySetSystem<GameTime>
    {
        MouseStateExtended mouseState;
        DefaultPathFinder pathFinder = new DefaultPathFinder();

        public PlayerControlSystem(World world) : base(world)
        {
        }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var newState = MouseExtended.GetState();
            if (!CanHandleInput(newState))
            {
                mouseState = newState;
                return;
            }

            var pointed = GameContext.PointedEntity != null ? GameContext.PointedEntity.Value.Get<Position>() : null;
            BaseAction after = null;
            var mapPosition = GameContext.Camera.ScreenToWorld(mouseState.Position.X, mouseState.Position.Y);
            var gridCell = mapPosition.GridCell();
            
            if (pointed != null)
                after = GetAfterAction(entity, GameContext.PointedEntity.Value);
            else if (!GameContext.Map.MovementGrid.IsWalkableAt(gridCell.X, gridCell.Y))
            {
                return;
            }

            if (pathFinder.TryGetPath(entity, gridCell, out var first, out var last))
            {
                if (after != null)
                {
                    var action = last ?? first;
                    action.Alternative = after;
                    action.Abort();
                }

                entity.Set<BaseAction>(first);
            }

            mouseState = newState;
        }

        private bool CanHandleInput(MouseStateExtended newState)
        {
            return mouseState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released
            && GameContext.Hud.State == HUDState.Default
            && !GameContext.Hud.IsMouseOnHud;
        }

        private BaseAction GetAfterAction(Entity entity, Entity pointedEntity)
        {
            if(!pointedEntity.Has<GameObjectType>())
                return null;
            var type = pointedEntity.Get<GameObjectType>();
            if ((type & GameObjectType.Item) != 0)
                return new PeakItemAction(entity, pointedEntity);
            if ((type & GameObjectType.Storage) != 0)
                return new OpenStorageAction(pointedEntity);

            return null;
        }
    }
}