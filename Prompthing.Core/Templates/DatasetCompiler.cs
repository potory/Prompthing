using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Prompthing.Core.Compilers;
using Prompthing.Core.Entities;
using Prompthing.Core.Extensions;
using Prompthing.Core.Functions;
using Prompthing.Core.Utilities;
using SonScript.Core;

namespace Prompthing.Core.Templates;

public class DatasetCompiler
{
    private readonly FunctionFactory _factory;
    private readonly IServiceCollection _serviceCollection;
    private readonly ServiceProvider _serviceProvider;

    public DatasetCompiler()
    {
        _serviceCollection = new ServiceCollection()
            .AddSingleton(x => x)
            .AddSingleton<FunctionContext>()
            .AddSingleton<FunctionParser>()
            .AddSingleton<ReferencePool>();

        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _factory = new FunctionFactory(_serviceProvider);

        RegisterFunctions();

        _serviceCollection.AddSingleton(_factory);
    }

    private void RegisterFunctions()
    {
        _factory.RegisterFunction<TemplateFunction>("t");
        _factory.RegisterFunction<TemplateFunction>("temp");
        _factory.RegisterFunction<TemplateFunction>("template");
        _factory.RegisterFunction<BackspaceFunction>("backspace");
    }

    public Dataset Compile(string jsonDataset)
    {
        var pool = _serviceProvider.GetService<ReferencePool>();
        pool.Clear();

        var jsonRoot = JObject.Parse(jsonDataset);

        MapCategoriesToPool(jsonRoot, pool);
        MapWrappersToPool(jsonRoot, pool);

        var templates = CompileTemplates(jsonRoot, pool);
        _serviceCollection.Remove<ReferencePool>();

        return new Dataset(_factory, pool, templates);
    }

    /// <summary>
    /// Maps categories from a JSON object to a reference pool.
    /// </summary>
    /// <param name="jsonObject">The root JSON object containing the categories.</param>
    /// <param name="referencePool">The reference pool to add the categories to.</param>
    /// <exception cref="ArgumentNullException">Thrown if jsonObject or referencePool is null.</exception>
    private static void MapCategoriesToPool(JObject jsonObject, ReferencePool referencePool)
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

    private void MapWrappersToPool(JObject jsonObject, ReferencePool referencePool)
    {
        if (jsonObject == null)
        {
            throw new ArgumentNullException(nameof(jsonObject));
        }

        if (referencePool == null)
        {
            throw new ArgumentNullException(nameof(referencePool));
        }

        var categoriesRoot = jsonObject.GetValue("Wrappers", StringComparison.OrdinalIgnoreCase);

        if (categoriesRoot == null)
        {
            return;
        }

        Wrapper[] wrappers = CompileWrappers(categoriesRoot, referencePool);

        foreach (var category in wrappers)
        {
            referencePool.AddObject(category.Name, category);
        }
    }

    /// <summary>
    /// Compiles an array of Category objects from a JObject.
    /// </summary>
    /// <param name="data">The JObject to compile.</param>
    /// <returns>The compiled Category objects.</returns>
    private static Category[] CompileCategories(JToken data)
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

    private Wrapper[] CompileWrappers(JToken data, ReferencePool referencePool)
    {
        var compiler = new WrapperCompiler(new TokenInterpreter(referencePool, _factory));

        var wrappersArray = (JArray)data;
        var wrappers = new Wrapper[wrappersArray.Count];

        for (int i = 0; i < wrappers.Length; i++)
        {
            try
            {
                wrappers[i] = compiler.Compile((JObject) wrappersArray[i]);
            }
            catch (CategoryCompilationException ex)
            {
                throw new DatasetCompilationException($"Failed to compile wrapper object at index {i}: {ex.Message}", ex);
            }
        }

        return wrappers;
    }

    /// <summary>
    /// Compiles an array of templates from a JObject.
    /// </summary>
    /// <param name="jsonObject">The JObject containing the templates.</param>
    /// <param name="referencePool">The reference pool used to store the compiled templates.</param>
    /// <returns>An array of Template objects.</returns>
    /// <exception cref="ArgumentNullException">Thrown when jsonObject or referencePool is null.</exception>
    /// <exception cref="DatasetCompilationException">Thrown when the 'Templates' property is missing from the JObject.</exception>
    private Template[] CompileTemplates(JObject jsonObject, ReferencePool referencePool)
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
            throw new DatasetCompilationException("The 'Templates' property is missing from the JObject.");
        }
        
        var compiler = new TemplateCompiler(new TokenInterpreter(referencePool, _factory));
        var array = (JArray)templatesRoot;
        
        var templates = new Template[array.Count];

        for (int i = 0; i < templates.Length; i++)
        {
            var template = compiler.Compile((JObject) array[i]);

            if (templates.Any(x => x != null && x.Name == template.Name))
            {
                throw new DatasetCompilationException($"Duplicate template name '{template.Name}' found in dataset. Please ensure that all template names are unique.");
            }
            
            templates[i] = template;
            referencePool.AddObject(templates[i].Name, templates[i]);
        }
        
        return templates.Where(x => !x.IsSnippet).ToArray();
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