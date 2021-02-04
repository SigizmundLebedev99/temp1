using System.Collections.Generic;
using System.Linq;
using EpPathFinding.cs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
using MonoGame.Extended.Sprites;
using temp1.Components;

namespace temp1.AI
{
    class PlayerControll : BaseAI
    {
        Mapper<IMovement> _moveMapper;
        Mapper<Storage> _storageMap;
        Mapper<Enemy> _enemyMap;
        Mapper<Dot> _dotMapper;

        JumpPointParam _jpParam;
        IBehaviourTreeNode _tree;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dotMapper = cm.Get<Dot>();
            _moveMapper = cm.Get<IMovement>();
            _storageMap = cm.Get<Storage>();
            _enemyMap = cm.Get<Enemy>();
            var aSpriteMap = cm.Get<AnimatedSprite>();

            int targetId = -1;

            _jpParam = new JumpPointParam(context.Grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
            MouseStateExtended state = MouseExtended.GetState();

            _tree = new BehaviourTreeBuilder()
                .Sequence("Start")
                    .Inverter("i1")
                        .Sequence("AwaitMove")
                            .Do("has no target", t =>
                            {
                                if (targetId == -1)
                                    return BehaviourTreeStatus.Failure;
                                return BehaviourTreeStatus.Success;
                            })
                            .Do("await move", t =>
                            {
                                var move = _moveMapper.Get(EntityId);
                                if (move == null || move.IsCompleted)
                                    return BehaviourTreeStatus.Success;
                                return BehaviourTreeStatus.Failure;
                            })
                            .Do("performAction", t =>
                            {
                                if (!_storageMap.Has(targetId) || !aSpriteMap.Has(targetId))
                                    return BehaviourTreeStatus.Failure;
                                var sprite = aSpriteMap.Get(targetId);
                                sprite.Play("open");
                                targetId = -1;
                                Context.IsInventoryOpen = true;
                                return BehaviourTreeStatus.Success;
                            })
                        .End()
                    .End()
                    .Do("Await input", t =>
                    {
                        state = MouseExtended.GetState();
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            targetId = -1;
                            return BehaviourTreeStatus.Success;
                        }
                        return BehaviourTreeStatus.Failure;
                    })
                    .Selector("Possible Actions")
                        .Do("just move", t =>
                        {
                            if (Context.PointedId != -1)
                                return BehaviourTreeStatus.Failure;
                            var pointOnMap = Context.Camera.ScreenToWorld(state.X, state.Y);
                            var to = (pointOnMap / 32).ToPoint();
                            CommitMovement(to);
                            return BehaviourTreeStatus.Success;
                        })
                        .Do("move to target", t =>
                        {
                            targetId = Context.PointedId;
                            var pos = (_dotMapper.Get(Context.PointedId).Position / 32).ToPoint();
                            var node = Context.Grid.GetNodeAt(pos.X, pos.Y);
                            var near = Context.Grid.GetNeighbors(node, DiagonalMovement.Never);
                            if (near.Count == 0)
                                return BehaviourTreeStatus.Failure;
                            CommitMovement(GetBestPath(near));
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End().Build();
        }

        void CommitMovement(Point to)
        {

            if (!Context.Grid.Contains(to) || !Context.Grid.IsWalkableAt(to.X, to.Y))
                return;
            var from = _dotMapper.Get(EntityId).MapPosition;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y), Context.Grid);
            var result = JumpPointFinder.FindPath(_jpParam);
            if (result.Count < 2)
                return;
            var movement = new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);
            _moveMapper.Put(EntityId, movement);
        }

        void CommitMovement(List<GridPos> path)
        {
            if (path.Count < 2)
                return;
            var movement = new PolylineMovement(
                    path.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);
            _moveMapper.Put(EntityId, movement);
        }

        List<GridPos> GetBestPath(List<Node> to)
        {
            if (to.Count == 1)
                return new List<GridPos>{
                    new GridPos(to[0].x, to[0].y)
                };
            var dot = _dotMapper.Get(EntityId).MapPosition;
            var from = new GridPos(dot.X, dot.Y);
            List<GridPos> min = null;
            int minDist = int.MaxValue;
            for (var i = 0; i < to.Count; i++)
            {
                _jpParam.Reset(from, new GridPos(to[i].x, to[i].y), Context.Grid);
                var path = JumpPointFinder.FindPath(_jpParam);
                if (path.Count < minDist)
                {
                    minDist = path.Count;
                    min = path;
                }
            }
            return min;
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}