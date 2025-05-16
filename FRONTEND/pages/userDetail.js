const params = new URLSearchParams(window.location.search); 
const currUserId = params.get("id"); 
let empruntIds = [];

const a = document.getElementById("link-add-button"); 
a.href = `addEmpruntForm.html?id=${currUserId}`;

async function getBookDetails(bookId){
    const url = `http://localhost:5183/books/${bookId}`; 
    const response = await fetch(url); 
    if(!response.ok) throw new Error(`Response status: ${response.status}`);
    const data = await response.json(); 
    return data; 
};

function removeId(idToRemove){
    empruntIds = empruntIds.filter(id => id !== idToRemove); 
};

async function getUserEmprunt(userId) {
    const url = `http://localhost:5183/emprunts/${userId}`;
    
    const response = await fetch(url);
    if(!response.ok) throw new Error(`Response status: ${response.status}`);
    const data = await response.json(); 

    if(data.length > 0){
        const fieldset = document.getElementById("emprunt-container");
        data.forEach(async emprunt => {
            const book = await getBookDetails(emprunt.bookId); 
            
            const div = document.createElement("div");
            
            const input = document.createElement("input"); 
            input.type = "checkbox";
            input.id = `input-emprunt-${book.title.replace(" ", "-")}`;
            input.name = book.title.replace(" ", "-"); 
            input.checked = false; 
            if(input.checked) empruntIds.push(emprunt.id)
                input.addEventListener("change", () => {
            if(input.checked){
                input.setAttribute("checked", "checked");
                empruntIds.push(emprunt.id)
                console.log(empruntIds); 
            }  else {
                input.removeAttribute("checked"); 
                removeId(emprunt.id)
                console.log(empruntIds); 
            } 
        }); 

        const label = document.createElement("label"); 
        const date = new Date(emprunt.created_At); 
        const day = String(date.getDate()).padStart(2, '0'); 
        const month = String(date.getMonth() + 1).padStart(2, '0'); 
        const year = date.getFullYear()
        const hours = String(date.getHours()).padStart(2, '0'); 
        const minutes = String(date.getMinutes()).padStart(2, '0'); 
        const formattedDate = `${day}/${month}/${year} à ${hours}:${minutes}`; 
        label.textContent = `${book.title} - Date de l'emprunt: ${formattedDate}`; 
        
        div.appendChild(input); 
        div.appendChild(label); 
        fieldset.appendChild(div); 
    });
} else {
    const fieldset = document.getElementById("emprunt-container");
    const div = document.createElement("div");
    const p = document.createElement("p"); 
    p.textContent = "Aucun emprunt en cours"

    div.appendChild(p); 
    fieldset.appendChild(div); 
}
    
    const deleteButton = document.getElementById("delete-button"); 
    const closeBtnPopup = document.getElementById("closePopup")
    const overlay = document.getElementById('overlay');
    const popup = document.getElementById('popup');
    const textPopup = document.getElementById("text-popup"); 

    deleteButton.addEventListener("click", (event) => { 
        if (data.length > 0){
            popup.style.display = 'block'
            overlay.style.display = 'block'
            if (data.length > 1) textPopup.textContent = `Impossible de supprimer l'utilisateur car ${data.length} emprunts sont ACTIF !`
            else textPopup.textContent = `Impossible de supprimer l'utilisateur car ${data.length} emprunt est ACTIF !`
        }
        else {
            deleteUser(userId);
            window.location.href = "./userList.html"; 
        }
    }); 
    closeBtnPopup.addEventListener("click", (event) => { 
        popup.style.display = 'none'; 
        overlay.style.display = 'none'; 
    })
    

    const returnEmpruntButton = document.getElementById("return-button"); 
        returnEmpruntButton.addEventListener("click", () => {
            if(empruntIds.length > 0){
                returnEmprunt(empruntIds); 
            }     
            location.reload(); 
        })
};

async function returnEmprunt(ids){
    for (const id of ids) {
        const url = `http://localhost:5183/emprunts/${id}`; 
        try {
            const response = await fetch(url, {
                method: "DELETE"
            }); 
            console.log(response.status); 
        } catch (error) {
            console.log(error.message); 
        }; 
    };
};

async function deleteUser(userId){
    const url = `http://localhost:5183/users/${userId}`;

    const response = await fetch(url, {
        method: "DELETE"
    });
    console.log(response.status);
};

getUserEmprunt(currUserId); 
