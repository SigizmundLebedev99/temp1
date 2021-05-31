using System.Drawing;
using MonoGame.Extended.Tiled;
using temp1.Components;

namespace temp1.Models
{
    struct Portal
    {
        private Rectangle Zone;
        private Point Transition;
        // if null, player stays on the map
        private string DestinationMap;

        public Portal(TiledMapObject from)
        {
            if(from.Properties.TryGetValue("destination", out var destination))
                DestinationMap = destination;
            if (int.TryParse(from.Properties["player_transition_x"], out int x) && int.TryParse(from.Properties["player_transition_y"], out int y))
                Transition = new Point(x, y);
        }

        public void Check(Position player)
        {
            if (int.TryParse(tiled.Properties["player_transition_x"], out int x) && int.TryParse(tiled.Properties["player_transition_y"], out int y))
                playerPosition = playerPosition + ((new Vector2(x, y) * 32));
            GameContext.Player.Get<Position>().Value = playerPosition;
            GameContext.LoadMap(toMap);
        }
    }
}