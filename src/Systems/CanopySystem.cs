using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Systems
{
    [With(typeof(Canopy))]
    [With(typeof(RenderingObject))]
    class CanopySystem : AEntitySetSystem<GameTime>
    {
        public CanopySystem(World world) : base(world)
        {
        }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var hull = entity.Get<Canopy>();
            var mapObjects = GameContext.EntitySets.MapObjects.GetEntities();
            var playerPosition = GameContext.Player.Get<Position>();
            var gridCell = playerPosition.GridCell;
            var layer = hull.Layer;
            var tile = layer.GetTile((ushort)gridCell.X, (ushort)gridCell.Y);
            var sprite = entity.Get<RenderingObject>();
            sprite.Visible = tile.IsBlank;
            
            if (!hull.IsInterier)
                return;

            RenderingObject renderingObject;

            for (var i = 0; i < mapObjects.Length; i++)
            {
                var mapObject = mapObjects[i];
                if(mapObject == GameContext.Player)
                    continue;
                var mapObjectPosition = mapObject.Get<Position>();

                if(!mapObject.Has<RenderingObject>())
                    continue;
                
                renderingObject = mapObject.Get<RenderingObject>();
                gridCell = mapObjectPosition.GridCell;
                layer = hull.Layer;
                tile = layer.GetTile((ushort)gridCell.X, (ushort)gridCell.Y);
                renderingObject.Visible = !sprite.Visible || tile.IsBlank;
            }
        }
    }
}