using Prompthing.Core.Entities;
using Prompthing.Core.Templates;
using Prompthing.Core.Templates.Nodes;
using Prompthing.Core.Templates.Nodes.Basic;

namespace Prompthing.Core;

/// <summary>
/// Provides functionality to interpret and resolve tokens in strings.
/// </summary>
public class TokenInterpreter
{
    private readonly ReferencePool _referencePool;

    /// <summary>
    /// Initializes a new instance of the TokenInterpreter class.
    /// </summary>
    /// <param name="referencePool">The reference pool used to create references to categories and templates.</param>
    public TokenInterpreter(ReferencePool referencePool)
    {
        _referencePool = referencePool;
    }

    /// <summary>
    /// Resolves a token and returns the corresponding BasicNode.
    /// </summary>
    /// <param name="token">The token to resolve.</param>
    /// <returns>A BasicNode representing the resolved token.</returns>
    public BasicNode Interpret(string token)
    {
        if (IsPlainText(token))
        {
            return new TextNode(token);
        }

        string tokenValue = ExtractTokenValue(token);

        if (IsCategoryToken(tokenValue))
        {
            return ResolveCategoryToken(tokenValue);
        }
        
        if (IsTemplateToken(tokenValue))
        {
            return ResolveTemplateToken(tokenValue);
        }

        throw new ArgumentException($"Invalid token: {token}");
    }

    /// <summary>
    /// Determines if the given token is plain text.
    /// </summary>
    /// <param name="token">The token to check.</param>
    /// <returns>true if the token is plain text; otherwise, false.</returns>
    private static bool IsPlainText(string token) => 
        !string.IsNullOrEmpty(token) && !token.StartsWith("{{") && !token.EndsWith("}}");

    /// <summary>
    /// Extracts the value of a token.
    /// </summary>
    /// <param name="token">The token to extract the value from.</param>
    /// <returns>The value of the token.</returns>
    private static string ExtractTokenValue(string token) => 
        token.Substring(2, token.Length - 4);

    /// <summary>
    /// Determines if the given token value represents a category.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>true if the token value represents a category; otherwise, false.</returns>
    private static bool IsCategoryToken(string tokenValue) => 
        !string.IsNullOrEmpty(tokenValue) && tokenValue[0] != '#';

    /// <summary>
    /// Resolves a category token and returns the corresponding CategoryNode.
    /// </summary>
    /// <param name="tokenValue">The category token to resolve.</param>
    /// <returns>A CategoryNode representing the resolved category token.</returns>
    private CategoryNode ResolveCategoryToken(string tokenValue) =>
        new(_referencePool.CreateReference<Category>(tokenValue));

    /// <summary>
    /// Determines if the given token value represents a template.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>true if the token value represents a template; otherwise, false.</returns>
    private static bool IsTemplateToken(string tokenValue) => 
        !string.IsNullOrEmpty(tokenValue) && tokenValue.StartsWith("#template:");

    /// <summary>
    /// Resolves a template token and returns the corresponding TemplateNode.
    /// </summary>
    /// <param name="tokenValue">The template token to resolve.</param>
    /// <returns>A TemplateNode representing the resolved template token.</returns>
    private TemplateNode ResolveTemplateToken(string tokenValue)
    {
        var segments = tokenValue.Split(':');
        var reference = _referencePool.CreateReference<Template>(segments[1]);

        return new TemplateNode(reference);
    }
}