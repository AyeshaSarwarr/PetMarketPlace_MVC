function getSelectOptions() {
    $.ajax({
        url: '/Seller/SelectOptions', // URL of the action
        type: 'GET', // HTTP method
        success: function (response) {
            // Handle the response from the server
            $('#select-options-container').html(response); // For example, insert the returned HTML into a container
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.error('Error:', error);
            alert('An error occurred while fetching the options.');
        }
    });
}
function postLogin() {
    var username = $('#username').val(); // Get the username from the input field
    var password = $('#password').val(); // Get the password from the input field

    $.ajax({
        url: '/Seller/Login', // URL of the action
        type: 'POST', // HTTP method
        data: {
            Username: username,
            Password: password
        },
        success: function (response) {
            // Redirect to another page or show success message
            if (response.success) {
                window.location.href = response.redirectUrl;
            } else {
                $('#error-message').text(response.errorMessage); // Show error message
            }
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.error('Error:', error);
            alert('An error occurred during login.');
        }
    });
}
function postSignup() {
    var username = $('#username').val();
    var password = $('#password').val();
    var businessName = $('#businessName').val();
    var businessAddress = $('#businessAddress').val();
    var contactNumber = $('#contactNumber').val();
    var website = $('#website').val();

    $.ajax({
        url: '/Seller/Signup',
        type: 'POST',
        data: {
            username: username,
            password: password,
            businessName: businessName,
            businessAddress: businessAddress,
            contactNumber: contactNumber,
            website: website
        },
        success: function (response) {
            // Redirect to login page or show success message
            if (response.success) {
                window.location.href = '/Seller/Login';
            } else {
                $('#error-message').text(response.errorMessage);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            alert('An error occurred during signup.');
        }
    });
}
function ajaxGet(url, successCallback, errorCallback) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            if (successCallback) {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (errorCallback) {
                errorCallback(xhr, status, error);
            } else {
                console.error("Error: ", xhr.responseText);
            }
        }
    });
}
ajaxGet('/PetSpecification/AddCatSpecification', function (response) {
    // Handle the response, e.g., load the form data into a modal
    $('#formContainer').html(response);
}, function (error) {
    alert("Failed to load the form. Please try again.");
});
function ajaxPost(url, data, successCallback, errorCallback) {
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (response) {
            if (successCallback) {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (errorCallback) {
                errorCallback(xhr, status, error);
            } else {
                console.error("Error: ", xhr.responseText);
            }
        }
    });
}
$('#addCatForm').on('submit', function (e) {
    e.preventDefault();
    var formData = $(this).serialize();
    ajaxPost('/PetSpecification/AddCatSpecification', formData, function (response) {
        alert("Cat added successfully!");
        window.location.href = "/PetSpecification/Success";
    }, function (error) {
        alert("Failed to add cat. Please check your inputs.");
    });
});
function ajaxFileUpload(url, formData, successCallback, errorCallback) {
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (successCallback) {
                successCallback(response);
            }
        },
        error: function (xhr, status, error) {
            if (errorCallback) {
                errorCallback(xhr, status, error);
            } else {
                console.error("Error: ", xhr.responseText);
            }
        }
    });
}
$('#addDogForm').on('submit', function (e) {
    e.preventDefault();
    var formData = new FormData(this);
    ajaxFileUpload('/PetSpecification/AddDogSpecification', formData, function (response) {
        alert("Dog added successfully!");
        window.location.href = "/PetSpecification/Success";
    }, function (error) {
        alert("Failed to add dog. Please check your inputs.");
    });
});
