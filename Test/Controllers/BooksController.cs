using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.DataTiers.Repositories;
using Test.Models.Books;
using ViewModels.Category;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _books;
        private readonly IMapper _mapper;

        public BooksController(IRepository<Book> book, IMapper mapper)
        {
            _books = book;
            _mapper = mapper;
        }


        [HttpGet, Route("get-all")]

        public IActionResult GetAll()
        {
            //[FromQuery] MovieParameters parameters
            try
            {
                var _booksExist = "Book(s) Retrieved Successfully";
                var _booksDoesNotExist = "No Book Found";

                // var movies = _category.GetAll(parameters);
                var movies = _books.GetAll();

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
            var books = _books.GetById(id);
            if (books == null)
                return Ok(new { message = "Books not available!" });
            return Ok(books);
        }

        [HttpPost, Route("add-book")]
        public IActionResult AddBook([FromBody] AddBookViewModel model)
        {
            try
            {
                var checker = _books.GetAll().Where(x => x.BookName == model.BookName);
                if (checker.Any())
                {
                    return BadRequest("Book already exist");
                }
                var movie = new Book()
                {

                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    Description = model.Description

                };
                _books.Add(movie);
                _books.Save();
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

        [HttpPut, Route("update-book")]
        public IActionResult UpdateBook([FromBody] BookViewModel model)
        {

            var entity = _mapper.Map<Book>(model);
            _books.Update(entity);
            _books.Save();
            return Ok(_books.GetById(model.BookId));
        }


        [HttpDelete, Route("delete-book/{id}")]
        public IActionResult DeleteBook(Guid id)
        {
            var movie = _books.GetById(id);
            if (movie == null)
                return Ok(new { message = "Book not available!" });

            _books.Delete(movie.BookId);
            _books.Save();
            return Ok(new { message = "Book deleted successfully!" });


        }
    }
}
