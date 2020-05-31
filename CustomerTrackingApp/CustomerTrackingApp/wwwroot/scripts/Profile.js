function main() {
	let userId = document.getElementById("userid-box").value;
	let onlineUser = document.getElementById("online-user-box").value;
	var userHeader = document.getElementsByTagName('header')[0];

	userHeader.style.display = "none";

	tryGetUserById(userId);
}

function tryGetUserById(userId) {
	httpRequest("api/User/GetUserById/?userId=" + userId, "GET", null, handleGetUser, showError.bind(null, "System Error"));
}

function handleGetUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.user = response.Data;
	let user = page.user;

	if (user.Type == 0) {
		user.Type = "Manager";
	}
	else if (user.ManagerId == 1) {
		user.Type = "Admin";
	}
	else if (user.ManagerId == 2) {
		user.Type = "Employee";
	}

	if (user.Gender == 0) {
		user.Gender = "Female";
	}
	else {
		user.Gender = "Male";
	}

	appendUser(user);
}

/*
function handleGetOnlineUser(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	if (response.Data != null) {
		redirect("Home/Index");
	}
	else {
		let loginDiv = document.getElementById("user-login-form");
		loginDiv.style.display = "block";
	}
}*/

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
