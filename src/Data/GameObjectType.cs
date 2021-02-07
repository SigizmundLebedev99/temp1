using Microsoft.Xna.Framework;
using temp1.Components;

namespace temp1.Data
{

    public enum GameObjectType{
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

    class Origin{
        public int x;
        public int y;
    }

    class Region{
        public int x;
        public int y;
        public int width;
        public int height;

        public Rectangle Rectangle => new Rectangle(x,y,width, height);
    }

    class GameObjectTypeInfo
    {
        public string typeName;
        public GameObjectType type;
        public string path;
        public Origin origin;
        public Region region;
        public string handler;
        public AIParams ai;
        public int stackSize;
        public int flags;
    }
}

