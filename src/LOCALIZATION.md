# English edition

## Translation policy

- Use **sacred** consistently for the primality metaphor: sacred numbers, sacred stones, sacred power, sacred knights. Do not alternate with holy, divine, or blessed for the same property. Keep "prime" out of hints that originally conceal the mathematical term.
- Inscriptions use measured, poetic language; residents speak conversationally. Preserve weary voices, humor, ambiguity, and deliberate fourth-wall remarks.
- Preserve numerical clues, page boundaries, answer branches, object IDs, island numbers, placement, and ability values. Do not shorten translations to fit the current UI.
- Translate 十六夜 as "the moon of the sixteenth night". This retains the number sixteen and the lunar image without requiring Japanese vocabulary or changing the destination.
- Use "the elder twin" for the smaller member. The existing natural-number explanation establishes that smaller numbers appear first.
- Adapt the reversed Japanese phrase to the reversed English phrase "Srebmun dercas." ("sacred numbers" backward).
- The resident discussing inconsistent Japanese synonyms now discusses "sacred numbers, sacred stones, sacred power". This preserves the hint that different images refer to one property while honoring the English terminology policy.
- For the stone dice clue, "one measure greater along each edge" preserves the first-kind construction and its contrast with "two" in the resident's hint. The removal is an equal **volume**, not an equal edge length. Do not name Cuban primes directly when the original does not.
- Knights of Prayer and Knights of Succession retain the two distinct families in the final puzzle. The Million Gate is strictly below one million; joining their powers means addition, as the resident explains.

The runtime loads `CAERO/Content/dialogue.en.json` and `CAERO/Content/special-islands.en.json`, including when reading `Content.dat`. Japanese originals remain as translation references. The dialogue language must be `en`. Names allow up to 128 characters; authored page groups retain a 20,000-character safety budget instead of the original 180-character limit per regular dialogue page. This is a data validation limit, not a promise that the text fits on screen.

The UI prefers Segoe UI, then Arial, then Tahoma from Windows. If none is available, it uses raylib's built-in font. No Japanese Windows font is required. Current English text uses printable ASCII and explicit newlines.

## Deferred visual review

Clipping, panel height, text size, control-screen layout, and translated page length require a later visual review. Translation accuracy takes priority over fitting text into the existing frames. Word wrapping alone does not guarantee that a page fits vertically.
