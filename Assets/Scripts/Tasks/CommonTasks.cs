using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace Tasks {
    public class CommonTasks {
        public static async Task<Texture2D> GetRemoteTexture ( string url )
        {
            using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
            {
                // begin request:
                var asyncOp = www.SendWebRequest();

                // await until it's done: 
                while( asyncOp.isDone==false )
                    await Task.Delay( 1000/30 );//30 hertz
        
                // read results:
                if( www.isNetworkError || www.isHttpError )
                    // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
                {
                    // log error:
#if DEBUG
                    Debug.Log( $"{www.error}, URL:{www.url}" );
#endif
            
                    // nothing to return on error:
                    return null;
                }
                else
                {
                    // return valid results:
                    return DownloadHandlerTexture.GetContent(www);
                }
            }
        }

    }
}