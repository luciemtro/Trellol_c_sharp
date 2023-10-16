$(document).ready(function () {
    onDrag();
    onDragOver();
    onDrop();
});
let cardDragged = null;
function onDrag() {

    const cards = document.getElementsByClassName("party_card");
    for (const card of cards) {
        card.addEventListener("dragstart", function (e) {
            cardDragged = e.target;
        });
    }
}

function onDragOver() {
    const lists = document.getElementsByClassName("list");
    for (const list of lists) {
        list.addEventListener("dragover", function (e) {
            console.log("drag over");
            e.preventDefault();
        });
    }
}

function onDrop() {
    const lists = document.getElementsByClassName("list");
    for (const list of lists) {

        list.addEventListener("drop", function (e) {
            e.preventDefault();          
            if (e.target.className === "list" || isDescendant(e.target, list)) {
                let spliceIdList = list.id.replace("list_", "");
                let spliceIdCard = cardDragged.id.replace("card_", "");
                updateCardList(spliceIdList, spliceIdCard, list, cardDragged);
         
            }
        });
    }
}

function isDescendant(childNode, parentNode) {
    if (childNode === parentNode) {
        // Le nœud enfant est le nœud parent, donc il est son propre descendant.
        return true;
    }

    if (!childNode.parentNode) {
        // Le nœud enfant n'a pas de nœud parent, donc il ne peut pas être descendant du nœud parent.
        return false;
    }

    return isDescendant(childNode.parentNode, parentNode);
}
function updateCardList(listId, cardId, list, cardDragged) {
    $.ajax({
        url:'/Board/UpdateCardList', 
        type: 'PUT',
        data: { ListId: listId, CardId: cardId },
        success: function (data) {
            list.appendChild(cardDragged);   
        }
    });
}









