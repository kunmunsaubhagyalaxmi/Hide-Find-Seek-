﻿using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game.Unit
{
	//
	// Initially inspired by Sebastian Lague tutorial: https://www.youtube.com/watch?v=rQG9aUWarwE
	//
	public sealed class FieldOfView : MonoBehaviour
	{
		private const float _scanRate = 0.1f;

		public LayerMask TargetMask;
		public LayerMask ObstacleMask;

		[HideInInspector]
        public List<Transform> Targets = new List<Transform>();
		public Transform Target;

		public float MeshResolution = 0.2f;
		public int EdgeResolveIterations = 3;
		public float EdgeDstThreshold = 0.5f;

		public MeshFilter MeshFilter;
		private Mesh _viewMesh;

		private float _scanTime;
        private float _radius;
		private float _angle;

        public void Initialize(float radius, float angle)
        {
			_radius = radius;
			_angle = angle;

			_viewMesh = new Mesh();
			MeshFilter.mesh = _viewMesh;

			enabled = true;
        }

		private void LateUpdate()
		{
			if (Time.time > _scanTime)
			{
				_scanTime = Time.time + _scanRate;
				FindVisibleTargets();
			}

			DrawFieldOfView();
		}

		private void FindVisibleTargets()
		{
			Targets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _radius, TargetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
			{
				Transform target = targetsInViewRadius[i].transform;
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				if (Vector3.Angle(transform.forward, dirToTarget) < _angle / 2)
				{
					float dstToTarget = Vector3.Distance(transform.position, target.position);
					if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
					{
						Targets.Add(target);
					}
				}
			}

			Target = null;
            var minDistance = float.MaxValue;
            foreach (var target in Targets)
            {
                var distance = Vector3.Distance(target.position, transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
					Target = target;
                }
            }
		}

		private void DrawFieldOfView()
		{
			int stepCount = Mathf.RoundToInt(_angle * MeshResolution);
			float stepAngleSize = _angle / stepCount;
			List<Vector3> viewPoints = new List<Vector3>();
			ViewCastInfo oldViewCast = new ViewCastInfo();
			for (int i = 0; i <= stepCount; i++)
			{
				float angle = transform.eulerAngles.y - _angle / 2 + stepAngleSize * i;
				ViewCastInfo newViewCast = ViewCast(angle);

				if (i > 0)
				{
					bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > EdgeDstThreshold;
					if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
					{
						EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
						if (edge.pointA != Vector3.zero)
						{
							viewPoints.Add(edge.pointA);
						}
						if (edge.pointB != Vector3.zero)
						{
							viewPoints.Add(edge.pointB);
						}
					}
				}

				viewPoints.Add(newViewCast.point);
				oldViewCast = newViewCast;
			}

			int vertexCount = viewPoints.Count + 1;
			Vector3[] vertices = new Vector3[vertexCount];
			int[] triangles = new int[(vertexCount - 2) * 3];

			vertices[0] = Vector3.zero;
			for (int i = 0; i < vertexCount - 1; i++)
			{
				vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

				if (i < vertexCount - 2)
				{
					triangles[i * 3] = 0;
					triangles[i * 3 + 1] = i + 1;
					triangles[i * 3 + 2] = i + 2;
				}
			}

			_viewMesh.Clear();

			_viewMesh.vertices = vertices;
			_viewMesh.triangles = triangles;
			_viewMesh.RecalculateNormals();
		}


		private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
		{
			float minAngle = minViewCast.angle;
			float maxAngle = maxViewCast.angle;
			Vector3 minPoint = Vector3.zero;
			Vector3 maxPoint = Vector3.zero;

			for (int i = 0; i < EdgeResolveIterations; i++)
			{
				float angle = (minAngle + maxAngle) / 2;
				ViewCastInfo newViewCast = ViewCast(angle);

				bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > EdgeDstThreshold;
				if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
				{
					minAngle = angle;
					minPoint = newViewCast.point;
				}
				else
				{
					maxAngle = angle;
					maxPoint = newViewCast.point;
				}
			}

			return new EdgeInfo(minPoint, maxPoint);
		}


		private ViewCastInfo ViewCast(float globalAngle)
		{
			Vector3 dir = DirFromAngle(globalAngle, true);
			RaycastHit hit;

			if (Physics.Raycast(transform.position, dir, out hit, _radius, ObstacleMask))
				return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
			else
				return new ViewCastInfo(false, transform.position + dir * _radius, _radius, globalAngle);
		}

		public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
				angleInDegrees += transform.eulerAngles.y;

			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
		}

		public struct ViewCastInfo
		{
			public bool hit;
			public Vector3 point;
			public float dst;
			public float angle;

			public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
			{
				hit = _hit;
				point = _point;
				dst = _dst;
				angle = _angle;
			}
		}

		public struct EdgeInfo
		{
			public Vector3 pointA;
			public Vector3 pointB;

			public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
			{
				pointA = _pointA;
				pointB = _pointB;
			}
		}
	}
}


