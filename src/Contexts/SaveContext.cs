using System.IO;
using DefaultEcs;
using DefaultEcs.Serialization;
using temp1.AI;
using temp1.Components;
using temp1.Models;
using System.Xml;
using temp1.Models.Serialization;
using System.Text;
using System.Linq;

namespace temp1
{
    static class SaveContext
    {
        const string TempFile = "temp.xml";

        public static void LoadGame(string saveFile)
        {
            if (File.Exists(TempFile))
                File.Delete(TempFile);
            File.Copy(saveFile, TempFile);
            LoadMap();
        }

        public static void LoadMap(string mapName = null)
        {
            var gameDataXml = LoadXml(TempFile);

            mapName = mapName ?? gameDataXml.DocumentElement.GetAttribute("current_map");

            var world = new World(64);

            foreach (XmlElement element in gameDataXml.GetElementsByTagName("map"))
            {
                if (element.Attributes.Count == 0 || !element.HasAttribute("name"))
                    continue;
                if (element.GetAttribute("name") != mapName)
                    continue;

                string worldInfo = element.InnerText;

                using var sContext = GetSetializationContext();
                var serializar = new TextSerializer(sContext);

                var worldInfoBytes = Encoding.UTF8.GetBytes(worldInfo);
                using var stream = new MemoryStream(worldInfoBytes);

                serializar.Deserialize(stream, world);
                break;
            }

            GameContext.LoadMap(mapName, world);
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
            .Marshal<GameObjectTypeInfo, GameObjectTypeName>(from => new GameObjectTypeName { Value = from.TypeName })
            .Unmarshal<GameObjectTypeName, GameObjectTypeInfo>(from => GameContext.GameObjects.GetGameObjectTypeInfo(from.Value))
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