
using MonoGame.Extended.Tiled;

namespace temp1.Components
{
    class Canopy
    {
        public TiledMapTileLayer Layer;
        
        public Canopy(TiledMapTileLayer layer)
        {
            Layer = layer;
        }

        public bool IsInterier { get; internal set; }
    }
}