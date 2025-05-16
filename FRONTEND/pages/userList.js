async function getAllUSers() {
    const url = "http://localhost:5183/users"; 
    try {
        const response = await fetch(url); 
        if(!response.ok) throw new Error (`Response status: ${response.status}`);
        const data = await response.json(); 
    
        const userList = document.getElementById("user-list");
        data.forEach(user => {
            const userId = user.id; 
    
            const li = document.createElement("li"); 
            li.textContent = `${user.firstName} ${user.lastName}`;
            li.classList.add("text-black"); 

            const a = document.createElement("a"); 
            a.href = `./userDetails.html?id=${userId}`; 
            a.classList.add("text-decoration-none")
            
            a.appendChild(li); 
            userList.appendChild(a);
        });
    } catch (error) {
        console.log(error.message); 
    }
}

getAllUSers(); 