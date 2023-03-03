# Prompthing

Prompthing is a flexible text generation library designed for use with JSON templates. The library includes a number of powerful features that make it easy to create rich, dynamic text output with minimal code.

Prompthing is particularly well-suited for generating text output from structured data sources, such as APIs or databases. The library supports a variety of special markup features, including:

Choosing from pre-defined lists: Prompthing can select words or phrases from pre-defined lists, allowing you to easily incorporate dynamic and varied content into your templates. 

- **Loops**: Prompthing supports loops, making it easy to generate repeated content from arrays or other data structures.
- **Nested templates**: Prompthing allows you to nest templates within each other, enabling you to build complex, hierarchical text output with ease.
- **Random token selection**: Prompthing supports selecting a random token from a pre-defined list right in a template, making it easy to create diverse and engaging text content.
- **Wrapper nodes**: Prompthing includes wrapper nodes, which allow you to wrap content with additional text or markup.

# Core Entities
Prompthing is a flexible text generation library that provides a range of tools and features for creating dynamic and engaging text output from structured data sources. The library includes several key entities that are used to define and generate text, including **Category**, **Term**, **Template**, and **Wrapper**.

### Category
A Category is a dataset that contains a collection of Term objects that can be used to generate dynamic text. A Category can be defined as a collection of words or phrases that have a common theme or are otherwise related. For example, you might define a Category called "animal" that contains terms such as "dog", "cat", "bird", and so on.

### Term
A Term is the smallest unit in the library and represents a single word or phrase. A Term can include a text value and a weight value, which can be used to influence the likelihood of the Term being selected during text generation. The weight value can be any positive decimal, and higher values will increase the chances of the Term being selected.

### Template
A Template is a string that includes special markup that can be used to generate dynamic text output. The Template can include Category markup, which will be replaced by a randomly selected Term from the associated Category during text generation. Template can also include additional special features such as loops, nested templates, and random token selection.

### Wrapper
A Wrapper is similar to a Template, but it includes an additional special tag that can be used to wrap content within a specified text or markup. The Wrapper can include the same special features as the Template, such as loops, nested templates, and random token selection. The Wrapper can be used to insert dynamic content into a larger text.

# Getting Started
### Overview
This documentation explains how to create and compile a JSON dataset containing templates and categories, and how to use it to generate text output. The sample JSON dataset provided includes a single template and two categories with predefined values.
### Creating a JSON Dataset
To create a JSON dataset, create a file and give it a name with a ".json" extension. In this example, we will create a file named "example.json" with the following content:
```json
{
  "templates": [
    {
      "template": "a {{gender}} holding {{item}}"
    }
  ],
  "categories": [
    {
      "name": "gender",
      "values": [
        "men",
        "woman"
      ]
    },
    {
      "name": "item",
      "values": [
        "book",
        "glass",
        "flag"
      ]
    }
  ]
}
```
The above JSON dataset includes one template and two categories. The "templates" category includes a single template, which contains placeholders enclosed in double curly braces. The "categories" category includes two categories, "gender" and "item", each with predefined values.

### Compiling a JSON Dataset

To compile the JSON dataset, read the content of the file using the `File.ReadAllText()` method and pass it to the `Compile()` method of the `DatasetCompiler` class as follows:

```csharp
string content = File.ReadAllText("example.json");
var templates = new DatasetCompiler().Compile(content);
```

The above code reads the content of the "example.json" file and compiles it using the `Compile()` method of the `DatasetCompiler` class. The resulting compiled templates are stored in the templates variable.

### Generating Text Output
Once the JSON dataset has been compiled, you can generate text output by evaluating the `Node` property of the compiled template. To do this, create a new `StringBuilder` object and pass it to the `Evaluate()` method of the `Node` property as follows:

```csharp
var stringBuilder = new StringBuilder();
templates[0].Node.Evaluate(stringBuilder);
```
The above code creates a new `StringBuilder` object and passes it to the `Evaluate()` method of the `Node` property of the first compiled template. The resulting text output is stored in the stringBuilder variable.

### Example Output
Running the above code may result in one of the following text outputs, depending on the values randomly selected from the "gender" and "item" categories:

- a men holding book
- a woman holding flag
- a woman holding glass
- a men holding flag
- a men holding glass
- a woman holding book
### Conclusion
This documentation provided a basic understanding of how to create and compile a JSON dataset, and how to use it to generate text output. The sample JSON dataset included a single template and two categories with predefined values, but this can be expanded to include more templates and categories with custom values.