using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UnoPropertyGrid;

namespace MarkdownViewTest;

public sealed class MultilineTextEditorProvider : IPropertyGridEditorProvider
{
    static void Commit(PropertyGridEditorContext context, object? value)
    {
        context.SetValue?.Invoke(value);
        context.Value = context.Descriptor.GetValue();
    }

    public bool CanEdit(PropertyGridEditorContext context)
    {
        return context.Descriptor.Name == nameof(MarkdownThemeSettings.CustomCss);
    }

    public FrameworkElement CreateEditor(PropertyGridEditorContext context)
    {
        var textBox = new TextBox
        {
            Text = context.Value?.ToString() ?? string.Empty,
            AcceptsReturn = true,
            Height = 120,
            TextWrapping = Microsoft.UI.Xaml.TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Top
        };
        textBox.TextChanged += (_, _) => Commit(context, textBox.Text);
        return textBox;
    }
}
