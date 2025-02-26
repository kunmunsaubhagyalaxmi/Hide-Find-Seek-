using System.Collections.Generic;
using Game.Cash;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    public sealed class LevelView : MonoBehaviour
    {
        [HideInInspector] public CashView[] Cashes;
        [HideInInspector] public List<UnitView> Units;

        private void Awake()
        {
            var buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (buildIndex != 0 && SceneManager.sceneCount <= 1)
            {
                SceneManager.LoadScene(0);
                return;
            }

            var units = GetComponentsInChildren<UnitView>();
            foreach (var unit in units)
            {
                Units.Add(unit);
            }

            Cashes = GetComponentsInChildren<CashView>();
        }
    }
}