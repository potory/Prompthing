# Prompthing

> **Warning**<br>
> This readme is deprecated due to the changing macro engine to SonScript and requires an update.<br><br>
> `#t` macro must now be used as `#t('templateName')`<br>
> `#r` macro has been replaced with `#oneof('value 1', 'value 2', 'value 3')`<br><br>
> `Wrappers` are currently not available.<br>
> `Loops` are currently not available.


Prompthing is a flexible text generation library designed for use with JSON templates. The library includes a number of powerful features that make it easy to create rich, dynamic text output with minimal code.

Prompthing is particularly well-suited for generating text output from structured data sources, such as APIs or databases. The library supports a variety of special markup features, including:

- **Choosing from pre-defined lists**: Prompthing can select words or phrases from pre-defined lists, allowing you to easily incorporate dynamic and varied content into your templates.
- **Loops**: Prompthing supports loops, making it easy to generate repeated content from arrays or other data structures.
- **Nested templates**: Prompthing allows you to nest templates within each other, enabling you to build complex, hierarchical text output with ease.
- **Random token selection**: Prompthing supports selecting a random token from a pre-defined list right in a template, making it easy to create diverse and engaging text content.
- **Wrapper nodes**: Prompthing includes wrapper nodes, which allow you to wrap content with additional text or markup.

### Installation

You can install Prompthing by cloning its GitHub repository and adding a reference to the `Prompthing.Core` project in your solution. To do so, follow these steps:

1. Clone the Prompthing repository from GitHub: <br>
   ```git clone https://github.com/potory/Prompthing.git```
2. Add the Prompthing project to your solution:
    - In Visual Studio, right-click your solution and select "Add" -> "Existing Project".
    - Navigate to the location where you cloned the Prompthing repository and select the `Prompthing.Core.csproj` file.
3. Add a reference to the Prompthing.Core project in your console application project:
    - In Visual Studio, right-click your console application project and select "Add" -> "Reference".
    - In the "Reference Manager" window, select the "Projects" tab.
    - Select the Prompthing.Core project and click "Add".

Now you can start using the Prompthing library in your console application.

# Core Entities
Prompthing is a flexible text generation library that provides a range of tools and features for creating dynamic and engaging text output from structured data sources. The library includes several key entities that are used to define and generate text, including **Category**, **Term**, **Template**, and **Wrapper**.

### Category
A Category is a dataset that contains a collection of Term objects that can be used to generate dynamic text. A Category can be defined as a collection of words or phrases that have a common theme or are otherwise related. For example, you might define a Category called `animal` that contains terms such as `dog`, `cat`, `bird`, and so on.

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
To create a JSON dataset, create a file and give it a name with a `.json` extension. In this example, we will create a file named `example.json` with the following content:
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
The above JSON dataset includes one template and two categories. The `templates` array includes a single template, which contains placeholders enclosed in double curly braces. The `categories` array includes two categories, `gender` and `item`, each with predefined values.

### Compiling a JSON Dataset

To compile the JSON dataset, read the content of the file using the `File.ReadAllText()` method and pass it to the `Compile()` method of the `DatasetCompiler` class as follows:

```csharp
string content = File.ReadAllText("example.json");
var templates = new DatasetCompiler().Compile(content);
```

The above code reads the content of the `example.json` file and compiles it using the `Compile()` method of the `DatasetCompiler` class. The resulting compiled templates are stored in the templates variable.

### Generating Text Output
Once the JSON dataset has been compiled, you can generate text output by evaluating the `Node` property of the compiled template. To do this, create a new `StringBuilder` object and pass it to the `Evaluate()` method of the `Node` property as follows:

```csharp
var stringBuilder = new StringBuilder();
templates[0].Node.Evaluate(stringBuilder);
```
The above code creates a new `StringBuilder` object and passes it to the `Evaluate()` method of the `Node` property of the first compiled template. The resulting text output is stored in the `stringBuilder` variable.

### Example Output
Running the above code may result in one of the following text outputs, depending on the values randomly selected from the `gender` and `item` categories:

- `a men holding book`
- `a woman holding flag`
- `a woman holding glass`
- `a men holding flag`
- `a men holding glass`
- `a woman holding book`
### Conclusion
This documentation provided a basic understanding of how to create and compile a JSON dataset, and how to use it to generate text output. The sample JSON dataset included a single template and two categories with predefined values, but this can be expanded to include more templates and categories with custom values.

