using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Data
{
    enum GameObjectType
    {
        None,
        Storage,
        Enemy,
        Item
    }

    class AIParams
    {
        public string type;
        public int targetMapId;
        public float speed;
    }

    class Origin
    {
        public int x;
        public int y;
    }

    class Region
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public Region(){}

        public Region(int x, int y, int width, int height){
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }

        public static implicit operator Rectangle(Region r)
        {
            return new Rectangle(r.x, r.y, r.width, r.height);
        }

        public static implicit operator Region(Rectangle r)
        {
            return new Region(r.X, r.Y, r.Width, r.Height);
        }
    }

    class GameObjectTypeInfo
    {
        public string typeName;
        public GameObjectType type;
        public string path;
        public Origin origin;
        public Region region;
        public string handler;
        public int stackSize;
        public int flags;
    }
}

