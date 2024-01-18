using UnityEngine;

namespace Util
{
    public class TargetFrameRate : MonoBehaviour
    {
        public static TargetFrameRate Instance;

        [SerializeField] int _targetFrameRate = 30;

        void Awake()
        {
            Instance = this;

            SetFrameRate(_targetFrameRate);
        }

        public void SetFrameRate(int frameRate)
        {
            Application.targetFrameRate = frameRate;
        }
    }
}
