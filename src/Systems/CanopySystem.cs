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
        EntitySet MapObjectsSet;

        public CanopySystem(World world) : base(world)
        {
            MapObjectsSet = GameContext.EntitySets.MapObjects;
        }

        protected override void Update(GameTime gameTime, in Entity entity)
        {
            var hull = entity.Get<Canopy>();
            var mapObjects = MapObjectsSet.GetEntities();
            var playerMO = GameContext.Player.Get<MapObject>();
            var position = playerMO.MapPosition;
            var layer = hull.Layer;
            var tile = layer.GetTile((ushort)position.X, (ushort)position.Y);
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
                var mo = mapObject.Get<MapObject>();

                if(!mapObject.Has<RenderingObject>())
                    continue;
                
                renderingObject = mapObject.Get<RenderingObject>();
                position = mo.MapPosition;
                layer = hull.Layer;
                tile = layer.GetTile((ushort)position.X, (ushort)position.Y);
                renderingObject.Visible = !sprite.Visible || tile.IsBlank;
            }
        }
    }
}