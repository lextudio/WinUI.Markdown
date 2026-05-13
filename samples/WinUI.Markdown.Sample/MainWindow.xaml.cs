using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WinUI.Markdown.Controls;
using WinUI.Markdown.Themes;

namespace MarkdownViewTest;

public sealed partial class MainWindow : Window
{
    private bool _isInitialized;
    private readonly MarkdownThemeSettings _settings = new();
    private readonly DispatcherQueueTimer _debounceTimer;

    public MainWindow()
    {
        InitializeComponent();

        _debounceTimer = DispatcherQueue.CreateTimer();
        _debounceTimer.Interval = TimeSpan.FromMilliseconds(120);
        _debounceTimer.Tick += (_, _) => { _debounceTimer.Stop(); ApplyTheme(); };

        MarkdownEditor.Text = SampleMarkdown;
        Viewer.Text = SampleMarkdown;

        PropsPanel.EditorProviders.Add(new MultilineTextEditorProvider());

        LoadThemePreset();
        PropsPanel.SelectedObject = _settings;

        _settings.PropertyChanged += OnSettingsChanged;

        ApplyTheme();
        ApplyViewVisibility();
        _isInitialized = true;
    }

    private string SampleMarkdown { get; } = """
        # WinUI.Markdown

        Type on the left and tune the control on the right.

        ## Native-friendly content

        - **Bold**, *italic*, ~~strike~~, and `inline code`
        - [x] Task lists render as read-only native checkboxes
        - [ ] Nested lists, tables, images, and horizontal rules
        - [Link click events](https://github.com/your-org/WinUI.Markdown)

        > Auto mode keeps this in native WinUI until the input needs the HTML renderer.

        | Feature | Native |
        | --- | --- |
        | Headings | Yes |
        | Tables | Yes |
        | Raw HTML | WebView2 fallback |

        ```csharp
        public sealed class Demo
        {
            public string Name { get; init; } = "native";
        }
        ```

        ---

        <details><summary>Raw HTML makes Auto choose WebView2</summary>HTML block</details>
        """;

    private void OnSettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!_isInitialized) return;

        switch (e.PropertyName)
        {
            case nameof(MarkdownThemeSettings.ThemePreset):
                LoadThemePreset();
                ApplyTheme();
                break;
            case nameof(MarkdownThemeSettings.RenderMode):
                Viewer.RenderMode = _settings.RenderMode;
                break;
            case nameof(MarkdownThemeSettings.AllowWebView2Fallback):
                Viewer.AllowWebView2Fallback = _settings.AllowWebView2Fallback;
                break;
            default:
                QueueApplyTheme();
                break;
        }
    }

    private void LoadThemePreset()
    {
        var preset = _settings.ThemePreset switch
        {
            ThemePreset.WinUILight  => MarkdownTheme.WinUILight,
            ThemePreset.WinUIDark   => MarkdownTheme.WinUIDark,
            ThemePreset.GitHubLight => MarkdownTheme.GitHubLight,
            ThemePreset.GitHubDark  => MarkdownTheme.GitHubDark,
            ThemePreset.Dracula     => MarkdownTheme.Dracula,
            _ => Viewer.ActualTheme == ElementTheme.Dark ? MarkdownTheme.WinUIDark : MarkdownTheme.WinUILight
        };

        _isInitialized = false;
        _settings.LoadFrom(preset);
        _isInitialized = true;
        PropsPanel.Refresh();
    }

    private void QueueApplyTheme()
    {
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private void ApplyTheme()
    {
        var tracksSystem = _settings.ThemePreset == ThemePreset.Auto;
        var theme = _settings.ToTheme(tracksSystem);
        Viewer.Theme = theme;
        Viewer.MaxImageWidth = _settings.MaxImageWidth;
        PreviewHost.Background = tracksSystem ? ResolveSystemPreviewBackground() : theme.Background;
    }

    private static double HeadingSize(MarkdownTheme theme, int index)
    {
        return theme.HeadingSizes.Length > index ? theme.HeadingSizes[index] : theme.BodyFontSize;
    }

    private static double HeadingWeight(MarkdownTheme theme, int index)
    {
        return theme.HeadingFontWeights.Length > index ? theme.HeadingFontWeights[index].Weight : 600;
    }

    private static double HeadingBorderThickness(MarkdownTheme theme, int index)
    {
        return theme.HeadingBorderThicknesses.Length > index ? theme.HeadingBorderThicknesses[index] : 0;
    }

    private SolidColorBrush ResolveSystemPreviewBackground()
    {
        var palette = Viewer.ActualTheme == ElementTheme.Dark ? MarkdownTheme.WinUIDark : MarkdownTheme.WinUILight;
        return palette.Background as SolidColorBrush ?? new SolidColorBrush(Microsoft.UI.Colors.Transparent);
    }

    private void OnMarkdownChanged(object sender, TextChangedEventArgs e)
    {
        if (_isInitialized)
            Viewer.Text = MarkdownEditor.Text;
    }

    private void OnMarkdownRendered(object sender, MarkdownRenderedEventArgs e)
    {
        ActualModeText.Text = $"Requested: {e.RequestedRenderMode}   Actual: {e.ActualRenderMode}";
    }

    private async void OnLinkClicked(object sender, MarkdownLinkEventArgs e)
    {
        e.Handled = true;
        var dialog = new ContentDialog
        {
            Title = "Link clicked",
            Content = e.Url,
            CloseButtonText = "OK",
            XamlRoot = Viewer.XamlRoot
        };
        await dialog.ShowAsync();
    }

    private void OnViewToggleChanged(object sender, RoutedEventArgs e)
    {
        if (_isInitialized)
            ApplyViewVisibility();
    }

    private void ApplyViewVisibility()
    {
        var showProps = ShowPropsToggle.IsChecked == true;
        SetColumnVisibility(MarkdownEditor, InputColumn, ShowInputToggle.IsChecked == true, 360);
        SetColumnVisibility(PreviewHost, PreviewColumn, ShowPreviewToggle.IsChecked == true, 1, true);
        SetColumnVisibility(PropsPanel, PropsColumn, showProps, 320);
        PropsSplitter.Visibility = showProps ? Visibility.Visible : Visibility.Collapsed;
    }

    private static void SetColumnVisibility(UIElement element, ColumnDefinition column, bool isVisible, double width, bool star = false)
    {
        element.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        column.Width = isVisible
            ? star ? new GridLength(width, GridUnitType.Star) : new GridLength(width)
            : new GridLength(0);
    }
}
