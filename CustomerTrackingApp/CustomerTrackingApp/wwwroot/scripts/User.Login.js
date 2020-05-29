function main() {
	let userCreateBtn = document.getElementById("user-login-btn");
	userCreateBtn.onclick = tryLogin;

	let onlineUser = document.getElementById("online-user-box").value;

	if (onlineUser != null && onlineUser != undefined && onlineUser != "") {

		let loginForum = document.getElementById("user-login-form");
		loginForum.style.display = "none";
	}
}

function tryGetOnlineUser() {
	httpRequest("api/User/GetOnlineUser", "GET", null, handleGetOnlineUser, showError.bind(null, "System Error"));
}

function tryLogin() {
	let username = document.getElementById("user-login-username").value;
	let password = document.getElementById("user-login-password").value;

	let data = {
		Username: username,
		Password: password,
	};

	httpRequest("api/User/Login", "POST", data, handleLogin, showError.bind(null, "System Error"));
}

function handleLogin(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	redirect("Home/Index");
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

function showError(message) {
	let errorDiv = document.getElementById("error");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function hideError() {
	let errorDiv = document.getElementById("error");
	errorDiv.style.display = "none";
}
