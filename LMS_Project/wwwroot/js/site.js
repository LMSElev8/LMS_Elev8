
function enrollFunc(i) 
{
    $.ajax({
        url: '/CourseList/Index',
        type: 'POST',
        data: {
            id: i
        },
        success: function() {
            window.location.reload();
        }
    });
}