using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;
using temp1.Data;

namespace temp1.Systems
{
    class WalkActionSystem : EntityProcessingSystem
    {
        Mapper<WalkAction> _actionMapper;
        Mapper<OpenStorageAction> _storageActionMap;
        Mapper<BaseAction> _baseActionMapper;
        Mapper<MapObject> _moMap;
        Mapper<Direction> _dirMap;
        GameContext _context;

        public WalkActionSystem(GameContext context) : base(Aspect.All(typeof(WalkAction)))
        {
            _context = context;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _actionMapper = mapperService.Get<WalkAction>();
            _moMap = mapperService.Get<MapObject>();
            _baseActionMapper = mapperService.Get<BaseAction>();
            _dirMap = mapperService.Get<Direction>();
            _storageActionMap = mapperService.Get<OpenStorageAction>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var action = _actionMapper.Get(entityId);
            _actionMapper.Delete(entityId);
            var mapObjects = _context.MapObjects;
            for (var i = 0; i < mapObjects.Count; i++)
            {
                var mapObj = _moMap.Get(mapObjects[i]);
                if (mapObj.MapPosition != action.to)
                    continue;
                switch (mapObj.Type)
                {
                    case GameObjectType.Storage : 
                        _storageActionMap.Put(entityId, new OpenStorageAction(mapObjects[i], _context));
                        return;
                }
            }
            _baseActionMapper.Put(entityId, action);
            _dirMap.Put(entityId, new Direction(action.to, action.from));
        }
    }
}