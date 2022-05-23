using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.DataTiers.Repositories;
using Test.Models.Books;
using Test.Models.Category;
using ViewModels.Category;

namespace Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _books;
        private readonly IRepository<BookCategory> _category;
        private readonly IMapper _mapper;

        public BooksController(IRepository<Book> book, IRepository<BookCategory> category,IMapper mapper)
        {
            _books = book;
            _mapper = mapper;
            _category = category;
        }

        /// <summary>
        /// This endpoint returns the list of all the books as a Json list.      
        /// </summary>
        /// <param></param>
        /// <returns>The list of all books</returns>
        [HttpGet, Route("get-all")]   
        public IActionResult GetAll()
        {
            //[FromQuery] MovieParameters parameters
            try
            {
                var _booksExist = "Book(s) Retrieved Successfully";
                var _booksDoesNotExist = "No Book Found";

                // var books = _category.GetAll(parameters);
                var books = _books.GetAll();

                if (books.Count > 0)
                {
                    return Ok(books);
                }
                else
                {
                    return NoContent();
                }
                //if (books.MetaData.TotalCount > 0)
                //{
                //    return Ok(new Response<PagedList<Movie>>
                //    {
                //        MetaData = books.MetaData,
                //        Data = books,
                //        Status = Constants.SuccessCode,
                //        Message = booksExist
                //    });
                //}
                //return Ok(new Response<PagedList<Movie>>
                //{
                //    MetaData = books.MetaData,
                //    Data = books,
                //    Status = Constants.NoData,
                //    Message = booksDoesNotExist
                //});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                //_logger.LogError($"AnywakaAPI: - {MethodBase.GetCurrentMethod()?.Name} - {ex.Message}");

                //return CustomResult(System.Net.HttpStatusCode.InternalServerError, Constants.ExceptionCode, Constants.DefaultExceptionFriendlyMessage);
            }

        }

        [HttpGet, Route("get-all-fav")]
        public IActionResult GetAllFav()
        {
            //[FromQuery] MovieParameters parameters
            try
            {
                var _booksExist = "Book(s) Retrieved Successfully";
                var _booksDoesNotExist = "No Book Found";

                // var books = _category.GetAll(parameters);
                var books = _books.GetAll().Where(x=>x.Favorite==true);

                if (books!=null||books.Count()<0)
                {
                    return Ok(books);
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
        /// This returns the book of the supplied ID if found else returns a bad request
        /// </summary>
        /// <param name="id"></param>
   
        /// <returns></returns>
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
                Guid g = Guid.NewGuid();
                var movie = new Book()
                {
                    BookId = g,
                    BookName = model.BookName,
                    Pages = model.Pages,
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                    Favorite = model.Favorite,

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

        [HttpPut, Route("set-as-fav/{id}")]
        public IActionResult SetAsFav(Guid id)
        {
            var books = _books.GetById(id);
            if (books == null)
            {
                return BadRequest("Book not found!");
            }
            else
            {
                books.Favorite = true;
                _books.Update(books);
                _books.Save();
                return Ok(books);
            }
        }
       
        [HttpPut, Route("remove-as-fav/{id}")]
        public IActionResult RemoveAsFav(Guid id)
        {
            var books = _books.GetById(id);
            if (books == null)
            {
                return BadRequest("Book not found!");
            }
            else
            {
                books.Favorite = false;
                _books.Update(books);
                _books.Save();
                return Ok(books);
            }
        }

        [HttpPut, Route("add-to-category/{BookId}")]
        public IActionResult AddToCategory(Guid BookId, Guid CategoryId)
        {
            var books = _books.GetById(BookId);
            if (books == null)
            {
                return BadRequest("Book not found!");
            }
            else
            {
                var cat = _category.GetById(CategoryId);
                if (cat == null)
                {
                    return BadRequest("Category not found!");
                }
                else
                {
                    books.CategoryId = cat.CategoryId.ToString();
                    books.CategoryName = cat.CategoryName;
                    _books.Update(books);
                    _books.Save();
                    return Ok(books);
                }
                
            }
        }
       
        [HttpPut, Route("remove-from-category/{id}")]
        public IActionResult RemoveFromCategory(Guid id)
        {
            var books = _books.GetById(id);
            if (books == null)
            {
                return BadRequest("Book not found!" );
            }
            else
            {

                books.CategoryId ="";
                books.CategoryName = "";
                _books.Update(books);
                _books.Save();
                return Ok(books);
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
