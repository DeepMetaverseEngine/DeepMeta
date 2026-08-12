using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NFCore.Extension
{
    public class HudTextExample : MonoBehaviour
    {
        public GameObject hudgo;
        public int count = 0;
        public Transform cameraTrans;
        public Slider slider;

        private void Awake()
        {
            string[] texts = new string[] { "锦瑟无端五十弦,一弦一柱思华年", "大江东去,浪淘尽,千古风流人物", "江山如画，一时多少豪杰", "我欲乘风归去，又恐琼楼玉宇，高处不胜寒" };
            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(hudgo);
                go.transform.SetParent(transform, false);
                HudCanvasRenderer canvasRenderer = go.GetComponent<HudCanvasRenderer>();
                if (canvasRenderer != null)
                {
                    HudText humText = canvasRenderer.GetHudComponet<HudText>("Text");
                    if (humText != null)
                    {
                        humText.text = texts[Random.Range(0, texts.Length)];
                        humText.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
                    }
                }
                TextMeshProUGUI textMeshProUGUI = go.GetComponent<TextMeshProUGUI>();
                if (textMeshProUGUI != null)
                {
                    textMeshProUGUI.text = texts[Random.Range(0, texts.Length)];
                    textMeshProUGUI.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
                }

                TextMeshPro textMeshPro = go.GetComponent<TextMeshPro>();
                if (textMeshPro != null)
                {
                    textMeshPro.text = texts[Random.Range(0, texts.Length)];
                    textMeshPro.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
                }


                Vector3 dir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                dir = dir.normalized;
                float len = Random.Range(12, 50);
                go.transform.position = dir * len;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, Time.time * 5, 0);
            Vector3 pos = new Vector3(0, 0, 50 - 150 * slider.value);
            if (cameraTrans != null)
            {
                cameraTrans.transform.position = pos;
            }
        }
    }
}