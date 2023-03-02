using System.Text.RegularExpressions;
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

        return InterpretTokenValue(tokenValue);
    }

    /// <summary>
    /// Interprets a token value by trimming it, checking if it is an escaped string, category token, template token, loop token, or random token, resolving it accordingly, and returning the resulting BasicNode.
    /// </summary>
    /// <param name="tokenValue">The token value to interpret.</param>
    /// <returns>A BasicNode representing the interpretation of the token value.</returns>
    /// <exception cref="ArgumentException">Thrown if the token value is not a valid token.</exception>
    private BasicNode InterpretTokenValue(string tokenValue)
    {
        tokenValue = tokenValue.Trim();

        if (IsEscapedString(tokenValue))
            return new TextNode(tokenValue.Substring(1, tokenValue.Length - 2));

        if (IsCategoryToken(tokenValue))
            return ResolveCategoryToken(tokenValue);

        if (IsTemplateToken(tokenValue))
            return ResolveTemplateToken(tokenValue);

        if (IsLoopToken(tokenValue))
            return ResolveLoopToken(tokenValue);

        if (IsRandomToken(tokenValue))
            return ResolveRandomToken(tokenValue);
        
        if (IsBackspaceNode(tokenValue))
            return ResolveBackspaceNode(tokenValue);

        if (IsWrapperNode(tokenValue))
            return ResolveWrapperNode(tokenValue);

        throw new ArgumentException($"Invalid token: {tokenValue}");
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
        !string.IsNullOrEmpty(token) && !token.StartsWith("{{") && !token.EndsWith("}}");

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

    /// <summary>
    /// Determines if the given token value represents a template.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>true if the token value represents a template; otherwise, false.</returns>
    private static bool IsTemplateToken(string tokenValue) => 
        !string.IsNullOrEmpty(tokenValue) && (tokenValue.StartsWith("#t:") || tokenValue.StartsWith("#template:"));

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

    /// <summary>
    /// Checks if the given token value is a loop token by checking if it starts with "#l:" or "#loop:".
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>True if the token value is a loop token, false otherwise.</returns>
    private static bool IsLoopToken(string tokenValue) => 
        !string.IsNullOrEmpty(tokenValue) && (tokenValue.StartsWith("#l:") || tokenValue.StartsWith("#loop:"));

    /// <summary>
    /// Resolves a loop token by parsing its arguments, determining the number of iterations, interpreting the token value, and returning a LoopNode.
    /// </summary>
    /// <param name="tokenValue">The token value to resolve.</param>
    /// <returns>A LoopNode representing the loop.</returns>
    private LoopNode ResolveLoopToken(string tokenValue)
    {
        var arguments = GetArguments(tokenValue);
        var iterations = arguments[0];
        
        var (minIterations, maxIterations) = LoopIterations(iterations);
        var node = InterpretTokenValue(arguments[1]);

        return new LoopNode(minIterations, maxIterations, node);
    }

    /// <summary>
    /// Checks if the given token value is a random token by checking if it starts with "#r:" or "#random:".
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>True if the token value is a random token, false otherwise.</returns>
    private static bool IsRandomToken(string tokenValue) => 
        !string.IsNullOrEmpty(tokenValue) && (tokenValue.StartsWith("#r:") || tokenValue.StartsWith("#random:"));

    /// <summary>
    /// Resolves a random token by interpreting its arguments, creating a BasicNode for each one, and returning a RandomNode containing the BasicNodes.
    /// </summary>
    /// <param name="tokenValue">The token value to resolve.</param>
    /// <returns>A RandomNode representing the random choice.</returns>
    private RandomNode ResolveRandomToken(string tokenValue)
    {
        var arguments = GetArguments(tokenValue);
        var nodes = new BasicNode[arguments.Length];

        for (int index = 0; index < nodes.Length; index++)
        {
            nodes[index] = InterpretTokenValue(arguments[index]);
        }

        return new RandomNode(nodes);
    }

    /// <summary>
    /// Determines whether the given token value represents a backspace node.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>true if the token value represents a backspace node; otherwise, false.</returns>
    private static bool IsBackspaceNode(string tokenValue) =>
        !string.IsNullOrEmpty(tokenValue) && (tokenValue.StartsWith("#b:") || tokenValue.StartsWith("#backspace:"));

    /// <summary>
    /// Resolves a backspace node and returns the corresponding BasicNode.
    /// </summary>
    /// <param name="tokenValue">The backspace node token to resolve.</param>
    /// <returns>A BasicNode representing the resolved backspace node.</returns>
    private static BasicNode ResolveBackspaceNode(string tokenValue)
    {
        var arguments = GetArguments(tokenValue);

        if (arguments.Length == 0)
        {
            throw new ArgumentException("Token value must have at least one argument");
        }

        if (!int.TryParse(arguments[0], out int value))
        {
            throw new ArgumentException("Backspace node argument must be an integer");
        }

        return new BackspaceNode(value);
    }

    /// <summary>
    /// Determines whether the given token value represents a wrapper node.
    /// </summary>
    /// <param name="tokenValue">The token value to check.</param>
    /// <returns>true if the token value represents a wrapper node; otherwise, false.</returns>
    private bool IsWrapperNode(string tokenValue) =>
        !string.IsNullOrEmpty(tokenValue) && (tokenValue.StartsWith("#w:") || tokenValue.StartsWith("#wrapper:"));

    /// <summary>
    /// Resolves a wrapper node and returns the corresponding BasicNode.
    /// </summary>
    /// <param name="tokenValue">The wrapper node token to resolve.</param>
    /// <returns>A BasicNode representing the resolved wrapper node.</returns>
    private BasicNode ResolveWrapperNode(string tokenValue)
    {
        var arguments = GetArguments(tokenValue);

        if (arguments.Length != 1)
        {
            throw new ArgumentException("Wrapper must have only one argument");
        }

        var argument = arguments.Single();

        int parenthesisIndex = argument.IndexOf('(');

        if (parenthesisIndex == -1)
        {
            throw new ArgumentException("Wrapper node argument must have parentheses");
        }

        string wrapperName = argument[..parenthesisIndex];
        string nodeName = argument.Substring(parenthesisIndex + 1, argument.Length - (parenthesisIndex + 2));

        if (string.IsNullOrEmpty(wrapperName))
        {
            throw new ArgumentException("Wrapper node wrapper name cannot be null or empty");
        }

        if (string.IsNullOrEmpty(nodeName))
        {
            throw new ArgumentException("Wrapper node node name cannot be null or empty");
        }

        var node = InterpretTokenValue(nodeName);
        var wrapper = _referencePool.CreateReference<Wrapper>(wrapperName);

        return new WrapperNode(node, wrapper);
    }

    /// <summary>
    /// Gets the arguments for a token by extracting the substring after the first colon and splitting it by semicolons.
    /// </summary>
    /// <param name="tokenValue">The token value to extract arguments from.</param>
    /// <returns>An array of strings representing the arguments.</returns>
    private static string[] GetArguments(string tokenValue)
    {
        var argsIndex = tokenValue.IndexOf(':') + 1;
        var arguments = tokenValue.Substring(argsIndex, tokenValue.Length - argsIndex).Split(';');

        return arguments;
    }

    /// <summary>
    /// Determines the minimum and maximum number of iterations for a loop based on the given iterations argument.
    /// </summary>
    /// <param name="iterations">The iterations argument to parse.</param>
    /// <returns>A tuple containing the minimum and maximum number of iterations.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the iterations argument is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if the iterations argument is not in the correct format or if the minimum iterations value is greater than the maximum iterations value.</exception>
    private static (int minIterations, int maxIterations) LoopIterations(string iterations)
    {
        if (string.IsNullOrEmpty(iterations))
        {
            throw new ArgumentNullException(nameof(iterations), "Iterations argument cannot be null or empty.");
        }

        if (!Regex.IsMatch(iterations, @"^\d+(-\d+)?$"))
        {
            throw new ArgumentException("Iterations argument is not in the correct format. Please enter a positive integer or a range of positive integers separated by a dash (-).");
        }

        int minIterations, maxIterations;

        var sep = iterations.Split('-');

        if (sep.Length == 1)
        {
            if (!int.TryParse(iterations, out minIterations))
            {
                throw new ArgumentException("Iterations argument is not a valid integer value.", nameof(iterations));
            }
            maxIterations = minIterations;
        }
        else
        {
            if (!int.TryParse(sep[0], out minIterations))
            {
                throw new ArgumentException("Minimum iterations value is not a valid integer value.", nameof(iterations));
            }
            if (!int.TryParse(sep[1], out maxIterations))
            {
                throw new ArgumentException("Maximum iterations value is not a valid integer value.", nameof(iterations));
            }

            if (minIterations > maxIterations)
            {
                throw new ArgumentException("Minimum iterations value cannot be greater than the maximum iterations value.", nameof(iterations));
            }
        }

        return (minIterations, maxIterations);
    }
}