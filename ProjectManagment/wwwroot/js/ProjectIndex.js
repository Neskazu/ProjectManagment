$(document).ready(function ()
{
    $('.invite-user-btn').click(function (event)
    {
        var button = $(this);
        var projectId = button.data('project-id');
        var projectName = button.data('project-name');

        // Логируем данные для проверки
        console.log("projectId: " + projectId);
        console.log("projectName: " + projectName);
        // set data for modal view
        $('#projectName').text(projectName);
        $('#projectId').val(projectId);

        // Загрузка содержимого модального окна через AJAX
        $.ajax({
            url: '/Project/InviteUserModal',
            type: 'GET',
            data: { projectId: projectId, projectName: projectName },
            success: function (result)
            {
                $('#inviteUserComponent').html(result);
                $('#inviteUserModal').modal('show');
            }
        });
    });
});