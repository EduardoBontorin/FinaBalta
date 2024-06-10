using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers
{
    public class CategoryHandler(AppDbContext context) : ICategoryHandler
    {
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            var category = new Category()
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(null, 201, "Categoria criada com sucesso.");
            }
            catch (DbUpdateException ex)
            {
                //TODO: Estudar Serilog, OpenTelemetry
                Console.WriteLine(ex.Message);
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
            }
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null) 
                {
                    return new Response<Category?>(category,404, message: "Categoria não encontrada");
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category, message: "Categoria excluida com sucesso");
            }
            catch (Exception ex) 
            {
                return new Response<Category?>(null, 500, message: "Houve um erro ao excluir categoria, tente novamente.");
            }
        }

        public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
        {
            try
            {
                var query = context.Categories
                                .AsNoTracking()
                                .Where(x => x.UserId == request.UserId)
                                .Skip((request.PageNumber - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .OrderBy(x => x.Title);

                var categories = await query.ToListAsync();
                var count = await query.CountAsync();

                return new PagedResponse<List<Category>?>(
                    categories,
                    count,
                    request.PageNumber,
                    request.PageSize);
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<Category>?>(null,500,"Não foi possível consultar as categorias");
            }
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>x.Id == request.Id && x.UserId == request.UserId);

                return category is null 
                    ? new Response<Category?>(null, 404, "Categoria Não encontrada") 
                    : new Response<Category?>(category);
            }
            catch (Exception ex) 
            {
                return new Response<Category?>(null, 500, "Houve um erro ao pesquisar categoria");
            }
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (category is null)
                {
                    return new Response<Category?>(null, 404, "Categoria não encontrada");
                }
                category.Title = request.Title;
                category.Description = request.Description;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return new Response<Category?>(category,message:"Categoria atualizada com sucesso");

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria");
            }
        }
    }
}
