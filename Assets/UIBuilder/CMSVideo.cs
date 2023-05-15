using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEngine.UI;

namespace UIBuilder {
    public class CMSVideo : VisualElement {
        UnityEngine.UIElements.Label videoTitle = new UnityEngine.UIElements.Label();
        UnityEngine.UIElements.Button videoPanel = new UnityEngine.UIElements.Button();
        VisualElement playIconTest = new VisualElement();

        public CMSVideo(VideoItem video, UIItemViewController uIItemViewController) {
            videoPanel.AddToClassList("cms-video-thumbnail");
            playIconTest.AddToClassList("cms-video-play-icon");
            videoPanel.Add(playIconTest);
            videoTitle.text = video.headline.ToUpper();
            videoTitle.AddToClassList("cms-arch-headline");

            Add(videoTitle);
            Add(videoPanel);
            Add(new Label("Video"));
#if DEVELOPMENT_BUILD
    string baseUrl = "https://var-staging.xailabs.com";
#else
            string baseUrl = "https://var-production.xailabs.com";
#endif
            videoPanel.clicked += delegate {
                uIItemViewController.videoPlayer.url = baseUrl + video.video.data.attributes.url;
                uIItemViewController.VideoCanvas.SetActive(true);

                if (video.orientation == "vertical") {
                    uIItemViewController.VideoPanel.transform.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(1080, 1920);
                    uIItemViewController.VideoPanel.transform.GetComponent<RectTransform>().rotation =
                        Quaternion.Euler(0, 0, 0);
                    Debug.Log("orientation: vertical");
                    uIItemViewController.videoPlayer.GetComponent<VideoPlayer>().targetTexture =
                        Resources.Load<RenderTexture>("Video/videoTextureVertical");
                    uIItemViewController.VideoPanel.transform.GetComponent<RawImage>().texture =
                        Resources.Load<RenderTexture>("Video/videoTextureVertical");
                } else if (video.orientation == "horizontal") {
                    Debug.Log("orientation: horizontal");
                    uIItemViewController.videoPlayer.GetComponent<VideoPlayer>().targetTexture =
                        Resources.Load<RenderTexture>("Video/videoTextureHorizontal");
                    uIItemViewController.VideoPanel.transform.GetComponent<RawImage>().texture =
                        Resources.Load<RenderTexture>("Video/videoTextureHorizontal");
                    uIItemViewController.VideoPanel.transform.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(1920, 1080);
                    uIItemViewController.VideoPanel.transform.GetComponent<RectTransform>().rotation =
                        Quaternion.Euler(0, 0, -90);
                }
            };
        }
    }
}