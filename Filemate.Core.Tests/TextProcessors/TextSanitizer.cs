using Xunit.Abstractions;
using Filemate.Core.Renamer.TextProcessors;

namespace Filemate.Tests.TextProcessors
{
    public class TextSanitizerTests
    {
        private readonly ITestOutputHelper _output;

        public TextSanitizerTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void TextSanitizer_ShouldRemoveBadValues()
        {
            List<string> badTexts = new List<string>
            {
                "My Vacation Photos 🌴☀️📸",
                "Secret Papyrus 📜𓀀𓁶",
                "Results [∆(π)≈3.14]",
                "R̴e̵p̴o̴r̴t̴ ̴f̴r̴o̴m̴ ̴t̴h̴e̴ ̴V̴o̴i̴d̸",
                "Ｉｍｐｏｒｔａｎｔ Ｄｏｃｕｍｅｎｔ",
            };

            foreach (string badText in badTexts)
            {
                string sanitized = TextSanitizer.Sanitize(badText);
                _output.WriteLine(badText);
                _output.WriteLine(sanitized);
                _output.WriteLine(badText.Normalize( System.Text.NormalizationForm.FormKC));
                _output.WriteLine("---");
            }

        }
    }
}
