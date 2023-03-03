using Newtonsoft.Json.Linq;

namespace Prompthing.Core;

public static class PredefinedTemplates
{
    public static string JsonDataset()
    {
        JObject jsonObject = new JObject(
            new JProperty("templates",
                new JArray(
                    new JObject(
                        new JProperty("template", "a {{gender}} holding {{item}}")
                    )
                )
            ),
            new JProperty("categories",
                new JArray(
                    new JObject(
                        new JProperty("name", "gender"),
                        new JProperty("values",
                            new JArray(
                                "men",
                                "woman"
                            )
                        )
                    ),
                    new JObject(
                        new JProperty("name", "item"),
                        new JProperty("values",
                            new JArray(
                                "book",
                                "glass",
                                "flag"
                            )
                        )
                    )
                )
            )
        );

        return jsonObject.ToString();
    }
}