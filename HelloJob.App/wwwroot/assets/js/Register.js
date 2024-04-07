var buttons = document.querySelectorAll(".button-link");

buttons.forEach(function (button) {
    button.addEventListener("click", function () {
        buttons.forEach(function (btn) {
            btn.classList.remove("active");
        });

        this.classList.add("active");
    });
});

function selectRole(role) {
    document.getElementById("roleInput").value = role; 
    document.getElementById("myForm").submit(); 
}