# Using Markup Features
### Overview
This documentation explains how to use markup features to generate text output with more advanced formatting. The markup syntax includes a hashtag followed by the name of the markup feature, followed by a colon and a list of arguments separated by semicolons.
### Nested Templates
To use nested templates, you must add a name to one of the templates in your dataset and use the special syntax `{{#t:templateName}}` to call it in text.

For example, the following dataset includes two templates, `locationTemplate` and `genderItemTemplate`. The `genderItemTemplate` uses the `locationTemplate` as a nested template:
```json
{
  "templates": [
    {
      "name": "locationTemplate",
      "template": "standing in a {{location}}"
    },
    {
      "template": "a {{gender}} holding {{item}} {{#t:locationTemplate}}"
    }
  ]
}
```
The `genderItemTemplate` can generate text output like `a man holding a book standing in a library`.
### Snippets
If you have a template that is just a snippet and you don't want it to be compiled, you can toggle the `isSnippet` boolean to exclude it from the return values of dataset compilation.

For example:
```json
{
  "templates": [
    {
      "isSnippet": true,
      "name": "locationSnippet",
      "template": "{{preposition}} a {{location}}"
    },
    {
      "template": "standing {{#t:locationSnippet}}",
      "name": "standingTemplate"
    }
  ]
}
```
In this example, `locationSnippet` is a snippet template and `standingTemplate` uses `locationSnippet` as a nested template. When the dataset is compiled, only `standingTemplate` will be returned as a compiled template.

### Random
To use random markup feature, you must use the special syntax `{{#r:tokenName1;tokenName2;...;tokenNameN}}` to call it in text. In this case, one of the tokens inside the `#r` function will be randomly selected for compilation.

Here's an example dataset using the random markup feature:
```json
{
  "templates": [
    {
      "isSnippet": true,
      "name": "catTemplate",
      "template": "a small young cat"
    },
    {
      "isSnippet": true,
      "name": "dogTemplate",
      "template": "a big old dog"
    },
    {
      "name": "menTemplate",
      "template": "a man standing with {{#r:#t:catTemplate;#t:dogTemplate}}"
    }
  ]
}
```
In the above example, the menTemplate contains a random markup feature that selects either `catTemplate` or `dogTemplate`. During compilation, one of the templates will be chosen at random and included in the output.

Here's an example of how the output may look like after compiling the dataset:

- `a man standing with a small young cat`
- `a man standing with a big old dog`

### Loops

Loops allow you to repeat a token or a snippet of text multiple times. There are two ways to specify the number of iterations: you can either specify a fixed number or a range of possible numbers.

To use loop markup feature, you must use the special syntax `{{#l:3;tokenName}}` or `{{#l;0-5;tokenName}}`. In the first case, the loop inside the text will be repeated exactly three times. In the second case, the loop will choose a random number of iterations from 0 to 5.

For example, suppose we have the following template:

```json
{
  "name": "foxTemplate",
  "template": "{{#l:3;\"little fox, \"}}"
}
```

Then we will get output like this: `little fox, little fox, little fox, `

If we have the following template:

```json
{
  "name": "foxTemplate",
  "template": "{{#l:2-4;\"little fox, \"}}"
}
```

Then we can get any of this outputs:
- `little fox, little fox, `
- `little fox, little fox, little fox, `
- `little fox, little fox, little fox, little fox, `

Note that the token or snippet of text that is being repeated can be any valid token or snippet, including other markup features.

### Wrappers
To create a wrapper, you need to define a new object in the JSON array `wrappers`. Each wrapper requires a name, a `content` placeholder name, and the wrapper content itself. The placeholder name should be unique within the wrapper, and is used to indicate where the content should be inserted.
```json
{
  "templates": [
    {
      "name": "exampleContent",
      "isSnippet": true,
      "template": "some example content"
    },
    {
      "template": "{{#w:exampleWrapper(#t:exampleContent)}}"
    }
  ],
  "wrappers": [
    {
      "name": "exampleWrapper",
      "content": "placeholder",
      "wrapper": "!!! {{placeholder}} !!!"
    }
  ]
}
```

After a wrapper is defined, you can use it with the syntax: `{{#w:wrapperName(tokenName)}}`.

For example, if we compile the non-snippet template from the previous example, we would get the following text:

`!!! some example content !!!`