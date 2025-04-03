const toggleSidebar = document.getElementById("toggleSidebar");
const sidebar = document.getElementById("sidebar");

toggleSidebar.addEventListener("click", function () {
    sidebar.classList.toggle("minimized");
});

document.addEventListener("DOMContentLoaded", function () {
    const navItems = document.querySelectorAll('.nav-link');

    navItems.forEach(item => {
        if (window.location.href.indexOf(item.href) !== -1) {
            item.classList.add('active');
        }
    });
});

function showItems(value) {
    document.getElementById("showingItems").textContent = value;
}

function updateFilter(filter) {
    document.getElementById("filterText").innerHTML = `Filter by ${filter}`;
}