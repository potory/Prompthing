using Newtonsoft.Json.Linq;
using Prompthing.Core.Abstract;
using Prompthing.Core.Entities;

namespace Prompthing.Core.Compilers;

/// <summary>
/// Compiles a JObject into a Category.
/// </summary>
public sealed class CategoryCompiler : ICompiler<JObject, Category>
{
    /// <summary>
    /// Compiles a JObject into a Category.
    /// </summary>
    /// <param name="obj">The JObject to compile.</param>
    /// <returns>The compiled Category.</returns>
    public Category Compile(JObject obj)
    {
        var nameToken = obj.GetValue("Name", StringComparison.OrdinalIgnoreCase);
        var valuesToken = obj.GetValue("Values", StringComparison.OrdinalIgnoreCase);

        if (valuesToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Values' property in category object.");
        }

        if (nameToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Name' property in category object.");
        }

        var valuesArray = (JArray)valuesToken;
        Term[] terms = new Term[valuesArray.Count];

        for (int i = 0; i < terms.Length; i++)
        {
            terms[i] = valuesArray[i].HasValues ? 
                CreateTerm((JObject)valuesArray[i]) : 
                new Term(valuesArray[i].ToString(), 1);
        }

        return new Category(nameToken.ToString(), terms);
    }
    
    /// <summary>
    /// Creates a Term from a JObject.
    /// </summary>
    /// <param name="valueObject">The JObject to create a Term from.</param>
    /// <returns>The created Term.</returns>
    private static Term CreateTerm(JObject valueObject)
    {
        var textToken = valueObject.GetValue("Text", StringComparison.OrdinalIgnoreCase);

        if (textToken == null)
        {
            throw new CategoryCompilationException("Could not find 'Text' property in term object.");
        }

        var weightToken = valueObject.GetValue("Weight", StringComparison.OrdinalIgnoreCase);
        double weight = weightToken?.ToObject<double>() ?? 1;

        return new Term(textToken.ToString(), weight);
    }
}


public class CategoryCompilationException : Exception
{
    public CategoryCompilationException()
    {
        
    }

    public CategoryCompilationException(string message) : base(message)
    {
        
    }
}