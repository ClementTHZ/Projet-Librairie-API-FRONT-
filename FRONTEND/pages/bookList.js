async function getAllBooks() {
    const url = "http://localhost:5183/books"; 
    try {
        const response = await fetch(url); 
        if(!response.ok) throw new Error(`Response Status: ${response.status}`)
        const data = await response.json(); 
        console.log(data)

        const bookList = document.getElementById("book-list")
        data.forEach(book => {
            const li = document.createElement("li")
            li.textContent = `${book.title}`
            bookList.appendChild(li)
        });
    } catch (error) {
        console.log(error.message)
    }
}

getAllBooks(); 