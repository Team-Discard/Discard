using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CutSceneSystem
{
    public class TimelineManager : MonoBehaviour
    {
        public static TimelineManager Instance;
        
        public static bool IsCutScenePlaying { get; private set; } = false;

        [SerializeField] private Image blackScreen;
        [SerializeField] private float blackScreenFadeTime;
        [SerializeField] private GameObject manikin;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void StaticInit()
        {
            IsCutScenePlaying = false;
        }

        public void ToggleIsCutScenePlaying(bool toggle)
        {
            IsCutScenePlaying = toggle;
        }

        public void DestroyManikin()
        {
            Destroy(manikin);
            manikin = null;
        }

        public void FadeInBlackScreen()
        {
            blackScreen.DOFade(1f, blackScreenFadeTime).OnStart(()=>{blackScreen.gameObject.SetActive(true);});
        }

        public void FadeOutBlackScreen()
        {
            blackScreen.DOFade(0f, blackScreenFadeTime).OnComplete(()=>{blackScreen.gameObject.SetActive(false);});
        }
    }
}
