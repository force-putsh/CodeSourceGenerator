using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CodeSourceGeneratorLib
{
    [Generator]
    public class RepositoryDependencyInjectionGenerator:ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            var attributeSymbol = compilation.GetTypeByMetadataName("CodeSourceGeneratorLib.RepositoryDependencyAttribute");
            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine("using System;");
            sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sourceBuilder.AppendLine("namespace CodeSourceGeneratorLib");
            sourceBuilder.AppendLine("{");
            sourceBuilder.AppendLine("    public static class RepositoryDependencyInjection");
            sourceBuilder.AppendLine("    {");
            sourceBuilder.AppendLine("        public static IServiceCollection AddRepositories(this IServiceCollection services)");
            sourceBuilder.AppendLine("        {");
            foreach (var typeSymbol in GetTypesWithAttribute(compilation, attributeSymbol))
            {
                var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
                var className = typeSymbol.Name;
                var interfaceName = $"I{className}";
                sourceBuilder.AppendLine($"            services.AddScoped<{namespaceName}.{interfaceName}, {namespaceName}.{className}>();");
            }
            sourceBuilder.AppendLine("                 return services;");
            sourceBuilder.AppendLine("        }");
            sourceBuilder.AppendLine("    }");
            sourceBuilder.AppendLine("}");
            context.AddSource("RepositoryDependencyInjection", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
        
        private static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceOrTypeSymbol namespaceSymbol)
        {
            if (namespaceSymbol is INamespaceSymbol ns)
            {
                foreach (var member in ns.GetMembers())
                {
                    foreach (var type in GetAllTypes(member))
                    {
                        yield return type;
                    }
                }
            }
            else if (namespaceSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                yield return namedTypeSymbol;
                foreach (var typeArgument in namedTypeSymbol.GetTypeMembers())
                {
                    foreach (var type in GetAllTypes(typeArgument))
                    {
                        yield return type;
                    }
                }
            }
        }
        
        private static bool HasAttribute(INamedTypeSymbol typeSymbol, INamedTypeSymbol attributeSymbol)
        {
            return typeSymbol.GetAttributes().Any(x => x.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
        }
        
        private static IEnumerable<INamedTypeSymbol> GetTypesWithAttribute(Compilation compilation, INamedTypeSymbol attributeSymbol)
        {
            return GetAllTypes(compilation.Assembly.GlobalNamespace)
                .Where(x => HasAttribute(x, attributeSymbol));
        }
    }
}