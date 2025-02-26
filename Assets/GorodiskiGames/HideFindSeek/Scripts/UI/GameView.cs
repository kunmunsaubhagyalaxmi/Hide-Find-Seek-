using System.Collections.Generic;
using Game.UI.Hud;
using Game.UI.Pool;
using UnityEngine;
using UnityEngine.UI;
using Game.Controls;

namespace Game
{
    public sealed class GameView : MonoBehaviour
    {
        private const float _ratioPortraitA = 0.5625f; //1080*1920
        private const float _ratioPortraitB = 0.4615f; //886*1920
        private const float _ratioLanscape = 1.77777f; //1920*1080

        public ComponentPoolFactory CagesPool;
        public ComponentPoolFactory SparksEffectPool;

        public Material[] OutlineMaterials;

        public BaseHud[] Huds;
        public Canvas Canvas;
        public Joystick Joystick;
        public CameraController CameraController;
        [HideInInspector] public RenderTexture RenderTexture;
        private AspectRatioFitter[] _ratioFitters;
        public GameObject GameplayLight;
        public GameObject HudLight;

        public UnitView PlayerView;

        public IEnumerable<IHud> AllHuds()
        {
            foreach (var hud in Huds)
            {
                yield return hud;
            }
        }

        private void Awake()
        {
            //SetAspectRatioFitters(); //used for video recording
        }

        public void SetAspectRatioFitters()
        {
            bool isDeveloperIPad = GameConstants.IsDebugIPad();
            if (isDeveloperIPad)
            {
                Screen.orientation = ScreenOrientation.AutoRotation;

                var mode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                var ratio = _ratioLanscape;

                if (Screen.height > Screen.width)
                {
                    mode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                    ratio = _ratioPortraitA;
                }

                foreach (var aspect in _ratioFitters)
                {
                    aspect.aspectMode = mode;
                    aspect.aspectRatio = ratio;
                }
            }
            else
            {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }
    }
}

