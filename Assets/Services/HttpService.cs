using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public class HttpService
    {

            public CachingService cachingService;
            public HttpService(string cacheLocation) {
                this.cachingService = new CachingService(cacheLocation);
            }

            // Web requests are typially done asynchronously, so Unity's web request system
            // returns a yield instruction while it waits for the response.
            //

            public IEnumerator RequestRoutine(string url, Action<string> callback = null)
            {
                var data  = "";
                string cleanUrl = url.Replace("?", "").Replace("*", "");

                if (this.cachingService.hasKey(cleanUrl)) {
                    data = this.cachingService.getValue(cleanUrl);
                } else {
                    // Using the static constructor
                    var request = UnityWebRequest.Get(url);

                    // Wait for the response and then get our data
                    yield return request.SendWebRequest();
                    data = request.downloadHandler.text;
                    this.cachingService.setValue(cleanUrl, data);
                    // This isn't required, but I prefer to pass in a callback so that I can
                    // act on the response data outside of this function
                }

                if (callback != null)
                    callback(data);
            }
    }


    public class CachingService {
        private string rootDirectory;

        public CachingService(string rootDirectory) {
            this.rootDirectory = rootDirectory;
            Directory.CreateDirectory(this.rootDirectory);
        }

        private string getFilename(string key) {
            key = key.Replace("/", "").Replace(":", "");
            return this.rootDirectory + "/" + key;
        }

        /*
        public void clearCache() {
            Directory.Delete(this.rootDirectory,true);
            Directory.CreateDirectory(this.rootDirectory);
            Debug.Log("Cache has been cleared");
        }
        */

        public void setValue(string key, string value) {
            // save value into file identified by key - should include a date
            Debug.Log("Saved to cache: " + key);
            Debug.Log("Curent Path: " + this.getFilename(key));
            File.WriteAllText(this.getFilename(key), value);
        }

        public string getValue(string key) {
            // get contents of file by key if file is not invalidated
            Debug.Log("Got from cache: " + key);
            return File.ReadAllText(this.getFilename(key));
        }

        public bool hasKey(string key) {
            // check if file with key exists and is not invalidated
            return File.Exists(this.getFilename(key)) ;
        }

        public void clearCache() {

        #if UNITY_IOS

            try {
                Debug.Log("tries to delete root");
                Directory.Delete(this.rootDirectory, true);
            } catch (IOException) {
                Debug.Log("ios exception");
                Directory.Delete(this.rootDirectory, true);
            } catch (UnauthorizedAccessException) {
                Debug.Log("unauth");
                Directory.Delete(this.rootDirectory, true);
            }
            //Debug.Log("Directories in cache root" + Directory.GetDirectories(this.rootDirectory).Length);
            //Debug.Log("files in cache root" + Directory.GetFiles(this.rootDirectory).Length);
            Directory.CreateDirectory(this.rootDirectory);

            #endif

            #if UNITY_ANDROID
                        foreach (string directory in Directory.GetDirectories(this.rootDirectory)) {
                            DeleteDirectory(directory);
                        }
                        try {
                            Debug.Log("tries to delete root");
                            Directory.Delete(this.rootDirectory, true);
                        
                        } catch (IOException) {
                            Debug.Log("ios exception");
                            Directory.Delete(this.rootDirectory, true);
                        } catch (UnauthorizedAccessException) {
                            Debug.Log("unauth");



                            Directory.Delete(this.rootDirectory, true);
                        }

                        Directory.CreateDirectory(this.rootDirectory);
                    
            #endif
            
        }

        public static void DeleteDirectory(string target_dir) {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);
            foreach (string file in files) {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs) {
                DeleteDirectory(dir);
            }
            Directory.Delete(target_dir, false);
        }

    }
}