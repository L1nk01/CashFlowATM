// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    var input = document.querySelector('#dispensation-mode');

    function loadSettings() {
        if (localStorage['selectedOption']) {
            input.value = localStorage['selectedOption'];
        }
    }

    function saveSettings() {
        localStorage['selectedOption'] = input.value;
    }

    document.addEventListener('DOMContentLoaded', loadSettings);

    input.addEventListener('change', saveSettings);
}