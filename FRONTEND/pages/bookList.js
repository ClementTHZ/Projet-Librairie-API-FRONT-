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
            if (book.quantity > 0) li.textContent = `${book.title} / auteur: ${book.author}`
            else li.textContent = `${book.title} / auteur: ${book.author} (En rupture)`
 
            link.appendChild(li)
            bookList.appendChild(link)
        });
    } catch (error) {
        console.log(error.message)
    }
}

getAllBooks(); 