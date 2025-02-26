using System;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;

namespace Game.Managers
{
    public sealed class iOSAuthorizationTrackingManager : IDisposable
    {
        public void Initialize()
        {
#if UNITY_IOS
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                RequestAuthorizationTracking();
            }
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
        }

        public void Dispose()
        {
        }

        private void RequestAuthorizationTracking()
        {
#if UNITY_IOS
            Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");

            ATTrackingStatusBinding.RequestAuthorizationTracking();
#else
            Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
#endif
        }
    }
}
