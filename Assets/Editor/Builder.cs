using UnityEditor;
using UnityEngine.SceneManagement;

namespace Varusschlacht {
    public class Builder {
        public static void BuildProject()
        {
            var options = new BuildPlayerOptions
            {
                scenes = new[] { "Assets/MainScene.unity" }, 
                target = UnityEditor.BuildTarget.iOS, 
                locationPathName = "/tmp/ios",
            };

            BuildPipeline.BuildPlayer(options);
        }
    }
}