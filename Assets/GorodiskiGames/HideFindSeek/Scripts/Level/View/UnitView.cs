using System;
using Game.Unit;
using Game.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public sealed class UnitView : AnimationUnitView
    {
        public Action<UnitView> ON_CAUGHT;

        [SerializeField] private Transform _localTransform;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Collider _collider;

        public FieldOfView FieldOfView { get; internal set; }

        public void SetOutline(Material[] materials)
        {
            Material[] result = new Material[] { _meshRenderer.sharedMaterial };
            if (null != materials)
            {
                ArrayUtils.AddRange(ref result, materials.Length, materials);
            }
            _meshRenderer.materials = result;
        }

        public void ResetPath()
        {
            _navMeshAgent.ResetPath();
        }

        public void NavMeshStatus(bool value)
        {
            _navMeshAgent.enabled = value;
        }

        public void SetNavMesh(float walkSpeed, float angularSpeed, float acceleration)
        {
            _navMeshAgent.speed = walkSpeed;
            _navMeshAgent.angularSpeed = angularSpeed;
            _navMeshAgent.acceleration = acceleration;
        }

        public void SetCollider(bool value)
        {
            _collider.enabled = value;
        }

        public void SetLayer(string layerLabel)
        {
            gameObject.layer = LayerMask.NameToLayer(layerLabel);
        }

        public void SetLabel(string name)
        {
            gameObject.name = name;
        }

        public void FireCaught()
        {
            ON_CAUGHT?.Invoke(this);
        }

        public void SetDestination(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }

        public void SetScale(float scale)
        {
            _localTransform.localScale = Vector3.one * scale;
        }

        public void Visibility(bool value)
        {
            _meshRenderer.gameObject.SetActive(value);
        }
    }
}