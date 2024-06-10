using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Fina.Api.Controllers.Categories
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/",HandleAsync)
                .WithName("Categories : Create")
                .WithSummary("Cria uma nova categoria")
                .WithDescription("Cria uma nova categoria")
                .WithOrder(1)
                .Produces<Response<Category>>();
        
            
        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
        {
            request.UserId = ApiConfiguration.UserId;
            var response = await handler.CreateAsync(request);
            
            return response.IsSuccess 
                ?TypedResults.Created($"v1/categories/{response.Data?.Id}",response)
                :TypedResults.BadRequest(response);
        }
    }
}
