using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace NLog.Server
{
    public class LogHttpClient : MonoBehaviour
    {

        public static LogHttpClient Ins;

        public string URL = "http://127.0.0.1:5432";

        IoBuffer _Buffer = new IoBuffer(10240);

        void Awake()
        {
            Ins = this;
        }

        void OnDestroy()
        {
            Ins = null;
        }

        public static LogHttpClient GetIns()
        {
            return Ins;
        }

        public void SetUrl(string url)
        {
            URL = url;
        }

        public void Post(short cmd, byte[] bytes, Action<byte[]> callback)
        {
            _Buffer.Clear();

            _Buffer.PutBytes(bytes);

            //NINFO 这个为了帮助server去掉client httpRequest头       
            int msgContentLen = _Buffer.GetLength();
            _Buffer.PutInt(msgContentLen);
            byte[] bs = _Buffer.ToArray();

            //Debug.Log("客户端发送len :" + bs.Length+" msgContentLen:"+msgContentLen);

            //for (int i = 0; i < bs.Length; i++)
            //{
            //    Debug.LogError("client send: i:" + i + "--->" + bs[i]);
            //}

            StartCoroutine(Post(bs, callback, false));

        }

        public void Get(string url, Action<string> callback)
        {
            StartCoroutine(Get(url, callback, false));
        }

        private IEnumerator Get(string url, Action<string> callback, bool b = false)
        {

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.error != null)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.responseCode == 200)//200表示接受成功
                    {
                        callback?.Invoke(www.downloadHandler.text);
                    }
                    else
                    {
                        Debug.LogError("HttpNetwork.Get responeCode:" + www.responseCode);
                    }

                }
            }
        }

        private IEnumerator Post(byte[] postBytes, Action<byte[]> callback = null, bool b = false)
        {
            using (UnityWebRequest www = new UnityWebRequest(URL, "POST"))
            {
                www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);

                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    //NINFO 发生问题再打开，log系统自身不打印log
                    //Debug.Log("wwwError:" + www.error);
                }
                else
                {

                    if (www.responseCode == 200)
                    {
                        //Debug.Log("客户端www.responseCode == 200");
                        callback?.Invoke(www.downloadHandler.data);
                    }
                    else
                    {
                        //NINFO 发生问题再打开，log系统自身不打印log
                        //Debug.LogError("HttpNetwork.Post responeCode:" + www.responseCode);
                    }

                }
            }
        }

    }

}

