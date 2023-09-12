/*
 * Took from https://stackoverflow.com/questions/5565885/how-to-bind-a-textblock-to-a-resource-containing-formatted-text/7569049#7569049
 * Author: Vincent
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ViewModel.Converters
{
    public static class FormattedTextBlock
    {
        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached("FormattedText",
                typeof(string),
                typeof(FormattedTextBlock),
                new UIPropertyMetadata("", FormattedTextChanged));

        private static Inline Traverse(string value)
        {
            // Get the sections/inlines
            var sections = SplitIntoSections(value);

            // Check for grouping
            if (sections.Length.Equals(1))
            {
                var section = sections[0];

                // Check for token
                if (!GetTokenInfo(section, out var token, out var tokenStart, out var tokenEnd))
                    return new Run(section);
                // Get the content to further examination
                var content = token.Length.Equals(tokenEnd - tokenStart) ?
                    null :
                    section.Substring(token.Length, section.Length - 1 - token.Length * 2);

                switch (token.ToUpper())
                {
                    case "<B>":
                    case "<BOLD>":
                        /* <b>Bold text</b> */
                        return new Bold(Traverse(content));
                    case "<I>":
                    case "<ITALIC>":
                        /* <i>Italic text</i> */
                        return new Italic(Traverse(content));
                    case "<U>":
                    case "<UNDERLINE>":
                        /* <u>Underlined text</u> */
                        return new Underline(Traverse(content));
                    case "<BR>":
                    case "<BR/>":
                    case "<LINEBREAK/>":
                        /* Line 1<br/>line 2 */
                        return new LineBreak();
                    default:
                        return new Run(section);
                }
            }

            var span = new Span();

            foreach (var section in sections)
                span.Inlines.Add(Traverse(section));

            return span;
        }

        /// <summary>
        /// Examines the passed string and find the first token, where it begins and where it ends.
        /// </summary>
        /// <param name="value">The string to examine.</param>
        /// <param name="token">The found token.</param>
        /// <param name="startIndex">Where the token begins.</param>
        /// <param name="endIndex">Where the end-token ends.</param>
        /// <returns>True if a token was found.</returns>
        public static bool GetTokenInfo(string value, out string token, out int startIndex, out int endIndex)
        {
            token = null;
            endIndex = -1;

            startIndex = value.IndexOf("<", StringComparison.Ordinal);
            var startTokenEndIndex = value.IndexOf(">", StringComparison.Ordinal);

            // No token here
            if (startIndex < 0)
                return false;

            // No token here
            if (startTokenEndIndex < 0)
                return false;

            token = value.Substring(startIndex, startTokenEndIndex - startIndex + 1);

            // Check for closed token. E.g. <LineBreak/>
            if (token.EndsWith("/>"))
            {
                endIndex = startIndex + token.Length;
                return true;
            }

            var endToken = token.Insert(1, "/");

            // Detect nesting;
            var nesting = 0;
            var pos = 0;
            do
            {
                var tempStartTokenIndex = value.IndexOf(token, pos, StringComparison.Ordinal);
                var tempEndTokenIndex = value.IndexOf(endToken, pos, StringComparison.Ordinal);

                if (tempStartTokenIndex >= 0 && tempStartTokenIndex < tempEndTokenIndex)
                {
                    nesting++;
                    pos = tempStartTokenIndex + token.Length;
                }
                else if (tempEndTokenIndex >= 0 && nesting > 0)
                {
                    nesting--;
                    pos = tempEndTokenIndex + endToken.Length;
                }
                else // Invalid tokenized string
                    return false;

            } while (nesting > 0);

            endIndex = pos;

            return true;
        }

        /// <summary>
        /// Splits the string into sections of tokens and regular text.
        /// </summary>
        /// <param name="value">The string to split.</param>
        /// <returns>An array with the sections.</returns>
        private static string[] SplitIntoSections(string value)
        {
            var sections = new List<string>();

            while (!string.IsNullOrEmpty(value))
            {
                // Check if this is a token section
                if (GetTokenInfo(value, out _, out var tokenStartIndex, out var tokenEndIndex))
                {
                    // Add pretext if the token isn't from the start
                    if (tokenStartIndex > 0)
                        sections.Add(value.Substring(0, tokenStartIndex));

                    sections.Add(value.Substring(tokenStartIndex, tokenEndIndex - tokenStartIndex));
                    value = value.Substring(tokenEndIndex); // Trim away
                }
                else
                { // No tokens, just add the text
                    sections.Add(value);
                    value = null;
                }
            }

            return sections.ToArray();
        }

        private static void FormattedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue as string;

            if (sender is TextBlock textBlock)
                textBlock.Inlines.Add(Traverse(value));
        }
    }
}