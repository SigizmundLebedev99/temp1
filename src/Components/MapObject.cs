using Microsoft.Xna.Framework;
using temp1.Data;

namespace temp1.Components
{
    class MapObject
    {
        public GameObjectType type;
        public Vector2 position;
        public Point MapPosition => (position / 32).ToPoint();
        
        public MapObject(){}

        public MapObject(Vector2 position, GameObjectType flag){
            this.position = position;
            type = flag;
        }
    }
}