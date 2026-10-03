# GOTCHA Spec: .NET modernization and measured optimization

## Goals
Upgrade every C# project to stable .NET 10 / C# 14, then retain only measured optimizations that preserve algorithm behavior.

## Objectives
- Build every solution in Debug and Release with SDK 10.0.401.
- Execute real correctness checks; empty tests do not establish correctness.
- Compare each optimization independently against its unchanged baseline, including allocations and elapsed time.
- Require exact equality where arithmetic order is unchanged; otherwise require absolute or relative error at most 1e-12 and report maximum observed error.
- Commit and push the verified modernization before the optimization pass; report retained and rejected candidates.

## Tasks
Trigger: requested modernization. Termination: verified builds, numerical evidence, published commits, and review report.

## Capabilities
Tools: .NET SDK, Git, official Microsoft documentation, isolated comparison harnesses.
Off-limits: unrelated refactors, changes to algorithm formulas or RNG distributions for performance.

## Health
No requested time or cost budget. Review failures before retrying; record unavailable platform checks explicitly.

## Attributes
Preserve objective functions, evaluation order, random draw order, bounds, quantization, and native calling conventions.
Never claim a build or empty test proves convergence or ABI compatibility.

## Constraints
Linux ARM64 host; Windows-specific integrations need separate platform evidence.
The required standards library currently has no C#/.NET coding standard; this absence was reported to the operator.

## Users
Maintainers and consumers of the particle swarm libraries and examples.

## Runtime
Working state is local; persist specs, commands, evidence, and reports in this repository.
Re-plan from build/test evidence; exit only when requested deliverables are verified.

## Beliefs & Intentions
Current stable Microsoft release: .NET 10.0.12, SDK 10.0.401, C# 14.
Use framework-default language selection and pin the stable SDK.
Reference: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
Language mapping: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-versioning
