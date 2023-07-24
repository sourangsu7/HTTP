using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
/*added new comment to reflect in git check-in through a new branch*/
/*This is a change*/
app.Run(async (HttpContext AppContext) =>
{

    if (AppContext.Request.Path == "/")
    {
        var FirstOperand = default(int);
        var SecondOperand = default(int);
        var Operator = default(string);

        var Payload = new StreamReader(AppContext.Request.Body);

        var Request = await Payload.ReadToEndAsync();

        var ParsedQuery = Microsoft.AspNetCore.WebUtilities
                      .QueryHelpers.ParseQuery(Request);
        var Errors = new List<string>();

        foreach (var queryKey in ParsedQuery.Keys)
        {
            if (queryKey == "firstNumber")
            {
                ParsedQuery.TryGetValue(queryKey, out var _FirstOperand);

                if(StringValues.IsNullOrEmpty(_FirstOperand))
                {
                    Errors.Add("Invalid input for first number");
                }

                int.TryParse(_FirstOperand.FirstOrDefault(), out FirstOperand);


            }
            else if (queryKey == "secondNumber")
            {
                ParsedQuery.TryGetValue(queryKey, out var _SecondOperand);

                if (StringValues.IsNullOrEmpty(_SecondOperand))
                {
                    Errors.Add("Invalid input for second number");
                }

                int.TryParse(_SecondOperand.FirstOrDefault(), out SecondOperand);
            }
            else if (queryKey == "operation")
            {
                ParsedQuery.TryGetValue(queryKey, out var _Operator);

                if (StringValues.IsNullOrEmpty(_Operator))
                {
                    Errors.Add("Invalid input for operation");
                }

                Operator = _Operator.ToString();
            }
           
        }
        if(Errors.Count > 0)
        {
            AppContext.Response.StatusCode = 400;
            AppContext.Response.ContentType = "text/html";
            foreach (var error in Errors)
            {
                 await AppContext.Response.WriteAsync($"<p>{error}</p><br/>");
            }
        }
        else
        {
            var Output = GetOutput(FirstOperand, SecondOperand, Operator);
            if (Output == "Invalid input for 'operation'")
            {
                AppContext.Response.StatusCode = 400;
                await AppContext.Response.WriteAsync(Output);
            }
            else
            {
                await AppContext.Response.WriteAsync(Output);
            }
        }
      
    }
});

app.Run();

string GetOutput(int firstOperand, int secondOperand, string? @operator)
{
    if (@operator == null)
        return "No Operator specified";
    else
    {
        switch (@operator)
        {
            case "add": return (firstOperand + secondOperand).ToString();
            case "subtract": return (firstOperand - secondOperand).ToString();
            case "multiply": return (firstOperand * secondOperand).ToString();
            case "divide": return (firstOperand / secondOperand).ToString();
            default: return "Invalid input for 'operation'";
        }
    }

}