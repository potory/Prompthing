using Newtonsoft.Json.Linq;
using Prompthing.Core.Compilers;
using Prompthing.Core.Entities;

namespace Prompthing.Core.Templates;

public class DatasetCompiler
{
    public TemplateNode[] Compile(string jsonDataset)
    {
        var pool = new ReferencePool();

        JObject jsonRoot = JObject.Parse(jsonDataset);

        MapCategoriesToPool(jsonRoot, pool);

        var templates = CompileTemplates(jsonRoot, pool);

        return null;
    }

    /// <summary>
    /// Maps categories from a JSON object to a reference pool.
    /// </summary>
    /// <param name="jsonObject">The root JSON object containing the categories.</param>
    /// <param name="referencePool">The reference pool to add the categories to.</param>
    /// <exception cref="ArgumentNullException">Thrown if jsonObject or referencePool is null.</exception>
    private void MapCategoriesToPool(JObject jsonObject, ReferencePool referencePool)
    {
        if (jsonObject == null)
        {
            throw new ArgumentNullException(nameof(jsonObject));
        }

        if (referencePool == null)
        {
            throw new ArgumentNullException(nameof(referencePool));
        }

        var categoriesRoot = jsonObject.GetValue("Categories", StringComparison.OrdinalIgnoreCase);

        if (categoriesRoot == null)
        {
            return;
        }

        Category[] categories = CompileCategories(categoriesRoot);

        foreach (var category in categories)
        {
            referencePool.AddObject(category.Name, category);
        }
    }


    /// <summary>
    /// Compiles an array of Category objects from a JObject.
    /// </summary>
    /// <param name="data">The JObject to compile.</param>
    /// <returns>The compiled Category objects.</returns>
    private Category[] CompileCategories(JToken data)
    {
        var compiler = new CategoryCompiler();

        var categoriesArray = (JArray)data;
        var categories = new Category[categoriesArray.Count];

        for (int i = 0; i < categories.Length; i++)
        {
            try
            {
                categories[i] = compiler.Compile((JObject) categoriesArray[i]);
            }
            catch (CategoryCompilationException ex)
            {
                throw new DatasetCompilationException($"Failed to compile category object at index {i}: {ex.Message}", ex);
            }
        }

        return categories;
    }

    private TemplateNode[] CompileTemplates(JObject jsonObject, ReferencePool referencePool)
    {
        if (jsonObject == null)
        {
            throw new ArgumentNullException(nameof(jsonObject));
        }

        if (referencePool == null)
        {
            throw new ArgumentNullException(nameof(referencePool));
        }

        var templatesRoot = jsonObject.GetValue("Templates", StringComparison.OrdinalIgnoreCase);
        
        if (templatesRoot == null)
        {
            throw new DatasetCompilationException("...");
        }
        
        var compiler = new TemplateCompiler(referencePool);
        var array = (JArray)templatesRoot;
        
        var templates = new TemplateNode[array.Count];

        for (int i = 0; i < templates.Length; i++)
        {
            templates[i] = compiler.Compile((JObject) array[i]);
        }
        
        return null;
    }
}

public class DatasetCompilationException : Exception
{
    public DatasetCompilationException()
    {
    }

    public DatasetCompilationException(string message) : base(message)
    {
        
    }

    public DatasetCompilationException(string message, Exception exception) : base(message, exception)
    {
    }
}