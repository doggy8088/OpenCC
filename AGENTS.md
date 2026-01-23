# Repository Guidelines

## Project Structure & Module Organization
- `src/OpenCC/` contains the library source. Core types live at the root, with `Internal/` for data and preset wiring and `Presets/` for user-facing preset helpers.
- `tests/OpenCC.Tests/` contains xUnit tests that mirror the public surface area and internal data helpers.
- `README.md` is kept at the repo root and packed into the NuGet package.
- `bin/` and `obj/` are build outputs; do not commit changes there.

## Build, Test, and Development Commands
- `dotnet build src/OpenCC/OpenCC.csproj` — builds the library (net8.0).
- `dotnet test tests/OpenCC.Tests/OpenCC.Tests.csproj` — runs the full test suite.
- `dotnet pack src/OpenCC/OpenCC.csproj -c Release` — produces the NuGet package (also generated on build).

## Coding Style & Naming Conventions
- C# with nullable reference types enabled and implicit usings; keep null checks explicit where needed.
- Indentation: 4 spaces; braces on new lines, consistent with existing files.
- Public types/members: `PascalCase`; locals/parameters: `camelCase`; private fields: `_camelCase`.
- File names should match the primary type (e.g., `Trie.cs` for `Trie`).

## Testing Guidelines
- Framework: xUnit.
- Test files end with `*Tests.cs` and live under `tests/OpenCC.Tests/`.
- Use the pattern `Method_Scenario_Expected` for test names.
- Aim to add/extend tests for each modified class, including internal helpers when behavior changes.

## Commit & Pull Request Guidelines
- No Git history is available in this workspace to infer conventions. Use concise, imperative summaries or Conventional Commits (e.g., `fix: handle empty input` or `feat: add locale preset`).
- PRs should include: a clear description, relevant test command output, and links to issues if applicable.
- If behavior changes, note any breaking changes and update tests accordingly.

## Buildability Requirement
- Every edit must keep the repository buildable. When making changes, ensure `dotnet build OpenCC.sln` succeeds (or the equivalent project build if the solution is unavailable).
