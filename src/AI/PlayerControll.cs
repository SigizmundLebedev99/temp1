using System;
using System.Linq;
using EpPathFinding.cs;
using FluentBehaviourTree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input;
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
        float _time = 0;
        int targetId = 0;

        public PlayerControll(int entityId, GameContext context) : base(entityId, context)
        {
            var cm = context.World.ComponentManager;
            _dotMapper = cm.Get<Dot>();
            _moveMapper = cm.Get<IMovement>();
            _storageMap = cm.Get<Storage>();
            _enemyMap = cm.Get<Enemy>();
            _jpParam = new JumpPointParam(context.Grid, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Never);
            MouseStateExtended state = MouseExtended.GetState();

            _tree = new BehaviourTreeBuilder()
                .Sequence("Start")
                    .Do("await", t => {
                        state = MouseExtended.GetState();
                        if(state.LeftButton == ButtonState.Pressed)
                            return BehaviourTreeStatus.Success;
                        return BehaviourTreeStatus.Running;
                    })
                    .Sequence("Possible Actions")
                        .Do("justMove", t => {
                            if(Context.PointedId == -1){
                                CommitMovement(state);
                                return BehaviourTreeStatus.Failure;
                            }
                            return BehaviourTreeStatus.Success;
                        })
                        .Inverter("i1")
                            .Sequence("Enemy")
                                .Do("is enemy", t => {
                                    if(_enemyMap.Has(Context.PointedId))
                                        return BehaviourTreeStatus.Success;
                                    return BehaviourTreeStatus.Failure;
                                })
                            .End()
                        .End()
                        .Sequence("Store")
                            .Do("is store", t => {
                                if(_storageMap.Has(Context.PointedId))
                                    return BehaviourTreeStatus.Success;
                                return BehaviourTreeStatus.Failure;
                            })
                        .End()
                    .End()
                .End().Build();
        }

        void CommitMovement(MouseStateExtended state){
            var pointOnMap = Context.Camera.ScreenToWorld(state.X, state.Y);
            var to = (pointOnMap / 32).ToPoint();
            if (!Context.Grid.Contains(to) || !Context.Grid.IsWalkableAt(to.X, to.Y))
                return;
            var from = _dotMapper.Get(Context.PlayerId).MapPosition;
            _jpParam.Reset(new GridPos(from.X, from.Y), new GridPos(to.X, to.Y));
            var result = JumpPointFinder.FindPath(_jpParam);
            if (result.Count < 2)
                return;
            var movement = new PolylineMovement(
                    result.Select(e => new Vector2(e.x * 32 + 16, e.y * 32 + 16)).ToArray(),
                3f);
            _moveMapper.Put(Context.PlayerId, movement);
        }

        public override void Update(GameTime time)
        {
            _tree.Tick(new TimeData());
        }
    }
}