const params = new URLSearchParams(window.location.search); // = params de l'URL
const bookId = params.get("id"); 

const deleteButton = document.getElementById("delete-button"); 
deleteButton.addEventListener("click", () => {
    deleteBook(bookId)
    window.location.href="bookList.html"
}); 

async function addQuantity(id) {
    const url = `http://localhost:5183/books/${id}`
    const response = await fetch(url, {
        method: "PUT"
    }); 
    console.log(response.status)
}

async function bookDetails(id){
    const url = `http://localhost:5183/books/${id}`; 
    try {
        const response = await fetch(url); 
        if(!response.ok) throw new Error(`Response Status: ${response.status}`); 
        const data = await response.json(); 

        document.getElementById("book-title").textContent = data.title; 
        document.getElementById("book-description").textContent = data.description; 
        document.getElementById("book-author").textContent = `Auteur: ${data.author}`;
        const h3 = document.getElementById("book-quantity"); 
        h3.textContent = `Quantité: ${data.quantity}`; 

        const a = document.createElement("a"); 
        a.href=""; 
        const addQuantityButton = document.createElement("i");
        addQuantityButton.classList.add("fa-solid");
        addQuantityButton.classList.add("fa-plus");
        addQuantityButton.addEventListener("click", () => {
            addQuantity(bookId); 
            location.reload; 
        }); 

        a.appendChild(addQuantityButton)
        h3.appendChild(a); 
    } catch (error) {
        console.log(error.message);
    }
}

async function deleteBook(id) {
    const url = `http://localhost:5183/books/${id}`; 
    try {
        const response = await fetch(url, {
        method: "DELETE", 
        }); 
        console.log(response.status); 
        location.reload; 
    } catch (error) {
        console.log(error.message)
    }
}

bookDetails(bookId); 