function main() {
	tryGetUsers();

	var modal = document.getElementById("myModal");
	var userHeader = document.getElementsByTagName('header')[0];
	var addUserBtn = document.getElementById("add-user-btn");
	var typeBtn = document.getElementById("type-btn");
	var btn = document.getElementById("create-user-btn");
	var span = document.getElementsByClassName("close")[0];

	typeBtn.onclick = tryGetManagers;
	addUserBtn.onclick = tryInsertUser;
	userHeader.style.display = "none";

	btn.onclick = function () {
		modal.style.display = "block";
	}

	span.onclick = function () {
		modal.style.display = "none";
	}

	window.onclick = function (event) {
		if (event.target == modal) {
			modal.style.display = "none";
		}
	}
}

function tryGetManagers() {
	var userType = document.getElementById("user-type").value;
	if (userType == 2) {
		httpRequest("api/User/GetManagers", "GET", null, handleGetManagers, showError.bind(null, "System Error"));
	}
	/*
	var addBtnDiv = document.createElement("div");
	var addBtn = document.createElement("button");

	document.getElementById("modal-content").appendChild(addBtnDiv);
	addBtnDiv.className = "create-user-div";
	addBtnDiv.id = "add-btn-div";

	document.getElementById("add-btn-div").appendChild(addBtn);
	addBtn.className = "btns";
	addBtn.id = "add-user-btn";
	addBtn.innerHTML = "Add User";
	addBtn.onclick = tryInsertUser;*/
}

function redirectUserProfile(userId) {
	redirect("User/Profile/" + userId);
}

function handleGetManagers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.managers = response.Data;

	//var managerTitleDiv = document.createElement("div");
	//var managerSelect = document.createElement("select");
	var managerOption = document.createElement("option");

	/*document.getElementById("modal-content").appendChild(managerTitleDiv);
	managerTitleDiv.innerHTML = "Select Manager";
	managerTitleDiv.className = "create-user-title";*/

	/*document.getElementById("modal-content").appendChild(managerSelect);
	managerSelect.className = "manager-list";
	managerSelect.id = "manager-list";*/

	var managerList = document.getElementById("manager-list");
	managerList.style.display = "block";

	var managerTitle = document.getElementById("manager-title");
	managerTitle.style.display = "block";

	for (let i = 0; i < page.managers.length; i++) {
		let manager = page.managers[i];
		managerList.appendChild(managerOption);
		managerOption.id = "manager-list";
		managerOption.value = manager.Id;
		managerOption.innerHTML = manager.Username;
    }
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
	let fullname = document.getElementById("user-create-fullname").value;
	let phone = parseInt(document.getElementById("user-create-phone").value);
	var userType = document.getElementById("user-type");
	var user = parseInt(userType.options[userType.selectedIndex].value);
	var gender = document.getElementById("gender");
	var selectedGender = parseInt(gender.options[gender.selectedIndex].value);
	var manager = document.getElementById("manager-list");
	var selectedManager;
	if (manager != undefined) {
		selectedManager = parseInt(manager.options[manager.selectedIndex].value);
    }
	else if (selectedManager == undefined) {
		selectedManager = 0;
    }

	let data = {
		Username: username,
		Email: email,
		Password: password,
		Birthyear: birthYear,
		Fullname: fullname,
		Phone: phone,
		Type: user,
		Gender: selectedGender,
		ManagerId: selectedManager,
		IsActive: 1,
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
	let userProfileUrl = generateHref("User/Profile/##user.Id##")
	let userTemplate = '<div class="user-list-div clearfix" id="user-id-##user.Id##">';
	userTemplate += '<span style="width:200px;" class="user-list-item">##user.Fullname##</span>';
	userTemplate += '<a href="' + userProfileUrl + '" class="user-list-item user-profile-link">##user.Username##</a>';
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
