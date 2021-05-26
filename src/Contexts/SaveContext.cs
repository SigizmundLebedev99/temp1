using System.IO;
using DefaultEcs;
using DefaultEcs.Serialization;
using temp1.AI;
using temp1.Components;
using temp1.Models;
using System.Xml;
using System;

namespace temp1
{
    static class SaveContext
    {
        const string WorldsConfig = "worldconfig.xml";
        const string TempFile = "temp.xml";

        public static void Load(string saveFile, string mapName = null)
        {
            if (saveFile == null)
                throw new ArgumentNullException();

            var doc = LoadXml(saveFile);
            var root = doc.DocumentElement;
            mapName = mapName ?? root.GetAttribute("current_map");

            if (mapName == null)
                throw new ArgumentNullException("Map name wasn't provided");

            foreach (var map in root.ChildNodes)
            {
                
            }
        }

        public static void SaveWorld()
        {
            using var context = GetSetializationContext();

            var textSerializer = new TextSerializer(context);

            using (Stream stream = File.Create("save.txt"))
            {
                var entities = GameContext.EntitySets.Serializable.GetEntities().ToArray();
                textSerializer.Serialize(stream, entities);
            }
        }

        public static World LoadWorld()
        {
            using var context = GetSetializationContext();

            var textSerializer = new TextSerializer(context);

            using Stream stream = File.OpenRead("save.txt");

            var world = new World(64);
            using var _ = world.SubscribeComponentAdded<IGameAI>((in Entity entity, in IGameAI ai) =>
            {
                if (ai is PlayerControl)
                    GameContext.Player = entity;
            });

            textSerializer.Deserialize(stream, world);
            return world;
        }

        private static TextSerializationContext GetSetializationContext()
        {
            return new TextSerializationContext()
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
        }

        private static XmlDocument LoadXml(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            var document = new XmlDocument();
            document.Load(stream);
            return document;
        }
    }
}