// Copyright (c) Meta Platforms, Inc. and affiliates.

using UnityEngine;

namespace CrypticCabinet.Puzzles.Clock
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        private void Update()
        {
            UpdatePos();
        }

        private void UpdatePos()
        {
            var thisTransform = transform;
            thisTransform.position = m_target.position;
            thisTransform.rotation = m_target.rotation;
        }
    }
}
