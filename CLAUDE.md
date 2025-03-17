# KSharp Development Guide

## Build Commands
```
# Build solution
dotnet build KSharp.sln

# Run tests
dotnet test KSharp.sln

# Run single test
dotnet test --filter "FullyQualifiedName=KSharp.Tests.Parsing.KsSyntaxReaderTest.ParseSum"
dotnet test --filter "FullyQualifiedName=KSharp.Tests.IR.SyntaxTransformerTest.Transform_VarsProject"
```

## Code Style Guidelines
- **Naming**: PascalCase for types/members, _camelCase for fields, camelCase for params/locals
- **Indentation**: 4 spaces, max line length 100
- **Always use var** for variables (enforced with error severity)
- **Braces**: Always on new line
- **Expression bodies**: Required for properties/accessors, discouraged for methods
- **Fields**: Private fields prefixed with underscore (_fieldName)
- **Pattern matching**: Preferred over is/as expressions
- **Ordering**: System namespaces first, then ordered alphabetically
- **Error handling**: Use throw expressions where appropriate

## Special Terms
- "IR" is recognized as a word in naming (Intermediate Representation)