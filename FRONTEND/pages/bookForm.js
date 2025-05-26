
const addBookButton = document.getElementById("add-book-button"); 
addBookButton.addEventListener("click", (event) => {
    event.preventDefault(); 

    const inputTitle  = document.getElementById("input-title"); 
    const title = inputTitle.value; 

    const inputDescription  = document.getElementById("input-description"); 
    const description = inputDescription.value; 

    const inputAuthor  = document.getElementById("input-author"); 
    const author = inputAuthor.value; 

    const inputPicture  = document.getElementById("input-picture"); 
    const picture = inputPicture.value; 

    createBook(title, description, author, picture);
    window.location.href = "http://localhost:5500/bookList.html"; 
})

async function createBook(title, description, author, picture) {
    const book = {
        title: title, 
        description: description, 
        author: author, 
        picture: picture
    }; 

    const url = "http://localhost:5183/books"
    const response = await fetch(url, {
        method: "POST", 
        headers: {
            "Content-Type": "application/json"
        }, 
        body: JSON.stringify(book)
    }); 
    console.log(response.status); 
}

