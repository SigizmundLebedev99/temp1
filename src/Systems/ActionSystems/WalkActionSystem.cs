using System.Collections.Generic;
using EpPathFinding.cs;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using temp1.Components;

namespace temp1.Systems
{
    class WalkActionSystem : BaseActionSystem
    {
        Mapper<WalkAction> _actionMapper;
        Mapper<IMovement> _moveMapper;
        Mapper<PossibleMoves> _possibleMovesMap;
        Mapper<MapObject> _moMap;
        Mapper<Direction> _directionMap;
        JumpPointParam _jpParam;

        public WalkActionSystem(GameContext context) : base(Aspect.All(typeof(WalkAction)), context)
        {
            _jpParam = new JumpPointParam(context.MovementGrid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            base.Initialize(mapperService);
            _actionMapper = mapperService.Get<WalkAction>();
            _moveMapper = mapperService.Get<IMovement>();
            _possibleMovesMap = mapperService.Get<PossibleMoves>();
            _moMap = mapperService.Get<MapObject>();
            _directionMap = mapperService.Get<Direction>();
        }

        public override void Update(GameTime gameTime)
        {
            for (var i = 0; i < ActiveEntities.Count; i++)
            {
                var action = _actionMapper.Get(ActiveEntities[i]);
                if (action.Status == ActionStatus.Started)
                    SetMovement(ActiveEntities[i], action);
                else if (action.Status != ActionStatus.Running)
                    Complete(action, ActiveEntities[i]);
                action.Update(gameTime);
            }
        }

        void SetMovement(int entityId, WalkAction action)
        {
            var actionPoints = _pointsMapper.Get(entityId);
            var path = FindPath(entityId, action.TargetId, action.To);
            var from = _moMap.Get(entityId).Position;
            _directionMap.Put(entityId, new Direction(from));
            if(path.Count - 1 > actionPoints.Remain)
                action.Abort();
            else
                action.SetInfo(path.Count - 1, new PolylineMovement(path, 3f));
        }

        List<GridPos> FindPath(int entityId, int targetId, Point to)
        {
            var from = _moMap.Get(entityId).MapPosition;
            if(targetId != -1)
                to = _moMap.Get(targetId).MapPosition;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y), context.MovementGrid);
            var path = JumpPointFinder.FindPath(_jpParam);
            if (targetId != -1 && path.Count > 2)
                path.RemoveAt(path.Count - 1);
            return path;
        }
    }
}