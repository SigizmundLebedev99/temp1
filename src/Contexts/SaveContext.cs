using System.IO;
using DefaultEcs.Serialization;
using temp1.AI;
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
                Path = from.ResourceName,
                Region = from.Bounds,
                Origin = new Origin { X = from.Origin.X, Y = from.Origin.Y }
            })
            .Marshal<BaseAction, string>(_ => null)
            .Marshal<IGameAI, AIFactory>(ai => ai.GetFactory())
            .Unmarshal<AIFactory, IGameAI>(factory => factory.Get())
            .Unmarshal<RenderObjectInfo, RenderingObject>(from => new RenderingObject(GameContext.Content.GetSprite(from), from.Path));

            var textSerializer = new TextSerializer(context);

            using (Stream stream = File.Create("save.txt"))
            {
                var entities = GameContext.EntitySets.Serializable.GetEntities().ToArray();
                textSerializer.Serialize(stream, entities);
            }
        }
    }
}