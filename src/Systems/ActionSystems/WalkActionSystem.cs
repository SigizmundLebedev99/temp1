using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;
using temp1.Data;
using temp1.UI;

namespace temp1.Systems
{
    class WalkActionSystem : EntityProcessingSystem
    {
        Mapper<WalkAction> _actionMapper;
        Mapper<OpenStorageAction> _storageActionMap;
        Mapper<BaseAction> _baseActionMapper;
        Mapper<MapObject> _moMap;
        Mapper<Direction> _dirMap;

        WorldContext _context;

        public WalkActionSystem(WorldContext context) : base(Aspect.All(typeof(WalkAction)))
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
                if (mapObj.MapPosition != action.To)
                    continue;
                else if((mapObj.Type & GameObjectType.Blocking) != 0)
                {
                    action.Abort();
                    break;
                }
            }
            _baseActionMapper.Put(entityId, action);
            _dirMap.Put(entityId, new Direction(action.To, action.From));
        }
    }
}