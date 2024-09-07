$(document).ready(function () {
    $('.js-example-basic-multiple').select2();
});

var elements = document.querySelectorAll('.category-content__item');

elements.forEach(function (element) {
    var hasImage = element.getElementsByTagName('img').length > 0;
    if (hasImage) {
        element.style.setProperty('height', '100px', 'important');
    } else {
        element.style.setProperty('height', '50px', 'important');
    }
});