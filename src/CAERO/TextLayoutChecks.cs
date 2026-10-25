namespace Caero;

internal static class TextLayoutChecks
{
    internal static void Run()
    {
        static void Check(bool value, string message)
        { if (!value) throw new InvalidOperationException("Text layout check failed: " + message); }
        var words = TextLayout.Arrange("one seven", 7, _ => 1);
        Check(words.Where(g => g.Index >= 4).All(g => g.Row == 1)
            && words.Single(g => g.Index == 4).X == 0, "wrap the entire second word without a leading space");
        var exact = TextLayout.Arrange("one two", 7, _ => 1);
        Check(exact.All(g => g.Row == 0), "a word that fits exactly remains on the line");
        var explicitLines = TextLayout.Arrange("one\n\ntwo", 20, _ => 1);
        Check(explicitLines.Single(g => g.Index == 5).Row == 2, "preserve authored blank lines");
        var longWord = TextLayout.Arrange("abcdefgh", 3, _ => 1);
        Check(longWord.Select(g => g.Index).SequenceEqual(Enumerable.Range(0, 8))
            && longWord.All(g => g.X < 3) && longWord[^1].Row == 2,
            "split an overlong word without losing characters");
        var proportional = TextLayout.Arrange("iii WWW", 8, c => c == 'W' ? 2 : 1);
        Check(proportional.Where(g => g.Index >= 4).All(g => g.Row == 1), "use measured glyph advances");
        // Revealing the first character of a wrapped word uses the full-page position.
        Check(words.Where(g => g.Index < 5).Last().Row == words.Last().Row,
            "the first revealed letter already occupies the final word's line");
        Console.WriteLine("TEXT LAYOUT: word boundaries, explicit breaks, long words, proportional advances and reveal stability passed.");
    }
}
