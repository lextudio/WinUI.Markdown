using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using WinUI.Markdown.Themes;

namespace WinUI.Markdown.Controls;

public sealed class MarkdownListPanel : StackPanel
{
    public MarkdownListPanel()
    {
        Spacing = 4;
    }

    public bool IsOrdered { get; set; }

    public int StartNumber { get; set; } = 1;

    public char OrderedDelimiter { get; set; } = '.';

    public int NestingLevel { get; set; }

    public MarkdownTheme? Theme { get; set; }

    internal void AddItem(FrameworkElement content, bool? isTask = null)
    {
        if (isTask.HasValue)
        {
            Children.Add(CreateTaskItem(content, isTask.Value));
            return;
        }

        var index = Children.Count + StartNumber;
        var markerWidth = IsOrdered ? 38 : 24;
        var row = new Grid
        {
            ColumnSpacing = 8,
            Margin = new Thickness(Math.Min(NestingLevel * 18, 72), 0, 0, 0)
        };
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(markerWidth) });
        row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        UIElement markerElement = IsOrdered ? CreateOrderedMarker(index) : CreateUnorderedMarker();

        row.Children.Add(markerElement);

        Grid.SetColumn(content, 1);
        row.Children.Add(content);
        Children.Add(row);
    }

    private UIElement CreateTaskItem(FrameworkElement content, bool isChecked)
    {
        return new CheckBox
        {
            IsChecked = isChecked,
            IsHitTestVisible = false,
            IsTabStop = false,
            Content = content,
            FontFamily = Theme?.BodyFont,
            FontSize = Theme?.BodyFontSize ?? 14,
            Foreground = Theme?.Foreground,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Top,
            MinWidth = 0,
            MinHeight = 0,
            Margin = new Thickness(Math.Min(NestingLevel * 18, 72), 0, 0, 0),
            Padding = new Thickness(0)
        };
    }

    private UIElement CreateOrderedMarker(int index)
    {
        return new TextBlock
        {
            Text = OrderedMarkerText(index),
            FontFamily = Theme?.BodyFont,
            FontSize = Theme?.BodyFontSize ?? 14,
            Foreground = Theme?.Foreground,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 0, 0, 0)
        };
    }

    private UIElement CreateUnorderedMarker()
    {
        var marker = new Grid
        {
            Width = 16,
            Height = Math.Max(18, Theme?.BodyFontSize * 1.5 ?? 21),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };

        marker.Children.Add(BulletForLevel(NestingLevel));
        return marker;
    }

    private Shape BulletForLevel(int level)
    {
        return (level % 3) switch
        {
            1 => new Ellipse
            {
                Width = 6,
                Height = 6,
                Stroke = Theme?.Foreground,
                StrokeThickness = 1.4,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            },
            2 => new Rectangle
            {
                Width = 5,
                Height = 5,
                Fill = Theme?.Foreground,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            },
            _ => new Ellipse
            {
                Width = 6,
                Height = 6,
                Fill = Theme?.Foreground,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            }
        };
    }

    private string OrderedMarkerText(int index)
    {
        return (NestingLevel % 3) switch
        {
            1 => $"{ToRoman(index).ToLowerInvariant()})",
            2 => $"{ToAlpha(index)})",
            _ => $"{index}{OrderedDelimiter}"
        };
    }

    private static string ToAlpha(int value)
    {
        var result = string.Empty;
        var current = Math.Max(1, value);
        while (current > 0)
        {
            current--;
            result = (char)('a' + current % 26) + result;
            current /= 26;
        }

        return result;
    }

    private static string ToRoman(int value)
    {
        ReadOnlySpan<(int Value, string Text)> numerals =
        [
            (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
            (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
        ];

        var current = Math.Clamp(value, 1, 3999);
        var result = string.Empty;
        foreach (var (number, text) in numerals)
        {
            while (current >= number)
            {
                result += text;
                current -= number;
            }
        }

        return result;
    }
}
