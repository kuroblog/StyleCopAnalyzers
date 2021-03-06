﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp8.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.ReadabilityRules.SA1118ParameterMustNotSpanMultipleLines>;

    public class SA1118CSharp9UnitTests : SA1118CSharp8UnitTests
    {
        [Fact]
        [WorkItem(3314, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3314")]
        public async Task TestWithExpressionAsync()
        {
            var testCode = @"
class Foo
{
    public record R(int X, int Y);

    public void FunA(params object[] j)
    {
    }

    public void FunB(R r)
    {
        FunA(
            1,
            r with
            {
                X = 1,
            });
    }
}";

            await new CSharpTest(LanguageVersion.CSharp9)
            {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
                TestCode = testCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        [WorkItem(3314, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3314")]
        public async Task TestWithExpression2Async()
        {
            var testCode = @"
class Foo
{
    public record R(int X, int Y);

    public void FunA(params object[] j)
    {
    }

    public void FunB(R r)
    {
        FunA(
            1,
            r with
            {
                X = 1,
            },
            2);
    }
}";

            await new CSharpTest(LanguageVersion.CSharp9)
            {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
                TestCode = testCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
