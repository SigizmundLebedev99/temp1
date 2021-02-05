using Microsoft.Xna.Framework;

namespace temp1.Components
{
    public enum MapObjectFlag{
        None,
        Storage,
        Enemy,
    }

    class MapObject
    {
        public MapObjectFlag Flag;
        public Vector2 Position;
        public Point MapPosition => (Position / 32).ToPoint();
        
        public MapObject(){}

        public MapObject(Vector2 position, MapObjectFlag flag){
            Position = position;
            Flag = flag;
        }
    }
}