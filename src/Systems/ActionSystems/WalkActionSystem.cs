using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using temp1.Components;

namespace temp1.Systems
{
    class WalkActionSystem : EntityProcessingSystem
    {
        Mapper<WalkAction> _actionMapper;
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
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var action = _actionMapper.Get(entityId);
            _actionMapper.Delete(entityId);
            _baseActionMapper.Put(entityId, action);
            _dirMap.Put(entityId, new Direction(action.to, action.from));
        }
    }
}