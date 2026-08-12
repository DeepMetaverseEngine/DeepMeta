using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NFCore.Extension
{
    public class HudLauncher : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //float w = Screen.width;
            //float h = Screen.height;
            //float ch = 480;
            //float cw = (int)(ch * w / h);
            //Screen.SetResolution((int)cw, (int)ch, true);
            GameObject.DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                HudRendererCenter.Destory();
            }
        }

        public void HudHead()
        {
            SceneManager.LoadScene("HudHead");
        }

        public void HudHeadSeparation()
        {
            SceneManager.LoadScene("HudHeadSeparation");
        }

        public void HudImage()
        {
            SceneManager.LoadScene("HudImage");
        }

        public void HudLevel()
        {
            SceneManager.LoadScene("HudLevel");
        }

        public void HudNum()
        {
            SceneManager.LoadScene("HudNum");
        }

        public void HudText()
        {
            SceneManager.LoadScene("HudText");
        }

        public void HudTextInstance()
        {
            SceneManager.LoadScene("HudTextInstance");
        }
        public void HudTextUGUI()
        {
            SceneManager.LoadScene("HudTextUGUI");
        }
        public void HudTextMeshRender()
        {
            SceneManager.LoadScene("HudTextMeshRenderer");
        }
    }
}