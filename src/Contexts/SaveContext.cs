using System.IO;
using DefaultEcs;
using DefaultEcs.Serialization;
using temp1.AI;
using temp1.Components;
using temp1.Models;
using System.Xml;
using temp1.Models.Serialization;
using System.Text;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace temp1
{
    static class SaveContext
    {
        const string TempFile = "tempState.xml";
        const string savesFolder = "saves/";

        public static void LoadGame(string saveFile)
        {
            if (File.Exists(TempFile))
                File.Delete(TempFile);
            File.Copy(saveFile, TempFile);
            LoadMap();
        }

        public static void LoadGame(FileInfo saveFile)
        {
            if (File.Exists(TempFile))
                File.Delete(TempFile);
            saveFile.CopyTo(TempFile);
            LoadMap();
        }

        public static void SaveGame(string saveName)
        {
            var gameDataXml = LoadXml();
            SaveMapState(gameDataXml);
            File.Copy(TempFile, savesFolder + saveName + ".xml");
        }

        public static IEnumerable<FileInfo> GetSaves()
        {
            var dir = new DirectoryInfo(savesFolder);
            if (!dir.Exists)
                dir.Create();
            return dir.GetFiles();
        }

        public static void SwitchMap(string newMap)
        {
            var gameDataXml = LoadXml();
            SaveMapState(gameDataXml);
            LoadMap(newMap, gameDataXml);
            gameDataXml.Save(TempFile);
        }

        private static void LoadMap(string mapName = null, XmlDocument gameDataXml = null)
        {
            gameDataXml = gameDataXml ?? LoadXml();

            mapName = mapName ?? gameDataXml.DocumentElement.GetAttribute("current_map");

            var world = new World(64);

            var element = GetMapState(gameDataXml, mapName);

            if (element == null)
            {
                GameContext.LoadMap(mapName);
                return;
            }

            string worldInfo = element.InnerText;

            using var sContext = GetSetializationContext();
            var serializar = new TextSerializer(sContext);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(worldInfo.Length);

            var bytesLength = Encoding.UTF8.GetBytes((ReadOnlySpan<char>)worldInfo, buffer);

            using var stream = new MemoryStream(buffer, 0, bytesLength);

            serializar.Deserialize(stream, world);

            ArrayPool<byte>.Shared.Return(buffer);

            GameContext.LoadMap(mapName, world);
        }

        public static void SaveMapState(XmlDocument gameDataXml)
        {
            using var context = GetSetializationContext();

            var textSerializer = new TextSerializer(context);

            var entities = GameContext.EntitySets.Serializable.GetEntities().ToArray();

            using var stream = new MemoryStream();

            textSerializer.Serialize(stream, entities);

            var element = GetMapState(gameDataXml, GameContext.Map.MapName);

            element.InnerText = Encoding.UTF8.GetString(stream.ToArray());
        }

        private static TextSerializationContext GetSetializationContext()
        {
            return new TextSerializationContext()
            .Marshal<BaseAction, string>(_ => null)
            .Marshal<IGameAI, AIFactory>(ai => ai.GetFactory())
            .Marshal<RenderingObject, RenderObjectInfo>(from => new RenderObjectInfo()
            {
                Path = from.ResourceName,
                Region = from.Bounds,
                Origin = new Origin { X = from.Origin.X, Y = from.Origin.Y }
            })
            .Marshal<GameObjectTypeInfo, GameObjectTypeName>(from => new GameObjectTypeName { Value = from.TypeName })
            .Unmarshal<GameObjectTypeName, GameObjectTypeInfo>(from => GameContext.GameObjects.GetGameObjectTypeInfo(from.Value))
            .Unmarshal<AIFactory, IGameAI>(factory => factory.Get())
            .Unmarshal<RenderObjectInfo, RenderingObject>(render => new RenderingObject(Content.GetSprite(render), render.Path));
        }

        private static XmlElement GetMapState(XmlDocument document, string mapName)
        {
            foreach (XmlElement element in document.GetElementsByTagName("map"))
            {
                if (element.Attributes.Count == 0 || !element.HasAttribute("name"))
                    continue;
                if (element.GetAttribute("name") == mapName)
                    return element;
            }
            var newElement = document.CreateElement("map");
            newElement.SetAttribute("name", mapName);
            document.AppendChild(newElement);
            return newElement;
        }

        private static XmlDocument LoadXml()
        {
            using var stream = File.OpenRead(TempFile);
            var document = new XmlDocument();
            document.Load(stream);
            return document;
        }
    }
}