function main() {
	page.customerId = document.getElementById("userid-box").value;
	page.onlineUser = document.getElementById("online-user-box").value;

	var header = document.getElementsByTagName('header')[0];
	page.modalBackgrandDiv = document.getElementById("modal-backgrand");
	var addPurchaseBtn = document.getElementById("modal-add-purchase-btn");
	var addPaymentBtn = document.getElementById("modal-add-payment-btn");
	var addProductReturnBtn = document.getElementById("modal-add-product-return-btn");

	addPurchaseBtn.onclick = tryInsertPurchase;
	addPaymentBtn.onclick = tryInsertPayment;
	//addProductReturnBtn.onclick = tryInsertProductReturn;

	header.style.display = "none";

	purchaseModal();
	paymentModal();
	productReturnModal();

	tryGetCustomerCurrentDept(page.customerId);
	
	tryGetActivityByCustomerId(page.customerId);
}
function purchaseModal() {
	var purchaseBtn = document.getElementById("add-purchase-btn");
	var purchaseModal = document.getElementById("purchase-modal");
	var closeSpan = document.getElementById("close-purchase");
	var modalBackgrandDiv = page.modalBackgrandDiv;

	purchaseBtn.onclick = function () {
		purchaseModal.style.display = "block";
		modalBackgrandDiv.style.display = "block";
	}

	closeSpan.onclick = function () {
		purchaseModal.style.display = "none";
		modalBackgrandDiv.style.display = "none";
	}

	window.onclick = function (event) {
		if (event.target == purchaseModal) {
			purchaseModal.style.display = "none";
			modalBackgrandDiv.style.display = "none";
		}
	}
}

function paymentModal() {
	var paymentBtn = document.getElementById("add-payment-btn");
	var paymentModal = document.getElementById("payment-modal");
	var closeSpan = document.getElementById("close-payment");
	var modalBackgrandDiv = page.modalBackgrandDiv;

	paymentBtn.onclick = function () {
		paymentModal.style.display = "block";
		modalBackgrandDiv.style.display = "block";
	}

	closeSpan.onclick = function () {
		paymentModal.style.display = "none";
		modalBackgrandDiv.style.display = "none";
	}

	window.onclick = function (event) {
		if (event.target == paymentModal) {
			paymentModal.style.display = "none";
			modalBackgrandDiv.style.display = "none";
		}
	}
}

function productReturnModal() {
	var productReturnBtn = document.getElementById("add-product-return-btn");
	var productReturnModal = document.getElementById("product-return-modal");
	var closeSpan = document.getElementById("close-product-return");
	var modalBackgrandDiv = page.modalBackgrandDiv;

	productReturnBtn.onclick = function () {
		productReturnModal.style.display = "block";
		modalBackgrandDiv.style.display = "block";
	}

	closeSpan.onclick = function () {
		productReturnModal.style.display = "none";
		modalBackgrandDiv.style.display = "none";
	}

	window.onclick = function (event) {
		if (event.target == productReturnModal) {
			productReturnModal.style.display = "none";
			modalBackgrandDiv.style.display = "none";
		}
	}
}

function tryGetCustomer(customerId) {
	httpRequest("api/Customer/GetCustomerById/?customerId=" + customerId, "GET", null, handleGetCustomer, showError.bind(null, "System Error"));
}

function tryGetActivityByCustomerId(customerId) {
	httpRequest("api/Customer/GetActivityByCustomerId/?customerId=" + customerId, "GET", null, handleGetActivities, showError.bind(null, "System Error"));
}

function tryGetCustomerCurrentDept(customerId) {
	httpRequest("api/Customer/GetCustomerCurrentDept/?customerId=" + customerId, "GET", null, handleGetCurrentDept, showError.bind(null, "System Error"));
}

function handleGetCurrentDept(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.lastActivity = response.Data;
	if (page.lastActivity == undefined || page.lastActivity == null) {
		page.currentDept = 0;
	}

	else if (page.lastActivity != undefined || page.lastActivity == null) {
		page.currentDept = page.lastActivity.CurrentDept;
	}

	tryGetCustomer(page.customerId);
}

function handleGetCustomer(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.user = response.Data;
	let user = page.user;

	let customerDiv = document.getElementById("customer-info");
	customerDiv.innerHTML = "";

	appendUser(user, page.currentDept);
}

function handleGetActivities(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	page.activities = response.Data;

	for (let i = 0; i < page.activities.length; i++) {
		let activity = page.activities[i];

		if (activity.ActivityType == 0) {
			activity.ActivityType = "Purchase";
		}

		else if (activity.ActivityType == 1) {
			activity.ActivityType = "Payment";
		}

		else if (activity.ActivityType == 2) {
			activity.ActivityType = "Product Return";
		}

		else if (activity.ActivityType == 3) {
			activity.ActivityType = "Payment Return";
		}

		appendActivity(activity);
    }

}

function tryInsertPurchase() {
	page.description = document.getElementById("purchase-description").value;
	if (page.description == undefined || page.description == null) {
		showPurchaseError.bind(null, "Description Cannot Be Empty")
	}

	page.amount = parseFloat(document.getElementById("purchase-amount").value);
	page.paymentTotal = parseFloat(document.getElementById("purchase-payment-total").value);
	page.customerId = parseInt(page.customerId);
	page.userId = parseInt(page.onlineUser);
	/*page.currentDept += amount - purchasePaymentTotal;
	let lastDept = page.currentDept;*/

	let data = {
		UserId: page.userId,
		Amount: page.amount,
		CustomerId: page.customerId,
		CurrentDept: page.amount,
		Description: page.description,
		ActivityType: 0,
	};

	httpRequest("api/Customer/CreateActivity", "POST", data, handleInsertActivity, showError.bind(null, "System Error"));

	tryInsertPurchasePayment();
}

