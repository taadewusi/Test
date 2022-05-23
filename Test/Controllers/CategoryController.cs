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

        /// <summary>
        /// This endpoint returns all categories.
        ///
        /// </summary>
        /// <param></param>
        /// <returns>All Categories as a JSON list</returns>

        [HttpGet, Route("get-all")]      
        public IActionResult GetAll()
        {
            try
            {
               
                var category = _category.GetAll();

                if (category.Count > 0)
                {
                    return Ok(category);
                }
                else
                {
                    return NoContent();
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }

        }

        /// <summary>
        /// This endpoint returns all favorite categories.
        ///
        /// </summary>
        /// <param></param>
        /// <returns>All favorite categories as a JSON list</returns>
        [HttpGet, Route("get-all-fav")]
        public IActionResult GetAllFave()
        {
            try
            {
                var category = _category.GetAll().Where(x=>x.Favorite==true);

                if (category!=null)
                {
                    return Ok(category);
                }
                else
                {
                    return NoContent();
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// This endpoint returns a particular category with the supplied ID.
        ///
        /// </summary>
        /// <param></param>
        /// <returns>A category as a JSON</returns>
        [HttpGet, Route("get-by-id/{id}")]        
        public IActionResult GetById(Guid id)
        {
            var movie = _category.GetById(id);
            if (movie == null)
                return Ok(new { message = "Category not available!" });
            return Ok(movie);
        }

        /// <summary>
        /// This endpoint adds a new category.
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A new category added as a JSON</returns>
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
                return Ok(movie);
                
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This endpoint sets a  category as a favorite.
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated category as a JSON</returns>
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

        /// <summary>
        /// This endpoint removes a  category as a favorite.
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated category as a JSON</returns>
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

        /// <summary>
        /// This endpoint update a  category as a favorite.
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated category as a JSON</returns>
        [HttpPut, Route("update-category")]
        public IActionResult UpdateCategory([FromBody] CategoryViewModel model)
        {
            
            var entity = _mapper.Map<BookCategory>(model);
            _category.Update(entity);
            _category.Save();


            return Ok(_category.GetById(model.CategoryId));
        }

        /// <summary>
        /// This endpoint deletes a  category.
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updated category as a JSON</returns>
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
