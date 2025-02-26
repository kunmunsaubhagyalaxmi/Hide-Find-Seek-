using UnityEngine;

namespace Game.Managers
{
    public sealed class ResourcesManager
    {
        private static string _unitMarkerPrefabPath = "Prefabs/UnitMarker";
        private static string _fieldOfViewPrefabPath = "Prefabs/FieldOfView";

        public GameObject LoadUnitMarker()
        {
            return Resources.Load<GameObject>(_unitMarkerPrefabPath);
        }

        public GameObject LoadFieldOfView()
        {
            return Resources.Load<GameObject>(_fieldOfViewPrefabPath);
        }

        public void ReleaseAsset(Object asset)
        {
            Resources.UnloadAsset(asset);
        }

        public void Dispose()
        {
            
        }
    }
}