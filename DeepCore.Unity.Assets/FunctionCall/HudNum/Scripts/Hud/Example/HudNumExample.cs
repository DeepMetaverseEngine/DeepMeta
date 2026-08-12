using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace NFCore.Extension
{

    public class HudNumExample : MonoBehaviour
    {
        public GameObject hudgo;
        public int count = 0;
        public Transform cameraTrans;
        public Slider slider;

        private void Awake()
        {
            if (!enabled) return;

            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(hudgo);
                go.transform.SetParent(transform, false);
                HudCanvasRenderer canvasRenderer = go.GetComponent<HudCanvasRenderer>();
                if (canvasRenderer != null)
                {
                    HudNum humNum = canvasRenderer.GetHudComponet<HudNum>("Num");

                    #region 设置数字类型
                    var HudNumTypeDict = new Dictionary<string, Dictionary<int, string>>();

                    var subDict = new Dictionary<int, string>();
                    //-----------------------------------------------------------------------------

                    //普通伤害 白字0-9
                    subDict.Add(0, "w0");
                    subDict.Add(1, "w1");
                    subDict.Add(2, "w2");
                    subDict.Add(3, "w3");
                    subDict.Add(4, "w4");
                    subDict.Add(5, "w5");
                    subDict.Add(6, "w6");
                    subDict.Add(7, "w7");
                    subDict.Add(8, "w8");
                    subDict.Add(9, "w9");

                    HudNumTypeDict.Add("Normal", subDict);

                    HudNum.SetNumTypeDict(HudNumTypeDict, "Heal");

                    #endregion

                    testlist.Add(humNum);
                    if (humNum != null)
                    {
                        //humNum.Num = (Random.Range(-99999999, 99999999));//模式1
                        humNum.SetNum("Normal", Random.Range(-99999999, 99999999));//模式2
                        humNum.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
                    }
                }
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                dir = dir.normalized;
                float len = Random.Range(12, 50);
                go.transform.position = cameraTrans.position + dir * len;
                //go.transform.position = new Vector3(10, 0, -i);//
                testTrans.Add(go.transform);//
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            //这里控制坐标旋转
            transform.rotation = Quaternion.Euler(0, Time.time * 5, 0);
            Vector3 pos = new Vector3(0, 0, 50 - 150 * slider.value);
            cameraTrans.transform.position = pos;

            /*     for (int i = 0; i < testlist.Count; i++)
                 {
                     testlist[i].position = testlist[i].position + new Vector2(10, 0);
                     var v = testTrans[i].position;
                     v.x = v.x * -1;
                     testTrans[i].position = v;
                 }*/
        }

        private List<HudNum> testlist = new List<HudNum>(5000);
        private List<Transform> testTrans = new List<Transform>(5000);

        [Button("变更")]
        private void ChangeNum()
        {
            for (int i = 0; i < testlist.Count; i++)
            {
                testlist[i].SetText("miss"); testlist[i].fontSize = 10;
            }
        }
    }
}
