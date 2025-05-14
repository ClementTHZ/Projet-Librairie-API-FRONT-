async function getAllBooks() {
    const url = "http://localhost:5183/books"; 
    try {
        const response = await fetch(url); 
        if(!response.ok) throw new Error(`Response Status: ${response.status}`)
        const data = await response.json(); 
        console.log(data)

        const bookList = document.getElementById("book-list")
        data.forEach(book => {
            const bookId = book.id

            const link = document.createElement("a")
            link.href = `./bookDetail.html?id=${bookId}`

            const li = document.createElement("li")
            li.textContent = `${book.title} / auteur: ${book.author} `

            const deleteTrash = document.createElement("i"); 
            deleteTrash.classList.add("fa-solid"); 
            deleteTrash.classList.add("fa-trash");
            deleteTrash.addEventListener("click", () => {
                deleteBook(bookId)
            }); 
            const a = document.createElement("a"); 
            a.href = ""; 

            a.appendChild(deleteTrash)
            li.appendChild(a)
            link.appendChild(li)
            bookList.appendChild(link)
        });
    } catch (error) {
        console.log(error.message)
    }
}

async function deleteBook(bookIdId) {
    const url = `http://localhost:5183/books/${bookId}`; 
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

// Faire une fonction getBookbyId qui récupère l'id du livre cliquer
// Faire une fonction

getAllBooks(); 