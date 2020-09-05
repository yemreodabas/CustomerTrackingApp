function main() {
	tryGetCustomer();

	var modal = document.getElementById("myModal");
	var modalBackgrandDiv = document.getElementById("modal-backgrand");
	var userHeader = document.getElementsByTagName('header')[0];
	var addCustomerBtn = document.getElementById("add-customer-btn");
	var btn = document.getElementById("create-customer-btn");
	var span = document.getElementsByClassName("close")[0];

	addCustomerBtn.onclick = tryInsertCustomer;
	userHeader.style.display = "none";

	btn.onclick = function () {
		modal.style.display = "block";
		modalBackgrandDiv.style.display = "block";
	}

	span.onclick = function () {
		modal.style.display = "none";
		modalBackgrandDiv.style.display = "none";
	}

	window.onclick = function (event) {
		if (event.target == modal) {
			modal.style.display = "none";
			modalBackgrandDiv.style.display = "none";
		}
	}
}

function tryGetCustomer() {
	httpRequest("api/Customer/GetActiveCustomers", "GET", null, handleGetCustomers, showError.bind(null, "System Error"));
}

function tryGetCustomersByPageNumber(pageNumber) {
	httpRequest("api/Customer/GetCustomersByPageNumber/?pageNumber=" + pageNumber, "GET", null, handleGetCustomersByPage, showError.bind(null, "System Error"));
}

function handleGetCustomers(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.customers = response.Data;

	pagination();
	tryGetCustomersByPageNumber(1);
}

function pagination() {
	var pageCount = page.customers.length / 5;
	var pageNumbers = document.getElementById("page-numbers");

	for (let i = 1; i < pageCount + 1; i++) {
		var pageNumberBtn = document.createElement("button");
		pageNumbers.appendChild(pageNumberBtn);
		pageNumberBtn.id = "page-number-" + i;
		pageNumberBtn.classList.add("page-number-btn");
		pageNumberBtn.innerHTML = i;
		pageNumberBtn.onclick = tryGetCustomersByPageNumber.bind(null, i);
	}
}

function handleGetCustomersByPage(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let userListDiv = document.getElementById("user-list-element");
	userListDiv.innerHTML = "";

	let pageBtn = document.getElementById("user-list-element");

	page.customersByPage = response.Data;

	for (let i = 0; i < page.customersByPage.length; i++) {
		let customer = page.customersByPage[i];

		appendCustomer(customer);
	}
}

function tryInsertCustomer() {
	let fullname = document.getElementById("customer-fullname").value;
	let phone = parseInt(document.getElementById("customer-phone").value);
	let data = {
		Fullname: fullname,
		Phone: phone,
	};

	httpRequest("api/Customer/CreateCustomer", "POST", data, handleInsertCustomer, showError.bind(null, "System Error"));
}

function handleInsertCustomer(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	var modal = document.getElementById("myModal");
	var modalBackgrandDiv = document.getElementById("modal-backgrand");
	let customer = response.Data;

	appendCustomer(customer);

	modal.style.display = "none";
	modalBackgrandDiv.style.display = "none";
}

function appendCustomer(customer) {
	let customerProfileUrl = generateHref("Customer/CustomerProfile/##customer.Id##")
	let customerTemplate = '<div class="user-list-div clearfix" id="customer-id-##customer.Id##">';
	customerTemplate += '<a href="' + customerProfileUrl + '" class="customer-list-item user-profile-link">##customer.Fullname##</a>';
	customerTemplate += '<span class="customer-list-item">##customer.Phone##</span>';
	customerTemplate += '</div>';

	let customerHtmlString = customerTemplate
		.split("##customer.Id##").join(customer.Id)//.replace("##user.Id##", userModel.Id)
		.split("##customer.Fullname##").join(customer.Fullname)//.replace("##user.Username##", userModel.Username)
		.split("##customer.Phone##").join(customer.Phone)//.replace("##user.Email##", userModel.Email)

	let customerHtml = toDom(customerHtmlString);

	let userListDiv = document.getElementById("user-list-element");
	userListDiv.appendChild(customerHtml);
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
