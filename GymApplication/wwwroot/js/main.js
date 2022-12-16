AOS.init({
    once: true
});


function validateFileType() {
    let input = document.getElementById("photo");
    let files = document.getElementById("photo").files;

    for (let i = 0; i < files.length; i = i + 1) {
        let fileName = files[i].name;
        let idxDot = fileName.lastIndexOf(".") + 1;
        let extFile = fileName.substr(idxDot, fileName.length).toLowerCase();

        if (extFile != "jpg" & extFile != "jpeg" & extFile != "png") {
            alert("Могут быть загружены только файлы типов: .jpeg, .jpg, .png!");
            input.value = null;
            return;
        }
    }
}


// add | delete price

function addPrice() {
    $("#prices-block").append('<div class="semantic-block"> <div class="form-group"> <label> Количество посещений: <input name="CountsOfVisits" type="number" placeholder="Введите количество посещений" min="1" max="1000000" required /> </label> </div> <div class="form-group"> <label>Цена: <input name="Prices" id="price" type="number" placeholder="Введите цену в рублях" min="0" max="1000000" required /> </label> </div> </div>');
}

function deletePrice() {
    let blocks = document.getElementsByClassName("semantic-block");
    let lastBlock = blocks[blocks.length - 1];
    lastBlock.remove();
}


// add | delete schedule day

function addDay(pattern) {
    let id = 0;
    let block = document.getElementsByClassName("day");
    
    if (block.length != 0) {
        id = parseInt((block[block.length - 1].id.slice(3))) + 1;
    }
    
    $("#days-block").append('<div id="day' + id + '" class="semantic-block day"> <span class="textbox" style="font-weight: 500;">День №' + (id + 1) + '</span> <input name="schedule.ScheduleDays[' + id + '].DayNumber" type="hidden" readonly required value="' + (id + 1) + '" /> <div class="workouts-block"> </div> <div class="flex-controls right"> <div class="delete-button button" onclick="deleteWorkout(' + id + ')">Удалить последнее занятие </div> <div class="button" onclick="addWorkout(' + id + ', \'' + pattern + '\')">Добавить занятие</div> </div> </div>');
}

function deleteDay() {
    let blocks = document.getElementsByClassName("day");
    let lastBlock = blocks[blocks.length - 1];
    lastBlock.remove();
}


// add | delete workout

function addWorkout(id, pattern) {
    let workoutNumber = $('#day' + id + ' div.workouts-block').children().length;
    
    $('#day' + id + ' div.workouts-block').append('<div class="semantic-block"> <div class="form-group"> <label>Название тренировки: <input name="schedule.ScheduleDays[' + (id) + '].ScheduleDayWorkouts[' + workoutNumber + '].WorkoutId" type="text" list="workout-list" placeholder="Выберите Id тренировки из списка" pattern="' + pattern + '" title="Выберите один из доступных вариантов" autocomplete="off" required /> </label> </div> <details> <summary>Больше информации</summary> <div class="form-group"> <label>Место: <input name="schedule.ScheduleDays[' + id + '].ScheduleDayWorkouts[' + workoutNumber + '].Place" type="text" placeholder="Укажите место проведения тренировки" max="100" required /> </label> </div> <div class="form-group"> <label for="qantityOfPlaces">Количество мест:</label> <input name="schedule.ScheduleDays[' + id + '].ScheduleDayWorkouts[' + workoutNumber + '].QuantityOfPlaces" id="qantityOfPlaces" type="number" placeholder="Укажите количество мест" min="0" max="1000000" required /> </div> <div class="flex-controls"> <div class="form-group"> <label>Время начала тренировки: <input name="schedule.ScheduleDays[' + id + '].ScheduleDayWorkouts[' + workoutNumber + '].StartTime" type="time" required> </label> </div> <div class="form-group"> <label>Время завершения: <input name="schedule.ScheduleDays[' + id + '].ScheduleDayWorkouts[' + workoutNumber + '].EndTime" type="time" required> </label> </div> </div> </details> </div>');
}

function deleteWorkout(id) {
    $('#day' + id + ' div.workouts-block').find("div.semantic-block:last").remove();
}
