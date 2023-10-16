// This script handles drag-and-drop functionality for cards in a Kanban-style board.

// Execute these functions when the document is ready.
$(document).ready(function () {
    onDrag();
    onDragOver();
    onDrop();
    $(".description").hide();
});

// Variable to keep track of the dragged card.
let cardDragged = null;

// Function to handle the dragstart event for cards.
function onDrag() {
    const cards = document.getElementsByClassName("party_card");
    for (const card of cards) {
        card.addEventListener("dragstart", function (e) {
            cardDragged = e.target;
        });
    }
}

// Function to handle the dragover event for lists.
function onDragOver() {
    const lists = document.getElementsByClassName("list");
    for (const list of lists) {
        list.addEventListener("dragover", function (e) {
            console.log("drag over");
            e.preventDefault();
        });
    }
}

// Function to handle the drop event for lists.
function onDrop() {
    const lists = document.getElementsByClassName("list");
    for (const list of lists) {
        list.addEventListener("drop", function (e) {
            e.preventDefault();

            // Check if the drop target is a list or one of its descendants.
            if (e.target.className === "list" || isDescendant(e.target, list)) {
                // Extract IDs and update card list.
                let spliceIdList = list.id.replace("list_", "");
                let spliceIdCard = cardDragged.id.replace("card_", "");
                updateCardList(spliceIdList, spliceIdCard, list, cardDragged);
            }
        });
    }
}

// Function to check if a node is a descendant of another node.
function isDescendant(childNode, parentNode) {
    if (childNode === parentNode) {
        // The child node is the parent node, so it is its own descendant.
        return true;
    }

    if (!childNode.parentNode) {
        // The child node has no parent node, so it cannot be a descendant of the parent node.
        return false;
    }

    return isDescendant(childNode.parentNode, parentNode);
}

// Function to update the card's list using AJAX.
function updateCardList(listId, cardId, list, cardDragged) {
    $.ajax({
        url: '/Board/UpdateCardList',
        type: 'PUT',
        data: { ListId: listId, CardId: cardId },
        success: function (data) {
            // Append the dragged card to the drop target list.
            list.appendChild(cardDragged);
        }
    });
}

function toggleDescription(cardId) {
    var description = $("#description_" + cardId);
    var addButton = $("#addButton_" + cardId);

    // Toggle the visibility of the description and button "Add"
    description.slideToggle(function () {
        addButton.toggle(description.is(":visible"));
    });
}
