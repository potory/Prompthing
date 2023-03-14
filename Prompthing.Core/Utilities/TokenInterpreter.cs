using Prompthing.Core.Abstract;
using Prompthing.Core.Entities;
using Prompthing.Core.Templates.Nodes;
using Prompthing.Core.Templates.Nodes.Basic;
using SonScript.Core;

namespace Prompthing.Core.Utilities;

/// <summary>
/// Provides functionality to interpret and resolve tokens in strings.
/// </summary>
public class TokenInterpreter
{
    private readonly IReferencePool _referencePool;
    private readonly FunctionParser _parser;

    /// <summary>
    /// Initializes a new instance of the TokenInterpreter class.
    /// </summary>
    /// <param name="referencePool">The reference pool used to create references to categories and templates.</param>
    /// <param name="factory">TODO: add parameter</param>
    public TokenInterpreter(IReferencePool referencePool, FunctionFactory factory)
    {
        _referencePool = referencePool;
        _parser = new FunctionParser(factory);
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

        if (IsCategoryToken(token))
        {
            return ResolveCategoryToken(token.Substring(2, token.Length-4));
        }

        if (token.StartsWith('#'))
        {
            var func = _parser.Parse(token);
            return new SonScriptNode(func);
        }

        throw new ArgumentException("Interpretable token must be longer then 4");
    }

    /// <summary>
    /// Checks if the given token value is an escaped string by checking if it starts and ends with double quotes.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>True if the token value is an escaped string, false otherwise.</returns>
    private static bool IsEscapedString(string tokenValue) => 
        tokenValue.StartsWith('"') && tokenValue.EndsWith('"');

    /// <summary>
    /// Determines if the given token is plain text.
    /// </summary>
    /// <param name="token">The token to check.</param>
    /// <returns>true if the token is plain text; otherwise, false.</returns>
    private static bool IsPlainText(string token) => 
        !string.IsNullOrEmpty(token) && !(token.StartsWith("{{") && token.EndsWith("}}")) && !token.StartsWith('#');

    /// <summary>
    /// Extracts the value of a token.
    /// </summary>
    /// <param name="token">The token to extract the value from.</param>
    /// <returns>The value of the token.</returns>
    public static string ExtractTokenValue(string token) => 
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
}