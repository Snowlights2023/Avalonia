﻿using System.Runtime.CompilerServices;

namespace Avalonia.Media.TextFormatting.Unicode
{
    /// <summary>
    ///     Helper for looking up unicode character class information
    /// </summary>
    internal static class UnicodeData
    {
        internal const int CATEGORY_BITS = 6;
        internal const int SCRIPT_BITS = 8;
        internal const int LINEBREAK_BITS = 6;

        internal const int BIDIPAIREDBRACKED_BITS = 16;
        internal const int BIDIPAIREDBRACKEDTYPE_BITS = 2;
        internal const int BIDICLASS_BITS = 5;

        internal const int SCRIPT_SHIFT = CATEGORY_BITS;
        internal const int LINEBREAK_SHIFT = CATEGORY_BITS + SCRIPT_BITS;

        internal const int BIDIPAIREDBRACKEDTYPE_SHIFT = BIDIPAIREDBRACKED_BITS;
        internal const int BIDICLASS_SHIFT = BIDIPAIREDBRACKED_BITS + BIDIPAIREDBRACKEDTYPE_BITS;

        internal const int CATEGORY_MASK = (1 << CATEGORY_BITS) - 1;
        internal const int SCRIPT_MASK = (1 << SCRIPT_BITS) - 1;
        internal const int LINEBREAK_MASK = (1 << LINEBREAK_BITS) - 1;

        internal const int BIDIPAIREDBRACKED_MASK = (1 << BIDIPAIREDBRACKED_BITS) - 1;
        internal const int BIDIPAIREDBRACKEDTYPE_MASK = (1 << BIDIPAIREDBRACKEDTYPE_BITS) - 1;
        internal const int BIDICLASS_MASK = (1 << BIDICLASS_BITS) - 1;

        /// <summary>
        /// Gets the <see cref="GeneralCategory"/> for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's general category.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GeneralCategory GetGeneralCategory(uint codepoint)
        {
            return (GeneralCategory)(UnicodeDataTrie.Trie.Get(codepoint) & CATEGORY_MASK);
        }

        /// <summary>
        /// Gets the <see cref="Script"/> for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's script.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Script GetScript(uint codepoint)
        {
            return (Script)((UnicodeDataTrie.Trie.Get(codepoint) >> SCRIPT_SHIFT) & SCRIPT_MASK);
        }

        /// <summary>
        /// Gets the <see cref="BidiClass"/> for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's biDi class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BidiClass GetBiDiClass(uint codepoint)
        {
            return (BidiClass)((BidiTrie.Trie.Get(codepoint) >> BIDICLASS_SHIFT) & BIDICLASS_MASK);
        }

        /// <summary>
        /// Gets the <see cref="BidiPairedBracketType"/> for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's paired bracket type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BidiPairedBracketType GetBiDiPairedBracketType(uint codepoint)
        {
            return (BidiPairedBracketType)((BidiTrie.Trie.Get(codepoint) >> BIDIPAIREDBRACKEDTYPE_SHIFT) & BIDIPAIREDBRACKEDTYPE_MASK);
        }

        /// <summary>
        /// Gets the paired bracket for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's paired bracket.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Codepoint GetBiDiPairedBracket(uint codepoint)
        {
            return new Codepoint(BidiTrie.Trie.Get(codepoint) & BIDIPAIREDBRACKED_MASK);
        }

        /// <summary>
        /// Gets the line break class for a Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's line break class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LineBreakClass GetLineBreakClass(uint codepoint)
        {
            return (LineBreakClass)((UnicodeDataTrie.Trie.Get(codepoint) >> LINEBREAK_SHIFT) & LINEBREAK_MASK);
        }

        /// <summary>
        /// Gets the grapheme break type for the Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's grapheme break type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GraphemeBreakClass GetGraphemeClusterBreak(uint codepoint)
        {
            return (GraphemeBreakClass)GraphemeBreakTrie.Trie.Get(codepoint);
        }

        /// <summary>
        /// Gets the EastAsianWidth class for the Unicode codepoint.
        /// </summary>
        /// <param name="codepoint">The codepoint in question.</param>
        /// <returns>The code point's EastAsianWidth class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EastAsianWidthClass GetEastAsianWidthClass(uint codepoint)
        {
            return (EastAsianWidthClass)EastAsianWidthTrie.Trie.Get(codepoint);
        }
    }
}
