using Microsoft.Xna.Framework;
using temp1.Data;

namespace temp1.Components
{
    class MapObject
    {
        public GameObjectType Flag;
        public Vector2 Position;
        public Point MapPosition => (Position / 32).ToPoint();
        
        public MapObject(){}

        public MapObject(Vector2 position, GameObjectType flag){
            Position = position;
            Flag = flag;
        }
    }
}