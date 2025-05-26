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

            
            const col = document.createElement("div")
            col.classList.add("col-md-2"); 
            col.classList.add("mb-4"); 

            const img = document.createElement("img"); 
            img.src = book.picture; 
            img.classList.add("card-img-top", "img-cover"); 
            
            const divCard = document.createElement("div");
            divCard.classList.add("card", "h-100"); 
            divCard.style.width = "100%"; 

            const divBody = document.createElement("div")
            divBody.classList.add("card-body", "d-flex", "flex-column")

            const cardTitle = document.createElement("h5")
            if (book.quantity > 0) cardTitle.textContent = `${book.title}`
            else  cardTitle.textContent = `${book.title} (En rupture)` // TODO color red
            cardTitle.classList.add("mb-3")

            const a = document.createElement("a"); 
            a.classList.add("text-decoration-none", "btn", "btn-success", "mt-auto", "w-50"); 
            a.href = `./bookDetail.html?id=${bookId}`; 
            a.textContent = "Détail"; 


            divBody.appendChild(cardTitle)
            divBody.appendChild(a)

            divCard.appendChild(img)
            divCard.appendChild(divBody)

            col.appendChild(divCard)
            bookList.appendChild(col)
            // link.appendChild(li)
            // div.appendChild(img)
            // div.appendChild(link)
            // bookList.appendChild(div)
        });
    } catch (error) {
        console.log(error.message)
    }
}

getAllBooks(); 