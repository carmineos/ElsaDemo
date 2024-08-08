using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System.Text.Json;

public class JsonSchemaBuilder
{
    private readonly JObject _inputJson;
    private JObject? _data = null;

    public JsonSchemaBuilder(string inputJson)
    {
        _inputJson = JObject.Parse(inputJson);
    }

    public async Task<JsonSchema> BuildAsync()
    {
        var schema = JsonSchema.FromJsonAsync(_inputJson["schema"]!.ToString()).Result;
        var graphQL = _inputJson["graphQL"];

        var query = graphQL["query"]!.ToString();
        var endpoint = "https://enum-field--harvest-haven.apollographos.net/graphql";

        _data = await ExecuteGraphQLQueryAsync(query, endpoint);

        BuildDataEnums(schema);

        return schema;
    }

    public void BuildDataEnums(JsonSchema schema, string tokenName = "name")
    {      
        foreach (var property in _data.Properties())
        {
            var definition = schema.Definitions[property.Name];
            definition.Enumeration.Clear();

            JToken propertyValue = property.Value;
            if (propertyValue.Type == JTokenType.Array)
            {
                foreach (var item in propertyValue.Children())
                {
                    var itemValue = item.SelectToken(tokenName);
                    definition.Enumeration.Add(itemValue);
                }
            }
        }
    }

    private async Task<JObject> ExecuteGraphQLQueryAsync(string query, string endpoint)
    {
        var graphQLClient = new GraphQLHttpClient(endpoint, new SystemTextJsonSerializer());
        var request = new GraphQLRequest { Query = query };
        var response = await graphQLClient.SendQueryAsync<object>(request);
        var element = (JsonElement)response.Data;
        return JObject.Parse(element.GetRawText());
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        // TODO: Describe a single GraphQL query and use multiple placeholders to get the result object property
        string inputSchema =
            """
            {
              "graphQL": {
                "query": "{ fruits { name } }"
              },
              "schema": {
                "definitions": {
                  "fruits": {
                    "type": "string",
                    "enum": []
                  }
                },
                "type": "object",
                "properties": {
                  "fruitName": {
                    "type": "string",
                    "title": "Fruit Name",
                    "description": "The name of the fruit",
                    "$ref": "#/definitions/fruits"
                  },
                  "quantity": {
                    "type": "integer",
                    "title": "Quantity",
                    "description": "The quantity of the fruit",
                    "default": 1,
                  }
                },
                "required": ["quantity", "fruitName"]
              }
            }
            """;

        Console.WriteLine("INPUT");
        Console.WriteLine(inputSchema);

        var builder = new JsonSchemaBuilder(inputSchema);
        var newSchema = await builder.BuildAsync();

        Console.WriteLine();
        Console.WriteLine("OUTPUT");
        Console.WriteLine(newSchema.ToJson());

        var test =
            """
            {
              "$defs": {
                "roles": {
                  "type": "string",
                  "enum": ["HR", "Manager"]
                }
              },
              "type": "object",
              "properties": {
                "firstName": {
                  "title": "First Name",
                  "type": "string",
                  "description": "The first name of the employee"
                },
                "lastName": {
                  "title": "Last Name",
                  "type": "string",
                  "description": "The last name of the employee"
                },
                "role": {
                  "title": "Role",
                  "type": "string",
                  "description": "The role of the employee",
                  "$ref": "#/$defs/roles"
                }
              },
              "required": ["firstName", "lastName", "role"]
            }
            """;
    }
}
