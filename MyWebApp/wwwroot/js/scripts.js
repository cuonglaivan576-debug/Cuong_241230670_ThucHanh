// [BỔ SUNG] Đề dùng js/scripts.js của template; đây là bản tối giản: bật/tắt menu trái.
window.addEventListener('DOMContentLoaded', function () {
    var toggle = document.getElementById('sidebarToggle');
    if (toggle) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
        });
    }
});
