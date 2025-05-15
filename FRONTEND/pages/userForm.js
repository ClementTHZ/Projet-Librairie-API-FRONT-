
const params = new URLSearchParams(window.location.search); 
const userId = params.get("id"); 
let bookIds = []; 

const backUserButton = document.getElementById("back-button"); 
backUserButton.href = `./userDetails.html?id=${userId}`

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
            label.textContent = `${book.title} / auteur: ${book.author} `

            div.appendChild(input); 
            div.appendChild(label); 
            bookList.appendChild(div); 
        });
    } catch (error) {
        console.log(error.message)
    }
}

getAllBooksAvailable(); 