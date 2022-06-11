// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    // This puts the label inside the input text box until the input gains focus. 
    // Then it moves to the top left of the container box.If input box loses focus,
    // then it is checked if it is empty - if so, label moves back.
    $(".form-control").focus(function () {
        $(this).parent().addClass("label-animate");
        $(".form-control").each(function () {
            if ($(this).val() == '' && !$(this).is(":focus")) {
                $(this).parent().removeClass("label-animate");
            }
        });
    });
    $(".form-control").blur(function () {
        if ($(this).val() == '') {
            $(this).parent().removeClass("label-animate");
        }
    });
});