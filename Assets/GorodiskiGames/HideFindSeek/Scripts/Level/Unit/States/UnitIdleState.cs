using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Game.Unit.States
{
    public sealed class UnitIdleSeekerState : UnitIdleState
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void OnTick()
        {
            base.OnTick();

            LookForUnit(_unit.View);
        }

        public override void Walk(Vector3 destination)
        {
            _unit.WalkSeeker(destination);
        }
    }

    public class UnitIdleState : UnitState
    {
        private const float _destinationDistance = 3f;
        private const float _radiusMin = 5f;
        private const float _radiusMax = 10f;

        private float _walkTime;

        public override void Initialize()
        {
            _unit.View.ResetPath();
            _unit.View.Idle();

            _walkTime = Time.time + _unit.IdleDuration;

            _timer.TICK += OnTick;
        }

        public override void Dispose()
        {
            _timer.TICK -= OnTick;
        }

        public virtual void OnTick()
        {
            if(Time.time < _walkTime)
                return;

            var position = _unit.View.Position;
            var randomPoint = GetRandomPosition();
            var requestedDestination = position + new Vector3(randomPoint.x, 0f, randomPoint.y);

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(requestedDestination, out hit, _radiusMax, NavMesh.AllAreas))
                return;

            var destination = hit.position;
            var distance = Vector3.Distance(position, destination);
            if(distance < _destinationDistance)
                return;

            Walk(destination);
        }

        private Vector2 GetRandomPosition()
        {
            float deg = Random.Range(0f, 360f) * MathUtil.OneOrMinus();
            float angle = deg * Mathf.Deg2Rad;
            float radian = 2f * Mathf.PI + angle;

            float xScaled = Mathf.Cos(radian);
            float yScale = Mathf.Sin(radian);

            float x = xScaled * Random.Range(_radiusMin, _radiusMax);
            float y = yScale * Random.Range(_radiusMin, _radiusMax);

            return new Vector2(x, y);
        }

        public virtual void Walk(Vector3 destination)
        {
            _unit.Walk(destination);
        }
    }
}