using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;
using UnityEngine.UIElements;

public class Game1ContentLoader : MonoBehaviour
{
    CrossGameManager crossGameManager;

    public VisualElement m_Root;
    public VisualElement Level1;
    public VisualElement Level2;
    public VisualElement Level3;
    public VisualElement nextPhase2Images;
    public VisualElement nextPhase3Images;
    public VisualElement win;

    public string onCorrectAnswer;
    public string onFalseAnswer;
    public string onCorrectFalseAnswer;
    public string onFalseCorrectAnswer;
    public string onTryAgain;

    bool isOn = false;

    private void Awake() {
        crossGameManager = GameObject.FindGameObjectWithTag("CrossGameManager").GetComponent<CrossGameManager>();
    }

    void Start()
    {
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;
        this.Level1 = this.m_Root.Q<VisualElement>("Level1");
        this.Level2 = this.m_Root.Q<VisualElement>("Level2");
        this.Level3 = this.m_Root.Q<VisualElement>("Level3");
        this.nextPhase2Images = this.m_Root.Q<VisualElement>("NextPhase2").Q<VisualElement>("Content").Q<VisualElement>("Images");
        this.nextPhase3Images = this.m_Root.Q<VisualElement>("NextPhase3").Q<VisualElement>("Content").Q<VisualElement>("Images");
        this.win = this.m_Root.Q<VisualElement>("Win").Q<VisualElement>("Content").Q<VisualElement>("Images");

        StartCoroutine(crossGameManager.strapiService.getSpiel1Content(loadPopups));
        StartCoroutine(crossGameManager.strapiService.getSpiel1Content(loadContent));
    }

    async void loadPopups(StrapiSingleResponse<Game1Data> res) {
        Game1Data _data = res.data;

        onCorrectAnswer = _data.attributes.correctAnswer;
        onFalseAnswer = _data.attributes.falseAnswer;
        onCorrectFalseAnswer = _data.attributes.correctFalseAnswer;
        onFalseCorrectAnswer = _data.attributes.falseCorrectAnswer;
        onTryAgain = _data.attributes.tryAgain;
    }

    async void loadContent(StrapiSingleResponse<Game1Data> res) {
        Game1Data _data = res.data;

        //LEVEL1
        foreach (Game1DataContent item in _data.attributes.content) {
            if (item.level == "level1") {
                VisualElement imgBox = new VisualElement();
                imgBox.AddToClassList("level-main-image");
                //imgBox.style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                imgBox.style.display = DisplayStyle.None;

                string imgUrl = item.image.data.attributes.GetFullImageUrl();
                Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(imgBox).start();


                this.Level1.Add(imgBox);
                imgBox.name = item.roman.ToString();
                
                if (item.roman) {
                    for (int i= 0; i < nextPhase2Images.childCount; i++) {
                        if (nextPhase2Images[i].name == "empty") {
                            nextPhase2Images[i].name = "image";
                            Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(nextPhase2Images[i]).start();
                            //nextPhase2Images[i].Q<Button>("correct").style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                            nextPhase2Images[i].Q<Button>("correct").Q<Label>("labelInfo").text = item.title.ToString();
                            break;
                        }

                    }

                }
                
            }
        }

        nextPhase2Images[0].Q<Button>("correct").clicked += delegate {
                if (!isOn) {
                    nextPhase2Images[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                    nextPhase2Images[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                    isOn = true;
                } else if (isOn) {
                    nextPhase2Images[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                    nextPhase2Images[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                    isOn = false;
                }
        };

        nextPhase2Images[1].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase2Images[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase2Images[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase2Images[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase2Images[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        nextPhase2Images[2].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase2Images[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase2Images[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase2Images[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase2Images[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        nextPhase2Images[3].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase2Images[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase2Images[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase2Images[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase2Images[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };


        //LEVEL2
        foreach (Game1DataContent item in _data.attributes.content) {
            if (item.level == "level2") {
                VisualElement imgBox = new VisualElement();
                imgBox.AddToClassList("level-main-image");
                //imgBox.style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                imgBox.style.display = DisplayStyle.None;

                string imgUrl = item.image.data.attributes.GetFullImageUrl();
                Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(imgBox).start();

                this.Level2.Add(imgBox);

                imgBox.name = item.roman.ToString();

                if (item.roman) {
                    for (int i = 0; i < nextPhase3Images.childCount; i++) {
                        if (nextPhase3Images[i].name == "empty") {
                            nextPhase3Images[i].name = "image";
                            Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(nextPhase3Images[i]).start();
                            //nextPhase3Images[i].Q<VisualElement>("correct").style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                            nextPhase3Images[i].Q<Button>("correct").Q<Label>("labelInfo").text = item.title.ToString();
                            break;
                        }

                    }

                }

            }
        }

        nextPhase3Images[0].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase3Images[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase3Images[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase3Images[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase3Images[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        nextPhase3Images[1].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase3Images[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase3Images[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase3Images[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase3Images[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        nextPhase3Images[2].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase3Images[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase3Images[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase3Images[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase3Images[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        nextPhase3Images[3].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                nextPhase3Images[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                nextPhase3Images[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                nextPhase3Images[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                nextPhase3Images[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };


        //LEVEL3
        foreach (Game1DataContent item in _data.attributes.content) {
            if (item.level == "level3") {
                VisualElement imgBox = new VisualElement();
                imgBox.AddToClassList("level-main-image");
                //imgBox.style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                imgBox.style.display = DisplayStyle.None;

                string imgUrl = item.image.data.attributes.GetFullImageUrl();
                Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(imgBox).start();

                this.Level3.Add(imgBox);

                imgBox.name = item.roman.ToString();

                if (item.roman) {
                    for (int i = 0; i < win.childCount; i++) {
                        if (win[i].name == "empty") {
                            win[i].name = "image";
                            Davinci.get().load(imgUrl).setLoadingPlaceholder(Resources.Load<Texture2D>("Images/loadingwheel")).into(win[i]).start();
                            //win[i].Q<VisualElement>("correct").style.backgroundImage = new StyleBackground(await item.image.GetTexture2D());
                            win[i].Q<Button>("correct").Q<Label>("labelInfo").text = item.title.ToString();
                            break;
                        }

                    }

                }

            }
        }

        win[0].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                win[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                win[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                win[0].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                win[0].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        win[1].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                win[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                win[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                win[1].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                win[1].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        win[2].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                win[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                win[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                win[2].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                win[2].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };

        win[3].Q<Button>("correct").clicked += delegate {
            if (!isOn) {
                win[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.None;
                win[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.Flex;
                isOn = true;
            } else if (isOn) {
                win[3].Q<Button>("correct").Q<VisualElement>("img").style.display = DisplayStyle.Flex;
                win[3].Q<Button>("correct").Q<VisualElement>("info").style.display = DisplayStyle.None;
                isOn = false;
            }
        };


    }
}
