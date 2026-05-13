using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;
using WinUI.Markdown.Controls;
using WinUI.Markdown.Themes;

namespace MarkdownViewTest;

public enum ThemePreset { Auto, WinUILight, WinUIDark, GitHubLight, GitHubDark, Dracula }

public sealed class MarkdownThemeSettings : INotifyPropertyChanged
{
    ThemePreset _themePreset = ThemePreset.Auto;
    RenderMode _renderMode = RenderMode.Auto;
    bool _allowWebView2Fallback = true;

    FontFamily _bodyFont = new("Segoe UI");
    FontFamily _codeFont = new("Cascadia Mono, Consolas");
    double _bodyFontSize = 14;

    double _h1Size = 32, _h2Size = 28, _h3Size = 24, _h4Size = 20, _h5Size = 18, _h6Size = 16;

    Windows.UI.Text.FontWeight _h1Weight = FontWeights.SemiBold;
    Windows.UI.Text.FontWeight _h2Weight = FontWeights.SemiBold;
    Windows.UI.Text.FontWeight _h3Weight = FontWeights.SemiBold;
    Windows.UI.Text.FontWeight _h4Weight = FontWeights.SemiBold;
    Windows.UI.Text.FontWeight _h5Weight = FontWeights.SemiBold;
    Windows.UI.Text.FontWeight _h6Weight = FontWeights.SemiBold;

    double _headingSpacingBefore = 16;
    double _headingSpacingAfter = 6;
    double _paragraphSpacing = 8;
    double _blockSpacing = 10;
    double _listSpacing = 8;

    Brush _background = Solid("#FFFFFFFF");
    Brush _foreground = Solid("#FF111111");
    Brush _codeBackground = Solid("#14000000");
    Brush _codeForeground = Solid("#FF24292F");
    Brush _codeBorder = Solid("#00000000");
    double _codeBorderThickness = 0;
    Microsoft.UI.Xaml.CornerRadius _codeCornerRadius = new(6);
    Microsoft.UI.Xaml.CornerRadius _inlineCodeCornerRadius = new(4);

    Brush _blockquoteAccent = Solid("#FF60789C");
    Brush _blockquoteBackground = Solid("#08000000");
    Microsoft.UI.Xaml.CornerRadius _blockquoteCornerRadius = new(0);

    Brush _headingBorder = Solid("#50000000");
    double _h1BorderThickness = 0;
    double _h2BorderThickness = 0;
    double _headingBorderPaddingEm = 0;

    Brush _linkForeground = Solid("#FF005FB8");
    Brush _tableBorder = Solid("#50000000");
    Brush _tableHeaderBackground = Solid("#0A000000");
    Brush _tableRowAlternateBackground = Solid("#00000000");
    Brush _hrStroke = Solid("#78000000");

    double _maxImageWidth = 720;
    string _customCss = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    [Category("Control")]
    [Description("Load a built-in color/font preset. Changing this overwrites the theme fields below.")]
    public ThemePreset ThemePreset { get => _themePreset; set => Set(ref _themePreset, value); }

    [Category("Control")]
    [Description("How the Markdown is rendered.")]
    public RenderMode RenderMode { get => _renderMode; set => Set(ref _renderMode, value); }

    [Category("Control")]
    [Description("Fall back to WebView2 when native rendering cannot handle the content.")]
    public bool AllowWebView2Fallback { get => _allowWebView2Fallback; set => Set(ref _allowWebView2Fallback, value); }

    [Category("Typography")]
    [Description("Body text font family.")]
    public FontFamily BodyFont { get => _bodyFont; set => Set(ref _bodyFont, value); }

    [Category("Typography")]
    [Description("Monospace font family for code blocks and inline code.")]
    public FontFamily CodeFont { get => _codeFont; set => Set(ref _codeFont, value); }

