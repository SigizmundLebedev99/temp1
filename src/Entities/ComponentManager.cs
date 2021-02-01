using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Entities.Systems;

namespace MonoGame.Extended.Entities
{
    public interface IComponentMapperService
    {
        Mapper<T> Get<T>() where T : class;
    }

    public class ComponentManager : UpdateSystem, IComponentMapperService
    {
        public ComponentManager()
        {
            _componentMappers = new Bag<ComponentMapper>();
            _componentTypes = new Dictionary<Type, int>();
        }

        private readonly Bag<ComponentMapper> _componentMappers;
        private readonly Dictionary<Type, int> _componentTypes;

        public Action<int> ComponentsChanged;

        private Mapper<T> CreateMapperForType<T>(int componentTypeId)
            where T : class
        {
            if (componentTypeId >= 64)
                throw new InvalidOperationException("Component type limit exceeded. We currently only allow 64 component types for performance reasons.");

            var mapper = new Mapper<T>(componentTypeId, ComponentsChanged);
            _componentMappers[componentTypeId] = mapper;
            return mapper;
        }

        public ComponentMapper GetMapper(int componentTypeId)
        {
            return _componentMappers[componentTypeId];
        }

        public Mapper<T> Get<T>() 
            where T : class
        {
            var componentTypeId = GetComponentTypeId(typeof(T));

            if (_componentMappers[componentTypeId] != null)
                return _componentMappers[componentTypeId] as Mapper<T>;

            return CreateMapperForType<T>(componentTypeId);
        }

        public int GetComponentTypeId(Type type)
        {
            if (_componentTypes.TryGetValue(type, out var id))
                return id;

            id = _componentTypes.Count;
            _componentTypes.Add(type, id);
            return id;
        }

        public long CreateComponentBits(int entityId)
        {
            var componentBits = 0;
            for (var componentId = 0; componentId < _componentMappers.Count; componentId++)
            {
                componentBits |= (_componentMappers[componentId] == null ? 0 : _componentMappers[componentId].Has(entityId) ? 1 : 0) << componentId;
            }

            return componentBits;
        }

        public void Destroy(int entityId)
        {
            foreach (var componentMapper in _componentMappers)
                if(componentMapper != null)
                    componentMapper.Delete(entityId);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}