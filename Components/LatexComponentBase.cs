using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Quaze.Components;

public class LatexComponentBase : ComponentBase
{
    [Inject]
    private IJSRuntime JsR {get;set;}


    protected override void OnAfterRender(bool firstRender)
    {
        JsR.InvokeVoidAsync("MathJax.typeset");
    }
}