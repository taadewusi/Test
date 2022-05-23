using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.DataTiers.Repositories;
using Test.Models.Category;
using ViewModels.Category;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<BookCategory> _category;
        private readonly IMapper _mapper;

        public CategoryController(IRepository<BookCategory> category, IMapper mapper)
        {
            _category = category;   
            _mapper = mapper;
        }


        [HttpGet, Route("get-all")]
      
        public IActionResult GetAll()
        {
            //[FromQuery] MovieParameters parameters
            try
            {
                var moviesExist = "Categories Retrieved Successfully";
                var moviesDoesNotExist = "No Category Found";

               // var movies = _category.GetAll(parameters);
                var movies = _category.GetAll();

                if (movies.Count > 0)
                {
                    return Ok(movies);
                }
                else
                {
                    return NoContent();
                }
                //if (movies.MetaData.TotalCount > 0)
                //{
                //    return Ok(new Response<PagedList<Movie>>
                //    {
                //        MetaData = movies.MetaData,
                //        Data = movies,
                //        Status = Constants.SuccessCode,
                //        Message = moviesExist
                //    });
                //}
                //return Ok(new Response<PagedList<Movie>>
                //{
                //    MetaData = movies.MetaData,
                //    Data = movies,
                //    Status = Constants.NoData,
                //    Message = moviesDoesNotExist
                //});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
                //_logger.LogError($"AnywakaAPI: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                //return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }

        }


        [HttpGet, Route("get-by-id/{id}")]        
        public IActionResult GetById(Guid id)
        {
            var movie = _category.GetById(id);
            if (movie == null)
                return Ok(new { message = "Category not available!" });
            return Ok(movie);
        }

        [HttpPost, Route("add-category")]
        public IActionResult AddCategory([FromBody] AddCategoryViewModel model)
        {
            try
            {
                var checker = _category.GetAll().Where(x=>x.CategoryName==model.CategoryName);
                if (checker.Any())
                {
                    return BadRequest("Category already exist");
                }
                Guid gi  = Guid.NewGuid();
                var movie = new BookCategory()
                {

                    CategoryId = gi,
                    CategoryName = model.CategoryName,          
                    Description = model.Description,
                    Favorite = model.Favorite,

                };
                _category.Add(movie);
                _category.Save();
               // _logger.LogInformation($"New Movie Created - {JsonConvert.SerializeObject(movie)}");
                return Ok(movie);
                //return Ok(new Response<Category>
                //{
                //    Message = "Movie added successfully",
                //    Status = Constants.SuccessCode,
                //    Data = movie

                //});
            }
            catch (Exception ex)
            {
               // _logger.LogError($"{MethodBase.GetCurrentMethod().Name} - {ex.Message}");
               return BadRequest(ex.Message);
               // return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }
        }
        [HttpPut, Route("set-as-fav/{id}")]
        public IActionResult SetAsFav(Guid id)
        {
            var cate = _category.GetById(id);
            if (cate == null)
            {

                return Ok(new { message = "Category not available!" });
            }
            else
            {
                cate.Favorite = true;
                _category.Update(cate);
                _category.Save();
                return Ok(cate);

            }
        }

        [HttpPut, Route("remove-as-fav/{id}")]
        public IActionResult RemoveAsFav(Guid id)
        {
            var cate = _category.GetById(id);
            if (cate == null)
            {

                return Ok(new { message = "Category not available!" });
            }
            else
            {
                cate.Favorite = false;
                _category.Update(cate);
                _category.Save();
                return Ok(cate);
            }
        }
        [HttpPut, Route("update-category")]
        public IActionResult UpdateCategory([FromBody] CategoryViewModel model)
        {
            
            var entity = _mapper.Map<BookCategory>(model);
            _category.Update(entity);
            _category.Save();


            return Ok(_category.GetById(model.CategoryId));
        }


        [HttpDelete, Route("delete-category/{id}")]
        public IActionResult DeleteCategory(Guid id)
        {
            var movie = _category.GetById(id);
            if (movie == null)
                return Ok(new { message = "Category not available!" });

            _category.Delete(movie.CategoryId);
            _category.Save();
            return Ok(new { message = "Category deleted successfully!" });


        }

    }
}
