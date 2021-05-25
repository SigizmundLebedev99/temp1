using System.IO;
using DefaultEcs.Serialization;
using temp1.Components;
using temp1.Models;

namespace temp1
{
    class SaveContext
    {
        public void Save()
        {
            using var context = new TextSerializationContext()
            .Marshal<RenderingObject, RenderObjectInfo>(from => new RenderObjectInfo()
            {
                Path = from.Texture.Name,
                Region = from.Bounds,
                Origin = from.Origin
            })
            .Unmarshal<RenderObjectInfo, RenderingObject>(from => new RenderingObject(GameContext.Content.GetSprite(from)));
            
            var textSerializer = new TextSerializer(context);

            using (Stream stream = File.Create("save.txt"))
            {
                textSerializer.Serialize(stream, GameContext.EntitySets.Serializable.GetEntities().ToArray());
            }
        }
    }
}