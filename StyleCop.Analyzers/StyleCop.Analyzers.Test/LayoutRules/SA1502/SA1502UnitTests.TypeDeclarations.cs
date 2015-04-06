﻿namespace StyleCop.Analyzers.Test.LayoutRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using StyleCop.Analyzers.LayoutRules;
    using TestHelper;
    using Xunit;


    /// <summary>
    /// Unit tests for the type declaration part of <see cref="SA1502ElementMustNotBeOnASingleLine"/>.
    /// </summary>
    public partial class SA1502UnitTests : CodeFixVerifier
    {
        public static IEnumerable<object[]> TokensToTest
        {
            get
            {
                yield return new[] { "class" };
                yield return new[] { "struct" };
            }
        }

        /// <summary>
        /// Verifies that a correct empty type will pass without diagnostic.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestValidEmptyType(string token)
        {
            var testCode = @"public ##PH## Foo 
{
}";

            await this.VerifyCSharpDiagnosticAsync(FormatTestCode(testCode, token), EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that an empty type defined on a single line will trigger a diagnostic.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestEmptyTypeOnSingleLine(string token)
        {
            var testCode = "public ##PH## Foo { }";

            var expected = this.CSharpDiagnostic().WithLocation(1, 13 + token.Length);
            await this.VerifyCSharpDiagnosticAsync(FormatTestCode(testCode, token), expected, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a type definition on a single line will trigger a diagnostic.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeOnSingleLine(string token)
        {
            var testCode = "public ##PH## Foo { private int bar; }";

            var expected = this.CSharpDiagnostic().WithLocation(1, 13 + token.Length);
            await this.VerifyCSharpDiagnosticAsync(FormatTestCode(testCode, token), expected, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a type with its block defined on a single line will trigger a diagnostic.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeWithBlockOnSingleLine(string token)
        {
            var testCode = @"public ##PH## Foo
{ private int bar; }";

            var expected = this.CSharpDiagnostic().WithLocation(2, 1);
            await this.VerifyCSharpDiagnosticAsync(FormatTestCode(testCode, token), expected, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a type definition with only the block start on the same line will pass without diagnostic.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeWithBlockStartOnSameLine(string token)
        {
            var testCode = @"public ##PH## Foo { 
    private int bar; 
}";

            await this.VerifyCSharpDiagnosticAsync(FormatTestCode(testCode, token), EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the code fix for an empty type element is working properly.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestEmptyTypeOnSingleLineCodeFix(string token)
        {
            var testCode = "public ##PH## Foo { }";
            var fixedTestCode = @"public ##PH## Foo
{
}
";

            await this.VerifyCSharpFixAsync(FormatTestCode(testCode, token), FormatTestCode(fixedTestCode, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the code fix for a type with a statement is working properly.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeOnSingleLineCodeFix(string token)
        {
            var testCode = "public ##PH## Foo { private int bar; }";
            var fixedTestCode = @"public ##PH## Foo
{
    private int bar;
}
";

            await this.VerifyCSharpFixAsync(FormatTestCode(testCode, token), FormatTestCode(fixedTestCode, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the code fix for a type with multiple statements is working properly.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeOnSingleLineWithMultipleStatementsCodeFix(string token)
        {
            var testCode = "public ##PH## Foo { private int bar; private bool baz; }";
            var fixedTestCode = @"public ##PH## Foo
{
    private int bar; private bool baz;
}
";

            await this.VerifyCSharpFixAsync(FormatTestCode(testCode, token), FormatTestCode(fixedTestCode, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the code fix for a type with its block defined on a single line is working properly.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeWithBlockOnSingleLineCodeFix(string token)
        {
            var testCode = @"public ##PH## Foo
{ private int bar; }";
            var fixedTestCode = @"public ##PH## Foo
{
    private int bar;
}
";

            await this.VerifyCSharpFixAsync(FormatTestCode(testCode, token), FormatTestCode(fixedTestCode, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the code fix for a type with lots of trivia is working properly.
        /// </summary>
        [Theory]
        [MemberData("TokensToTest")]
        public async Task TestTypeWithLotsOfTriviaCodeFix(string token)
        {
            var testCode = @"public ##PH## Foo /* TR1 */ { /* TR2 */ private int bar; /* TR3 */ private int baz; /* TR4 */ } /* TR5 */";
            var fixedTestCode = @"public ##PH## Foo /* TR1 */
{ /* TR2 */
    private int bar; /* TR3 */ private int baz; /* TR4 */
} /* TR5 */
";

            await this.VerifyCSharpFixAsync(FormatTestCode(testCode, token), FormatTestCode(fixedTestCode, token)).ConfigureAwait(false);
        }
    }
}