    [Category("Typography")]
    [Description("Base font size for body text in pixels.")]
    public double BodyFontSize { get => _bodyFontSize; set => Set(ref _bodyFontSize, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H1 headings.")]
    public double H1Size { get => _h1Size; set => Set(ref _h1Size, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H2 headings.")]
    public double H2Size { get => _h2Size; set => Set(ref _h2Size, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H3 headings.")]
    public double H3Size { get => _h3Size; set => Set(ref _h3Size, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H4 headings.")]
    public double H4Size { get => _h4Size; set => Set(ref _h4Size, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H5 headings.")]
    public double H5Size { get => _h5Size; set => Set(ref _h5Size, value); }

    [Category("Heading Sizes")]
    [Description("Font size for H6 headings.")]
    public double H6Size { get => _h6Size; set => Set(ref _h6Size, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H1 headings.")]
    public Windows.UI.Text.FontWeight H1Weight { get => _h1Weight; set => Set(ref _h1Weight, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H2 headings.")]
    public Windows.UI.Text.FontWeight H2Weight { get => _h2Weight; set => Set(ref _h2Weight, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H3 headings.")]
    public Windows.UI.Text.FontWeight H3Weight { get => _h3Weight; set => Set(ref _h3Weight, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H4 headings.")]
    public Windows.UI.Text.FontWeight H4Weight { get => _h4Weight; set => Set(ref _h4Weight, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H5 headings.")]
    public Windows.UI.Text.FontWeight H5Weight { get => _h5Weight; set => Set(ref _h5Weight, value); }

    [Category("Heading Weights")]
    [Description("Font weight for H6 headings.")]
    public Windows.UI.Text.FontWeight H6Weight { get => _h6Weight; set => Set(ref _h6Weight, value); }

    [Category("Spacing")]
    [Description("Space above each heading.")]
    public double HeadingSpacingBefore { get => _headingSpacingBefore; set => Set(ref _headingSpacingBefore, value); }

    [Category("Spacing")]
    [Description("Space below each heading.")]
    public double HeadingSpacingAfter { get => _headingSpacingAfter; set => Set(ref _headingSpacingAfter, value); }

    [Category("Heading Borders")]
    [Description("Color of the decorative border drawn under headings.")]
    public Brush HeadingBorder { get => _headingBorder; set => Set(ref _headingBorder, value); }

    [Category("Heading Borders")]
    [Description("Border thickness drawn under H1.")]
    public double H1BorderThickness { get => _h1BorderThickness; set => Set(ref _h1BorderThickness, value); }

    [Category("Heading Borders")]
    [Description("Border thickness drawn under H2.")]
    public double H2BorderThickness { get => _h2BorderThickness; set => Set(ref _h2BorderThickness, value); }

    [Category("Heading Borders")]
    [Description("Vertical padding between heading text and its border, as a fraction of the font size.")]
    public double HeadingBorderPaddingEm { get => _headingBorderPaddingEm; set => Set(ref _headingBorderPaddingEm, value); }

    [Category("Spacing")]
    [Description("Space between paragraphs.")]
    public double ParagraphSpacing { get => _paragraphSpacing; set => Set(ref _paragraphSpacing, value); }

    [Category("Spacing")]
    [Description("Space between block-level elements.")]
    public double BlockSpacing { get => _blockSpacing; set => Set(ref _blockSpacing, value); }

    [Category("Spacing")]
    [Description("Space between list items.")]
    public double ListSpacing { get => _listSpacing; set => Set(ref _listSpacing, value); }

    [Category("Colors")]
    [Description("Page background color.")]
    public Brush Background { get => _background; set => Set(ref _background, value); }

    [Category("Colors")]
    [Description("Default text color.")]
    public Brush Foreground { get => _foreground; set => Set(ref _foreground, value); }

    [Category("Colors")]
    [Description("Background color for code blocks.")]
    public Brush CodeBackground { get => _codeBackground; set => Set(ref _codeBackground, value); }

    [Category("Colors")]
    [Description("Text color inside code blocks.")]
    public Brush CodeForeground { get => _codeForeground; set => Set(ref _codeForeground, value); }

    [Category("Colors")]
    [Description("Border color for code blocks.")]
    public Brush CodeBorder { get => _codeBorder; set => Set(ref _codeBorder, value); }

    [Category("Colors")]
    [Description("Left accent stripe color for blockquotes.")]
    public Brush BlockquoteAccent { get => _blockquoteAccent; set => Set(ref _blockquoteAccent, value); }

    [Category("Colors")]
    [Description("Background color for blockquotes.")]
    public Brush BlockquoteBackground { get => _blockquoteBackground; set => Set(ref _blockquoteBackground, value); }

    [Category("Colors")]
    [Description("Link text color.")]
    public Brush LinkForeground { get => _linkForeground; set => Set(ref _linkForeground, value); }

    [Category("Colors")]
    [Description("Table cell border color.")]
    public Brush TableBorder { get => _tableBorder; set => Set(ref _tableBorder, value); }

    [Category("Colors")]
    [Description("Table header row background color.")]
    public Brush TableHeaderBackground { get => _tableHeaderBackground; set => Set(ref _tableHeaderBackground, value); }

    [Category("Colors")]
    [Description("Alternating row background color for table rows.")]
    public Brush TableRowAlternateBackground { get => _tableRowAlternateBackground; set => Set(ref _tableRowAlternateBackground, value); }

    [Category("Colors")]
    [Description("Horizontal rule stroke color.")]
    public Brush HrStroke { get => _hrStroke; set => Set(ref _hrStroke, value); }

    [Category("Code")]
    [Description("Border width for code blocks.")]
    public double CodeBorderThickness { get => _codeBorderThickness; set => Set(ref _codeBorderThickness, value); }

    [Category("Code")]
    [Description("Corner radius for code blocks.")]
    public Microsoft.UI.Xaml.CornerRadius CodeCornerRadius { get => _codeCornerRadius; set => Set(ref _codeCornerRadius, value); }

    [Category("Code")]
    [Description("Corner radius for inline code spans.")]
    public Microsoft.UI.Xaml.CornerRadius InlineCodeCornerRadius { get => _inlineCodeCornerRadius; set => Set(ref _inlineCodeCornerRadius, value); }

    [Category("Blockquote")]
    [Description("Corner radius for blockquote containers.")]
    public Microsoft.UI.Xaml.CornerRadius BlockquoteCornerRadius { get => _blockquoteCornerRadius; set => Set(ref _blockquoteCornerRadius, value); }

    [Category("Misc")]
    [Description("Maximum width for embedded images.")]
    public double MaxImageWidth { get => _maxImageWidth; set => Set(ref _maxImageWidth, value); }

    [Category("Misc")]
    [Description("Extra CSS injected into the WebView2 renderer.")]
    public string CustomCss { get => _customCss; set => Set(ref _customCss, value); }

    public MarkdownTheme ToTheme(bool tracksSystem)
    {
        return new MarkdownTheme
        {
            BodyFont = BodyFont,
            CodeFont = CodeFont,
            BodyFontSize = BodyFontSize,
            HeadingSizes = [H1Size, H2Size, H3Size, H4Size, H5Size, H6Size],
            HeadingFontWeights = [H1Weight, H2Weight, H3Weight, H4Weight, H5Weight, H6Weight],
            HeadingSpacingBefore = HeadingSpacingBefore,
            HeadingSpacingAfter = HeadingSpacingAfter,
            HeadingBorder = HeadingBorder,
            HeadingBorderThicknesses = [H1BorderThickness, H2BorderThickness, 0, 0, 0, 0],
            HeadingBorderPaddingEm = HeadingBorderPaddingEm,
            ParagraphSpacing = ParagraphSpacing,
            BlockSpacing = BlockSpacing,
            ListSpacing = ListSpacing,
            Background = Background,
            Foreground = Foreground,
            CodeBackground = CodeBackground,
            CodeForeground = CodeForeground,
            CodeBorder = CodeBorder,
            CodeBorderThickness = CodeBorderThickness,
            CodeCornerRadius = CodeCornerRadius,
            InlineCodeCornerRadius = InlineCodeCornerRadius,
            BlockquoteAccent = BlockquoteAccent,
            BlockquoteBackground = BlockquoteBackground,
            BlockquoteCornerRadius = BlockquoteCornerRadius,
            LinkForeground = LinkForeground,
            TableBorder = TableBorder,
            TableHeaderBackground = TableHeaderBackground,
            TableRowAlternateBackground = TableRowAlternateBackground,
            HrStroke = HrStroke,
            MaxImageWidth = MaxImageWidth,
            CustomCss = CustomCss,
            TracksSystemTheme = tracksSystem
        };
    }

    public void LoadFrom(MarkdownTheme theme)
    {
        _bodyFont = theme.BodyFont;
        _codeFont = theme.CodeFont;
        _bodyFontSize = theme.BodyFontSize;
        _h1Size = theme.HeadingSizes.ElementAtOrDefault(0, 32);
        _h2Size = theme.HeadingSizes.ElementAtOrDefault(1, 28);
        _h3Size = theme.HeadingSizes.ElementAtOrDefault(2, 24);
        _h4Size = theme.HeadingSizes.ElementAtOrDefault(3, 20);
        _h5Size = theme.HeadingSizes.ElementAtOrDefault(4, 18);
        _h6Size = theme.HeadingSizes.ElementAtOrDefault(5, 16);
        _h1Weight = theme.HeadingFontWeights.ElementAtOrDefault(0, FontWeights.SemiBold);
        _h2Weight = theme.HeadingFontWeights.ElementAtOrDefault(1, FontWeights.SemiBold);
        _h3Weight = theme.HeadingFontWeights.ElementAtOrDefault(2, FontWeights.SemiBold);
        _h4Weight = theme.HeadingFontWeights.ElementAtOrDefault(3, FontWeights.SemiBold);
        _h5Weight = theme.HeadingFontWeights.ElementAtOrDefault(4, FontWeights.SemiBold);
        _h6Weight = theme.HeadingFontWeights.ElementAtOrDefault(5, FontWeights.SemiBold);
        _headingSpacingBefore = theme.HeadingSpacingBefore;
        _headingSpacingAfter = theme.HeadingSpacingAfter;
        _headingBorder = theme.HeadingBorder;
        _h1BorderThickness = theme.HeadingBorderThicknesses.ElementAtOrDefault(0, 0d);
        _h2BorderThickness = theme.HeadingBorderThicknesses.ElementAtOrDefault(1, 0d);
        _headingBorderPaddingEm = theme.HeadingBorderPaddingEm;
        _paragraphSpacing = theme.ParagraphSpacing;
        _blockSpacing = theme.BlockSpacing;
        _listSpacing = theme.ListSpacing;
        _background = theme.Background;
        _foreground = theme.Foreground;
        _codeBackground = theme.CodeBackground;
        _codeForeground = theme.CodeForeground;
        _codeBorder = theme.CodeBorder;
        _codeBorderThickness = theme.CodeBorderThickness;
        _codeCornerRadius = theme.CodeCornerRadius;
        _inlineCodeCornerRadius = theme.InlineCodeCornerRadius;
        _blockquoteAccent = theme.BlockquoteAccent;
        _blockquoteBackground = theme.BlockquoteBackground;
        _blockquoteCornerRadius = theme.BlockquoteCornerRadius;
        _linkForeground = theme.LinkForeground;
        _tableBorder = theme.TableBorder;
        _tableHeaderBackground = theme.TableHeaderBackground;
        _tableRowAlternateBackground = theme.TableRowAlternateBackground;
        _hrStroke = theme.HrStroke;
        _maxImageWidth = theme.MaxImageWidth;
        _customCss = theme.CustomCss;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
    }

    void Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    static SolidColorBrush Solid(string hex)
    {
        var h = hex.TrimStart('#');
        if (h.Length == 6) h = "FF" + h;
        var parsed = uint.Parse(h, System.Globalization.NumberStyles.HexNumber);
        var color = Windows.UI.Color.FromArgb(
            (byte)(parsed >> 24), (byte)(parsed >> 16), (byte)(parsed >> 8), (byte)parsed);
        return new SolidColorBrush(color);
    }
}

file static class ArrayExtensions
{
    public static T ElementAtOrDefault<T>(this T[] array, int index, T defaultValue) =>
        index < array.Length ? array[index] : defaultValue;
}
