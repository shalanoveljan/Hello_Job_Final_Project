
let similar_Resume = document.getElementById("similar-resume");

function MyFunction() {
    const id = getCurrentOptionId();
    const dto = getFilterDto(); 

    console.log(dto)

    $.ajax({
        type: 'Post',
        url: `https://localhost:7066/Course/SortCourses/${id}`,
        data: dto,
        success: function (data) {
            similar_Resume.innerHTML = data;
        }
    });
}

function getCurrentOptionId() {
    var selectElement = document.getElementById("sort");
    var selectedIndex = selectElement.selectedIndex;
    var selectedOption = selectElement.options[selectedIndex];
    var selectedOptionId = selectedOption.value;
    return selectedOptionId;
}
$(document).ready(function () {
    $("input[name='CategoriesIds'], input[name='Agencies'], #paid, #certificate_1, #retire_1, input[name='Selected_Lesson_Mode'], #slider-1, #slider-2, input[name='Level']").change(function (e) {
        e.preventDefault();

        var dto = getFilterDto(); 

        $.ajax({
            url: '/Course/FilterCourses',
            type: 'POST',
            data: {
                dto: dto
            },
            success: function (data) {
                similar_Resume.innerHTML = data;
                var courseCountValue = $("#courseCount").val();
                $('#count_items_head').text(courseCountValue);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

function getFilterDto() {
    var categoriesIds = [];
    $("input[name='CategoriesIds']:checked").each(function () {
        categoriesIds.push(parseInt($(this).val()));
    });

    var agencies = [];
    $("input[name='Agencies']:checked").each(function () {
        agencies.push($(this).val());
    });

    var isFree = $("#paid").is(":checked");
    var isSertificate = $("#certificate_1").is(":checked");
    var isRetirement = $("#retire_1").is(":checked");
    var selectedLessonMode = [];
    $("input[name='Selected_Lesson_Mode']:checked").each(function () {
        selectedLessonMode.push(parseInt($(this).val()));
    });

    var minTime = parseInt($("#slider-1").val());
    var maxTime = parseInt($("#slider-2").val());
    var level = [];
    $("input[name='Level']:checked").each(function () {
        level.push(parseInt($(this).val()));
    });

    return {
        CategoriesIds: categoriesIds,
        Agencies: agencies,
        IsFree: isFree,
        IsSertificate: isSertificate,
        IsRetirement: isRetirement,
        Selected_Lesson_Mode: selectedLessonMode,
        MinTime: minTime,
        MaxTime: maxTime,
        Level: level
    };
}


