namespace Caero;

// Lay out the complete page before applying the typewriter reveal, so a word
// never jumps to the next line as its remaining letters become visible.
internal static class TextLayout
{
    internal readonly record struct Glyph(int Index, float X, int Row);

    internal static IReadOnlyList<Glyph> Arrange(string text, float width, Func<char, float> advance)
    {
        var result = new List<Glyph>(text.Length);
        float x = 0;
        int row = 0;
        for (int i = 0; i < text.Length;)
        {
            if (text[i] == '\n') { x = 0; row++; i++; continue; }
            if (char.IsWhiteSpace(text[i]))
            {
                // Spaces at a wrapped line boundary need not be drawn.
                if (x > 0) x += advance(text[i]);
                i++;
                continue;
            }
            int end = i;
            float wordWidth = 0;
            while (end < text.Length && !char.IsWhiteSpace(text[end])) wordWidth += advance(text[end++]);
            if (x > 0 && x + wordWidth > width) { x = 0; row++; }
            while (i < end)
            {
                float step = advance(text[i]);
                // A single overlong word still has to fit within the available width.
                if (x > 0 && x + step > width) { x = 0; row++; }
                result.Add(new(i++, x, row));
                x += step;
            }
        }
        return result;
    }
}