function tryInsertPurchasePayment() {
	if (page.paymentTotal != 0) {
		let description = page.paymentTotal + " tahsil edildi";

		let data = {
			UserId: page.userId,
			Amount: page.paymentTotal,
			CustomerId: page.customerId,
			CurrentDept: page.paymentTotal,
			Description: description,
			ActivityType: 1,
		};

		httpRequest("api/Customer/CreateActivity", "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
	}
}

function tryInsertPayment() {
	let description = document.getElementById("payment-description").value;
	if (description == undefined || description == null) {
		showPaymentError.bind(null, "Description Cannot Be Empty");
	}

	let amount = parseFloat(document.getElementById("payment-amount").value);
	if (amount == undefined || amount == null) {
		showPaymentError.bind(null, "Amount Cannot Be Empty");
    }
	let customerId = parseInt(page.customerId);
	let userId = parseInt(page.onlineUser);
	/*page.currentDept -= amount;
	let lastDept = page.currentDept;*/

	let data = {
		UserId: userId,
		Amount: amount,
		CustomerId: customerId,
		CurrentDept: amount,
		Description: description,
		ActivityType: 1,
	};

	httpRequest("api/Customer/CreateActivity", "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
}

function tryInsertProductReturn() {
	let description = document.getElementById("product-return-description").value;
	if (description == undefined || description == null) {
		showPaymentError.bind(null, "Description Cannot Be Empty");
	}

	let amount = parseFloat(document.getElementById("product-total-amount").value);
	if (amount == undefined || amount == null) {
		showPaymentError.bind(null, "Amount Cannot Be Empty");
	}
	let customerId = parseInt(page.customerId);
	let userId = parseInt(page.onlineUser);
	/*page.currentDept -= amount;
	let lastDept = page.currentDept;*/

	let data = {
		UserId: userId,
		Amount: amount,
		CustomerId: customerId,
		CurrentDept: amount,
		Description: description,
		ActivityType: 2,
	};

	httpRequest("api/Customer/CreateActivity", "POST", data, handleInsertActivity, showError.bind(null, "System Error"));
}

function handleInsertActivity(response) {
	if (!response.Success) {
		showError(response.ErrorMessage);
		return;
	}

	let activity = response.Data;

	if (activity.ActivityType == 0) {
		activity.ActivityType = "Purchase";
	}

	else if (activity.ActivityType == 1) {
		activity.ActivityType = "Payment";
	}

	else if (activity.ActivityType == 2) {
		activity.ActivityType = "Product Return";
	}

	else if (activity.ActivityType == 3) {
		activity.ActivityType = "Payment Return";
	}
	
	appendActivity(activity);

	tryGetCustomerCurrentDept(page.customerId);
}

function appendUser(user, currentDept) {
	let userTemplate = '<div class="customer-list-div clearfix" id="user-id-##user.Id##">';
	userTemplate += '<span style="width:200px;" class="customer-list-item">##user.Fullname##</span>';
	userTemplate += '<span class="customer-list-item">Phone: ##user.Phone##</span>';
	userTemplate += '<span class="customer-list-item" id="customer-current-dept">Current Dept: ##currentDept## $</span>';
	userTemplate += '</div>';

	let userHtmlString = userTemplate
		.split("##user.Id##").join(user.Id)//.replace("##user.Id##", userModel.Id)
		.split("##user.Fullname##").join(user.Fullname)//.replace("##user.Username##", userModel.Username)
		.split("##user.Email##").join(user.Email)//.replace("##user.Email##", userModel.Email)
		.split("##user.Phone##").join(user.Phone)//.replace("##user.Email##", userModel.Email)
		.split("##currentDept##").join(currentDept)//.replace("##user.Email##", userModel.Email)

	let userHtml = toDom(userHtmlString);

	let userListDiv = document.getElementById("customer-info");
	userListDiv.appendChild(userHtml);
}

function appendActivity(activity) {
	let activityTemplate = '<div class="customer-list-div clearfix" id="activity-id-##activity.Id##">'; 
	activityTemplate += '<span style="width:250px;" class="customer-list-item">##activity.ActivityType##</span>';
	activityTemplate += '<span class="customer-list-item">##activity.Description##</span>';
	activityTemplate += '<span class="customer-list-item">##activity.Amount## $</span>';
	activityTemplate += '</div>';

	let activityHtmlString = activityTemplate
		.split("##activity.Id##").join(activity.Id)//.replace("##user.Id##", userModel.Id)
		.split("##activity.Description##").join(activity.Description)//.replace("##user.Username##", userModel.Username)
		.split("##activity.ActivityType##").join(activity.ActivityType)//.replace("##user.Username##", userModel.Username)
		.split("##activity.Amount##").join(activity.Amount)//.replace("##user.Email##", userModel.Email)

	let activityHtml = toDom(activityHtmlString);

	let activityListDiv = document.getElementById("activity-list");
	activityListDiv.prepend(activityHtml);

	let operationType = document.getElementById("activity-id-" + activity.Id);

	if (activity.ActivityType == "Purchase") {
		operationType.classList.add("purchase-div");
	}

	else if (activity.ActivityType == "Payment") {
		operationType.classList.add("payment-div");
	}

	else if (activity.ActivityType == "ProductReturn") {
		operationType.classList.add("product-return-div");
	}
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

function showPurchaseError(message) {
	let errorDiv = document.getElementById("error-purchase");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function showPaymentError(message) {
	let errorDiv = document.getElementById("error-payment");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}

function showProductReturnError(message) {
	let errorDiv = document.getElementById("error-product");
	errorDiv.innerHTML = message;
	errorDiv.style.display = "block";
}