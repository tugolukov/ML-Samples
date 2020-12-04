using NJsonSchema;
using NSwag;
using NSwag.Annotations;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace CatsOrDogs.API.Utils
{
    /// <inheritdoc />
    public class FileUploadProcessor : IOperationProcessor
    {
        /// <inheritdoc />
        public bool Process(OperationProcessorContext context)
        {
            var data = context.OperationDescription.Operation.Parameters;
            
            data.Add( new OpenApiParameter()
            {
                IsRequired = true,
                Name = "file",
                Description = "filechunk",
                Type = JsonObjectType.File,
                Kind = OpenApiParameterKind.FormData
            });
            
            // //custom formdata
            // data.Add(new OpenApiParameter()
            // {
            //     IsRequired = true,
            //     Name = "file-name",
            //     Description = "the original file name",
            //     Type = JsonObjectType.String,
            //     Kind = OpenApiParameterKind.FormData
            // });

            return true;
        }
    }

    /// <inheritdoc />
    public class FileUploadAttribute : OpenApiOperationProcessorAttribute
    {
        /// <inheritdoc />
        public FileUploadAttribute() : base(typeof(FileUploadProcessor)) { }
    }
}