using System.Collections.Generic;
using UnityEngine;


namespace NFCore.Extension
{
    public class TransformExample : MonoBehaviour
    {

        public int ObjCount = 5000;
        public Transform cameraTrans;
        public List<Transform> objsTrans;
        public bool startRot = false;
        private void Awake()
        {
            GameObject go = null;
            objsTrans = new List<Transform>(ObjCount);
            for (int i = 0; i < ObjCount; i++)
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //go = new GameObject();
                go.transform.SetParent(transform, false);

                Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                dir = dir.normalized;
                float len = Random.Range(12, 50);
                go.transform.position = cameraTrans.position + dir * len;
                go.transform.LookAt(cameraTrans.position);
                objsTrans.Add(go.transform);
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

            if (!startRot) return;

            for (int i = 0; i < objsTrans.Count; i++)
            {
                objsTrans[i].rotation = Quaternion.Euler(0, Time.time * Random.Range(5,15), 0);
            }
        }
    }
}
