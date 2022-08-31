$('#showEyes').on('click', function () {
    document.getElementById("password").type = "text";
    document.getElementById("spanShow").style.display = 'none';
    document.getElementById("spanHiden").style.display = 'block';
    console.log("0");
});


$('#hideEyes').on('click', function () {
    document.getElementById("password").type = "password";
    document.getElementById("spanShow").style.display = 'block';
    document.getElementById("spanHiden").style.display = 'none';
    console.log("1");
});

function validateform() {

}