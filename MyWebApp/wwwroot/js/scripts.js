window.addEventListener('DOMContentLoaded', function () {
    var toggle = document.getElementById('sidebarToggle');
    if (toggle) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
        });
    }
});
