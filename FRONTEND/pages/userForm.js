const addUserButton = document.getElementById("add-user-button"); 
addUserButton.addEventListener("click", (event) => {
    event.preventDefault(); 
    const inputFirstName = document.getElementById("input-firstname")
    const firstName = inputFirstName.value; 
    
    const inputLastName = document.getElementById("input-lastname")
    const lastName = inputLastName.value; 

    const inputAge = document.getElementById("input-age")
    const age = inputAge.value; 

    createUser(firstName, lastName, age); 

    window.location.href = "./userList.html"; 
})

async function createUser(firstName, lastName, age){
    const user = {
        firstName: firstName, 
        lastName: lastName, 
        age: age
    }

    const url = "http://localhost:5183/users"

    const response = await fetch(url, {
        method: "POST", 
        headers: {
            "content-type": "application/json"
        }, 
        body: JSON.stringify(user)
    }); 
    console.log(response.status)
}