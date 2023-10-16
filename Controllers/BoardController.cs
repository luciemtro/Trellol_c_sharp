using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using Trellol.Data;
using Trellol.Models;

namespace Trellol.Controllers
{
    // Controller responsible for handling Board-related actions
    public class BoardController : Controller
    {
        // Retrieving the database context in the controller
        private readonly ApplicationDbContext _db;
        public BoardController(ApplicationDbContext db)
        {
            _db = db;
        }

        #region Board Actions

        // Action for the index page of Board
        public IActionResult Index()
        {
            // Fetching the list of boards from the database, or an empty list if null
            List<Board> objBoardList = _db.Boards?.ToList() ?? new List<Board>();
            return View(objBoardList);
        }

        // Action for the Create page of Board
        public IActionResult Create()
        {
            return View();
        }

        // Action for creating boards, HTTP POST method, taking a board object as a parameter
        [HttpPost]
        public IActionResult Create(Board obj)
        {
            // Checking if the model state is valid
            if (ModelState.IsValid)
            {
                // Adding the board to the database and saving changes
                _db.Boards.Add(obj);
                _db.SaveChanges();
                // Displaying a temporary success message (See Toastr and _Notification)
                TempData["success"] = "Board created successfully!";
                // If everything is successful, redirect to the index
                return RedirectToAction("Index");
            }
            // If it fails, reload the same page
            return View();
        }

        // Action for deleting boards, taking the board id as a parameter
        public IActionResult Delete(int? id)
        {
            // If the id is not found or null, return the not found page
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Board boardFromDb = _db.Boards.Find(id);

            if (boardFromDb == null)
            {
                return NotFound();
            }

            return View(boardFromDb);
        }

        // This time, we perform the actual deletion in the database
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            // Retrieving the board based on the id from the board table
            Board obj = _db.Boards.Find(id);
            // Checking if the object (board) is found, if not, return not found
            if (obj == null)
            {
                return NotFound();
            }
            // Using the remove method, giving it the object (board), and saving the database
            _db.Boards.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Board deleted successfully!";
            // Returning a JSON response indicating the success of the deletion
            return Json(new { success = true });
        }
        #endregion

        #region List Actions
        // Action for SingleBoard
        public IActionResult SingleBoard(int id)
        {
            // Retrieving the board and related lists and cards based on the Id
            Board board = _db.Boards.Include(b => b.Lists).ThenInclude(l => l.Cards).FirstOrDefault(b => b.Id == id);
            // Handling the case where no board with this ID is found
            if (board == null)
            {
                return NotFound();
            }
            // If there are no errors, return the view to the user
            return View(board);
        }

        // HTTP POST method for adding lists
        [HttpPost]
        public IActionResult AddList(int boardId, string listName)
        {
            Board board = _db.Boards.Include(b => b.Lists).FirstOrDefault(b => b.Id == boardId);

            if (board == null)
            {
                return NotFound();
            }

            List newList = new List
            {
                Name = listName,
                BoardId = boardId,
                Board = _db.Boards.FirstOrDefault(b => b.Id == boardId)
            };

            if (ModelState.IsValid)
            {
                board.Lists.Add(newList);
                _db.SaveChanges();
                TempData["success"] = "List created successfully!";
                // Returning a JSON response indicating success
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost, ActionName("DeleteList")]
        public IActionResult DeleteListPOST(int? id)
        {
            List list = _db.Lists.Find(id);

            if (list == null)
            {
                return NotFound();
            }

            int boardId = list.BoardId;

            _db.Lists.Remove(list);
            _db.SaveChanges();
            TempData["success"] = "List deleted successfully!";
            // Returning a JSON response indicating the success of the deletion
            return Json(new { success = true });
        }
        #endregion

        #region Card Actions
        [HttpPost]
        public IActionResult AddCard(int listId, string cardName)
        {
            List list = _db.Lists.Include(l => l.Cards).FirstOrDefault(l => l.Id == listId);

            if (list == null)
            {
                return NotFound();
            }

            Card newCard = new Card
            {
                Name = cardName,
                ListId = listId,
                List = _db.Lists.FirstOrDefault(l => l.Id == listId)
            };

            if (ModelState.IsValid)
            {
                list.Cards.Add(newCard);
                _db.SaveChanges();
                TempData["success"] = "Card created successfully!";
                // Returning a JSON response indicating success
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost, ActionName("DeleteCard")]
        public IActionResult DeleteCardPOST(int? id)
        {
            Card card = _db.Cards.Find(id);

            if (card == null)
            {
                return NotFound();
            }

            int listId = card.ListId;

            _db.Cards.Remove(card);
            _db.SaveChanges();
            TempData["success"] = "Card deleted successfully!";
            // Returning a JSON response indicating the success of the deletion
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult EditCardDescription(int cardId, string description)
        {
            Card card = _db.Cards.Find(cardId);

            if (card == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                card.Description = description;

                _db.SaveChanges();
                TempData["success"] = "Description created successfully!";

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        #endregion

        #region Drag and Drop
        [HttpGet]
        public IActionResult Hello()
        {
            return Json(new { success = true });
        }

        [HttpPut]
        public IActionResult UpdateCardList(int listId, int cardId)
        {
            Card card = _db.Cards.Find(cardId);

            if (card == null)
            {
                return NotFound();
            }

            card.ListId = listId;

            _db.SaveChanges();
            TempData["success"] = "Card update position !";

            return Json(new { success = true });
        }
        #endregion
    }
}
