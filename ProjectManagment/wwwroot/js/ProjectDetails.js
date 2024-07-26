function closeComments()
{
    $('#commentsPanel').removeClass('show');
    $('#commentsContainer').html('');
    $('#newCommentContent').val('');
}
function showComments(taskId)
{
    var commentsPanel = $('#commentsPanel');

    if (commentsPanel.hasClass('show'))
    {
        closeComments();
    } else
    {
        $.ajax({
            url: '/Task/GetComments',
            type: 'GET',
            data: { taskId: taskId },
            success: function (result) {
                $('#commentsPanel').html(result);
                $('#commentsPanel').addClass('show');
            }
        });
    }
}

function addComment(taskId) {
    var content = $('#newCommentContent').val();
    $.ajax({
        url: '/Task/AddComment',
        type: 'POST',
        data: {
            taskId: taskId,
            content: content,
        },
        success: function (response) {
            if (response.success) {
                showComments(taskId);
            } else {
                alert(response.message);
            }
        }
    });
}
function allowDrop(event)
{
    event.preventDefault();
}

function drag(event)
{
    event.dataTransfer.setData("text", event.target.getAttribute("data-user-id"));
}

function drop(event)
{
    event.preventDefault();
    var userId = event.dataTransfer.getData("text");
    var taskId = event.target.closest('.task-item').getAttribute("data-task-id");
    console.log("Dropping user with ID:", userId);
    console.log("On task with ID:", taskId);

    // Validate data
    if (!userId || !taskId) {
        console.error('Invalid user or task ID');
        return;
    }

  
    $.ajax({
        url: '/Task/AssignUser',
        type: 'POST',
        data: {
            taskId: taskId,
            userId: userId
        },
        success: function (response) {
            if (response.success) {
                location.reload(); 
            } else {
                alert(response.message); 
                console.error('Assignment failed:', response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Assignment failed:', error);
        }
    });
}

$(document).ready(function ()
{
    var userItems = document.querySelectorAll('.user-item');
    userItems.forEach(function (item) {
        item.setAttribute("draggable", "true");
        item.addEventListener('dragstart', drag);
    });

    var taskItems = document.querySelectorAll('.task-item');
    taskItems.forEach(function (item) {
        item.addEventListener('dragover', allowDrop);
        item.addEventListener('drop', drop);
    });
});

$(document).ready(function ()
{
    $('.btn-danger').click(function (event)
    {
        var button = $(this);
        var taskId = button.data('task-id');
        var taskTitle = button.data('task-title');

        // set data for modal view
        $('#taskTitle').text(taskTitle);
        $('#taskId').val(taskId);

        // Load modal view by ajax
        $.ajax({
            url: '/Task/DeleteTaskModal',
            type: 'GET',
            data: { taskId: taskId, taskTitle: taskTitle },
            success: function (result) {
                $('#deleteTaskComponent').html(result);
                $('#deleteModal').modal('show');
            }
        });
    });
});