function main() {
	/*let userCreateBtn = document.getElementById("create-user-btn");
	userCreateBtn.onclick = tryInsertUser;*/

	tryGetUsers();
}

function tryGetUsers() {
	httpRequest("api/User/GetActiveUsers", "GET", null, handleGetUsers, showError.bind(null, "System Error"));
}

function handleGetUsers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.users = response.Data;

	for (let i = 0; i < page.users.length; i++) {
		let user = page.users[i];
		appendUser(user);
	}
}

function tryInsertUser() {
	let username = document.getElementById("user-create-username").value;
	let email = document.getElementById("user-create-email").value;
	let password = document.getElementById("user-create-password").value;
	let birthYear = parseInt(document.getElementById("user-create-birthyear").value);

	let data = {
		Username: username,
		Email: email,
		Password: password,
		Birthyear: birthYear,
	};

	httpRequest("api/User/CreateUser", "POST", data, handleInsertUser, showError.bind(null, "System Error"));
}

function handleInsertUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let user = response.Data;
	appendUser(user);
}

function appendUser(user) {
	let userTemplate = '<div class="user-list-div clearfix" id="user-id-##user.Id##">';
	userTemplate += '<span style="width:250px;" class="user-list-item">##user.Fullname##</span>';
	userTemplate += '<span class="user-list-item">##user.Username##</span>';
	userTemplate += '<span class="user-list-item">##user.Email##</span>';
	userTemplate += '<span class="user-list-item">##user.Phone##</span>';
	userTemplate += '<span class="user-list-item">##user.Type##</span>';
	userTemplate += '<span class="user-list-item">##user.ManagerId##</span>';
	userTemplate += '<span class="user-list-item">##user.Gender##</span>';
	userTemplate += '<span class="user-list-item">##user.IsActive##</span>';
	userTemplate += '<span class="user-list-item">##user.BirthYear##</span>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Username##").join(user.Username)//.replace("##user.Username##", userModel.Username)
		.split("##user.Fullname##").join(user.Fullname)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.BirthYear##").join(user.BirthYear)//.replace("##user.Email##", userModel.Email)
		.split("##user.Phone##").join(user.Phone)//.replace("##user.Email##", userModel.Email)
		.split("##user.Type##").join(user.Type)//.replace("##user.Email##", userModel.Email)
		.split("##user.Gender##").join(user.Gender)//.replace("##user.Email##", userModel.Email)
		.split("##user.IsActive##").join(user.IsActive)//.replace("##user.Email##", userModel.Email)
		.split("##user.ManagerId##").join(user.ManagerId)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("user-list");
	userListDiv.appendChild(userHtml);
}

function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}
