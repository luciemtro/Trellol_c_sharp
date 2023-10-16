using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text;
using Trellol.Data;
using Trellol.Models;

namespace Trellol.Controllers
{
    public class BoardController : Controller
    {   
        // Récupération de la DB dans le controller
        private readonly ApplicationDbContext _db;
        public BoardController(ApplicationDbContext db)
        {   
            _db = db;
        }
#region Board Actions

        // Action sur l'index de Board
        public IActionResult Index()
        {
            List<Board> objBoardList = _db.Boards?.ToList() ?? new List<Board>();
            return View(objBoardList);
        }

        // Action sur la page Création de Board
        public IActionResult Create() 
        {
            return View();
        }

        // Action sur la création des Boards, méthode POST, on lui donne un obj(un board)
        [HttpPost]
        public IActionResult Create(Board obj)
        {   //ModelState.IsValid est préfait
            if (ModelState.IsValid)
            {   
                _db.Boards.Add(obj);
                _db.SaveChanges();
                // Sert à diffuser un message temporaire (Voir TostR et _Notification)
                TempData["success"] = "Board created successfully !";
                // Si tout est réussi, on retourne à l'index
                return RedirectToAction("Index");
            }
            // Si ça a échoué, on reload la même page
            return View();
        }

        // Action sur la suppression des boards, on lui donne l'id du board comme para
        public IActionResult Delete(int? id)
        {   
            // Si l'id n'est pas trouvé, est null, on retourne la page not found
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

        // Cette fois, on agit sur la DB avec la suppression
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            // On dit que le board est égale à l'id trouvé dans la table board
            Board obj = _db.Boards.Find(id);
            // On test si l'obj (Le board) a été trouvé, si non, on renvoie not found
            if (obj == null)
            {
                return NotFound();
            }
            // On utilise la méthode remove en lui donnant l'obj (Le board), on sauvegarde la DB

            _db.Boards.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Board deleted successfully !";
            // Retourne une réponse JSON indiquant le succès de la suppression
            return Json(new { success = true });
        }
        #endregion

#region List Actions
        // Action sur SingleBoard
        public IActionResult SingleBoard(int id)
        {
            // Je récupère le tableau via son Id ainsi que les listes reliées à mon tableau
            Board board = _db.Boards.Include(b => b.Lists).ThenInclude(l => l.Cards).FirstOrDefault(b => b.Id == id);
            // Gère le cas où aucun tableau avec cet ID n'est trouvé
            if (board == null)
            {
                return NotFound();
            }
            // Si il n'y a pas d'erreur, je retourne la vue à l'utilisateur
            return View(board);
        }

        // Méthode post pour ajouter des listes
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
                TempData["success"] = "List created successfully !";
                // Retourne une réponse JSON indiquant le succès
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
            TempData["success"] = "List deleted successfully !";
            // Retourne une réponse JSON indiquant le succès de la suppression
            return Json(new { success = true });
        }
#endregion

#region Action Card
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
                TempData["success"] = "Card created successfully !";
                // Retourne une réponse JSON indiquant le succès
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
            TempData["success"] = "Card deleted successfully !";
            // Retourne une réponse JSON indiquant le succès de la suppression
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

            card.Description = description;

            
            
                _db.SaveChanges();
                TempData["success"] = "Description created successfully !";

                return Json(new { success = true });

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
