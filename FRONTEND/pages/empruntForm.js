
const params = new URLSearchParams(window.location.search); 
const userId = params.get("id"); 
let bookIds = []; 

const backUserButton = document.getElementById("back-button"); 
backUserButton.href = `./userDetails.html?id=${userId}`;

const addButton = document.getElementById("add-emprunt-button"); 
addButton.addEventListener("click", (event) => {
    event.preventDefault(); 
    bookIds.forEach(async (bookId) => {
        await createEmprunt(userId, bookId)
    }); 
    console.log(userId)
    window.location.href = `./userDetails.html?id=${userId}`;
    console.log(userId)
}); 

function removeId(idToRemove){
    bookIds = bookIds.filter(id => id !== idToRemove); 
}
async function getAllBooksAvailable() {
    const url = "http://localhost:5183/books/available"; 
    try {
        const response = await fetch(url); 
        if(!response.ok) throw new Error(`Response Status: ${response.status}`)
        const data = await response.json(); 
        console.log(data)

        const bookList = document.getElementById("new-emprunt-list")
        data.forEach(book => {
            const div = document.createElement("div"); 

            const input = document.createElement("input"); 
            input.type = "checkbox"; 
            input.id = `input-${book.title.replace(" ", "-")}`;
            input.name = book.title.replace(" ", "-"); 
            input.checked = false;

            if(input.checked) bookIds.push(book.id)

            input.addEventListener("change", () => {
                if(input.checked){
                    input.setAttribute("checked", "checked");
                    bookIds.push(book.id)
                    console.log(bookIds); 
                }  else {
                    input.removeAttribute("checked"); 
                    removeId(book.id)
                    console.log(bookIds); 
                } 
        });

            const label = document.createElement("label"); 
            if (book.quantity > 3)label.textContent = `${book.title} / auteur: ${book.author} `;
            else if (book.quantity > 1) label.textContent = `${book.title} / auteur: ${book.author} (${book.quantity} exemplaires restant)`;
            else label.textContent = `${book.title} / auteur: ${book.author} (plus que ${book.quantity} exemplaire restant)`;
            label.classList.add("p-1")
            div.appendChild(input); 
            div.appendChild(label); 
            bookList.appendChild(div); 
        });
    } catch (error) {
        console.log(error.message)
    }
}
async function createEmprunt(userId, bookId) {
    const newEmprunt = {
        userId: userId,
        bookId: bookId
    }
    const url = "http://localhost:5183/emprunts"

    const response = await fetch(url, {
        method: "POST", 
        headers: {
            "content-type": "application/json"
        }, 
        body: JSON.stringify(newEmprunt)
    }); 
    console.log(response.status); 
}

getAllBooksAvailable(); 