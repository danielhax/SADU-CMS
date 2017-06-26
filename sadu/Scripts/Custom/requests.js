$(function () {

    //requests on regular click
    $("#deleteOrgBtn").click(function () {
        var id = $("#orgInfoModal").find(".content").prop("id");

        $.ajax({
            type: "post",
            url: "Organizations/Delete",
            data: { orgId: id },
            success: function (result) {
                console.log(result.Message);
                updateOrganizationsPartialView();
            },
            error: function (xhs) {
                console.log("Cannot run delete request: " + xhs.responseText);
            },
            complete: function () {
                $("#orgInfoModal").modal("hide");
            }
        });
    });

    //submit on clicks
    $('#loginForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            type: "post",
            url: "Session/Login",
            data: $(this).serialize(),
            beforeSend: function () {
                //show progress bar while processing login
                $("#loginBtn").toggle();
                $("#loginForm .progress").removeClass("hidden");
            },
            success: function (data) {
                if (data) {
                    window.location.href = data;
                }
                else {
                    $('#loginAlert').removeClass('hidden');
                    $('#loginAlert').addClass('alert-danger');
                    $('#loginAlert').text("User not found!");
                    $("#loginForm .progress").addClass("hidden");
                    $("#loginBtn").toggle();
                }
            },
            error: function (xhr) {
                $('#loginAlert').removeClass('hidden');
                $('#loginAlert').addClass('alert-danger');
                $('#loginAlert').text("Something went wrong.");
                $("#loginForm .progress").addClass("hidden");
                $("#loginBtn").toggle();
            }
        });
    });

    $('#createSubmissionForm').submit(function (e) {
        e.preventDefault();
        console.log("create submission request");
        
        $.ajax({
            type: "post",
            url: "Submissions/Create",
            data: $(this).serializeArray(),
            success: function (success) {
                if (success) {
                    $("#createSubmissionModal").modal("toggle");
                    updateSubmissionsPartialView();
                    console.log("succesx");
                }
                else {
                    console.log("fail creation");
                }
            },
            error: function (xhs) {
                console.log("error" + xhs.responseText);
            }
        });
    });

    $('#createUserForm').submit(function (e) {
        e.preventDefault();
        console.log("create user request");
        console.log($(this).serialize());
        $.ajax({
            type: "post",
            url: "Users/Create",
            data: $(this).serializeArray(),
            success: function (data) {
                if (data.success) {
                    $("#createUserModal").modal("toggle");
                    updateUsersTable();
                    console.log("succesx");
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (xhs) {
                console.log("error" + xhs.responseText);
            }
        });
    });

    $('#createOrganizationForm').submit(function (e) {
        e.preventDefault();
        console.log("create organization request");
        var fd = new FormData();
        fd.append($("#createOrgImg").attr("name"), $("#createOrgImg").prop("files")[0].name);
        fd.append($("#createOrgName").attr("name"), $("#createOrgName").val());
        $.ajax({
            type: "post",
            url: "Organizations/Create",
            data: fd,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.success) {
                    console.log(data.Message);
                    $("#createOrganizationModal").modal("toggle");
                    updateOrganizationsPartialView();
                    console.log("succesx");
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (xhs) {
                console.log("error" + xhs.responseText);
            }
        });
    });

    $("#updateProfileForm").submit(function () {
        console.log("updae request");
        $.ajax({
            type: "post",
            url: "Users/Edit",
            data: $(this).serialize(),
            success: function (result) {
                console.log(result.Message);
            },
            error: function (xhs) {
                console.log("Cannot edit user profile" + xhs.responseText);
            }
        });
    });
});

//functions
function updateSubmissionsPartialView() {
    $.ajax({
        type: "get",
        url: "Submissions/GetSubmissions",
        success: function (partialView) {
            if (partialView["Message"]) {
                console.log(partialView.Message)
            }
            $("#submissionPartial").html(partialView);
            console.log("view loaded");
        },
        error: function (xhs) {
            console.log(xhs.responseText);
        }
    });
}

function updateUsersTable() {
    $.ajax({
        type: "get",
        url: "Users/GetUsers",
        success: function (partialView) {
            $("#usersTable").html(partialView);
            console.log("table loaded");
        },
        error: function (xhs) {
            console.log(xhs.responseText);
        }
    });
}

function updateUploadsPartialView(submittalId) {
    $.ajax({
        type: "get",
        url: "Submissions/GetUploads/" + submittalId,
        success: function (partialView) {
            if (partialView["Message"]) {
                console.log(partialView.Message);
            }
            else {
                $("#uploadsDiv" + submittalId).html(partialView);
            }
        },
        error: function (xhs) {
            console.log(xhs.responseText);
        }
    });
}

function updateOrganizationsPartialView() {
    $.ajax({
        type: "get",
        url: "Organizations/GetOrganizations",
        success: function (partialView) {
            if (partialView["Message"]) {
                console.log(partialView.Message);
            }
            else {
                $("#organizationPartial").html(partialView);
            }
        },
        error: function (xhr) {
            console.log("Error getting organizations: " + xhr.responseText);
        }
    })
}

function getOrgInfoPartialView(id, orgName) {
    console.log(id);
    $.ajax({
        type: "get",
        url: "Organizations/GetOrganizationInfo/" + id,
        success: function (partialView) {
            $("#orgInfoPartial").html(partialView);
        },
        error: function () {
            console.log("Error getting org info");
        }
    })
}

function deleteUser(id) {
    console.log(id);
    $.ajax({
        type: "post",
        url: "Users/Delete",
        data: { userId: id },
        success: function (result) {
            if (result) {
                updateUsersTable();
            }
            else {
                console.log("Error deleting user");
            }
        },
        error: function (xhr) {
            console.log("error: " + xhr.responseText);
        }
    });
}

function archiveSubmission(id) {
    $.ajax({
        type: "get",
        url: "Submissions/Archive/" + id,
        success: function (result) {
            if (result["Message"]) {
                console.log(result.Message);
            }
            else {
                updateSubmissionsPartialView();
                console.log("Archive successful");
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText);
        }
    })
}

//an object based on the model is needed to be able to be processed by the controller
//function submissionObject(formData) {
//    console.log(formData);
//    var data = [];
//    $.each(formData, function (index, item) {
//        data[item.name] = item.val;
//    });

//    return data;
//}