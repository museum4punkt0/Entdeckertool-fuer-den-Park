using UnityEngine.UIElements;
using UnityEngine;

public class ErrorLog : VisualElement {
    VisualElement error;
    VisualElement root;
    Button close;

    public new class UxmlFactory : UxmlFactory<ErrorLog, UxmlTraits> {
    }
    public new class UxmlTraits : VisualElement.UxmlTraits {
    }

    public ErrorLog() {
        this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }

    private void OnGeometryChange(GeometryChangedEvent evt) {
        
        error = this.Q<VisualElement>("error");
        close = this.Q<Button>("closebtn");

        /*
        close.RegisterCallback<ClickEvent>(ev =>
            this.style.display = DisplayStyle.None
        );
        */

        UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
    }
}
