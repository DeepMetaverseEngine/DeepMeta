using UnityEngine;

namespace NFCore.Extension
{
    public class HudRun : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            HudRendererCenter.Update();
        }

        void LateUpdate()
        {
            HudRendererCenter.LateUpdate();
        }

        private void OnDestroy()
        {
            HudRendererCenter.Destory();
        }
    }
}