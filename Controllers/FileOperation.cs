using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

public class FileOperation : IOperationFilter
{
    public void Apply(Operation operation, OperationFilterContext context)
    {
        if (operation.OperationId.Equals("ApiGetSDFileInfoGetSDFileInfoPost", StringComparison.InvariantCultureIgnoreCase))
        {
            operation.Parameters.Clear();//Clearing parameters
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "file",
                In = "formData",
                Description = "Upload Image",
                Required = true,
                Type = "file"
            });
            operation.Consumes.Add("application/form-data");
        }
    }
}