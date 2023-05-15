using UnityEngine.UIElements;
using UnityEngine;

namespace UIBuilder
{
    public class CMSCollapsibleText : VisualElement {
        private VisualElement _wrapperTextElement;
        private Label _shortTextElement;
        private Label _longTextElement;
        private Button _toggleTextButton;

        public CMSCollapsibleText(string shortText, string longText, string readMore, string readLess) {
            this._wrapperTextElement = new VisualElement();
            this._shortTextElement = new Label(shortText);
            this._longTextElement = new Label(longText);

            this._toggleTextButton = new Button();
            this._toggleTextButton.text = readMore;
            this._toggleTextButton.AddToClassList("cms-toggleTextButton");

            this._longTextElement.style.display = DisplayStyle.None;

            this._toggleTextButton.clicked += () => {

                if (this._toggleTextButton.text == readMore)
                {
                    this._toggleTextButton.text = readLess;
                    this._longTextElement.style.display = DisplayStyle.Flex;
                }

                else if (this._toggleTextButton.text == readLess)
                {
                    this._toggleTextButton.text = readMore;
                    this._longTextElement.style.display = DisplayStyle.None;
                }


            };

            this._wrapperTextElement.Add(this._shortTextElement);
            this._wrapperTextElement.Add(this._longTextElement);
            this._wrapperTextElement.Add(this._toggleTextButton);
            Add(this._wrapperTextElement);
            this._shortTextElement.AddToClassList("cms-text");
            this._longTextElement.AddToClassList("cms-text");
        }
    }
}