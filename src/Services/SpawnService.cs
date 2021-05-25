using System;
using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Services
{
    class SpawnService
    {
        public void Spawn(string type, int x, int y)
        {
            var portal = GameContext.World.CreateEntity();
            var random = new Random();
            var grid = GameContext.Map.MovementGrid;
            if (!grid.IsWalkableAt(x, y))
                return;
            portal.Set(new Position(new Vector2(x, y) * 32 + new Vector2(16)));
            portal.Set(new RenderingObject(GameContext.Content.GetAnimatedSprite("images/portal.sf"), "images/portal.sf"));
            portal.Set<Expired>(new Timer(1.5f, () =>
            {
                GameContext.GameObjects.CreateMapObject("enemy", new Vector2(x, y) * 32 + new Vector2(16));
            }, true));
        }
    }
}