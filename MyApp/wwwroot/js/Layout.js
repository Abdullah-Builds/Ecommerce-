
document.addEventListener('DOMContentLoaded', () => {
    const nav = document.querySelector('.nav');
    const searchIcon = document.querySelector('#searchIcon');
    const navOpenBtn = document.querySelector('.navOpenBtn');
    const navCloseBtn = document.querySelector('.navCloseBtn');
    const searchCloseBtn = document.querySelector('#searchCloseBtn');
    const themeToggle = document.querySelector('#themeToggle');
    const searchBox = document.querySelector('.search-box');
    const searchInput = searchBox ? searchBox.querySelector('input') : null;

    // Helper
    function setAriaExpanded(el, value) { 
        if (el) el.setAttribute('aria-expanded', String(value)); 
    }

    // Open search
    if (searchIcon) {
        searchIcon.addEventListener('click', () => {
            if (nav) {
                nav.classList.add('openSearch');
                nav.classList.remove('openNav');
            }
            searchIcon.style.display = 'none';
            setAriaExpanded(searchBox, true);
            setTimeout(() => { if (searchInput) searchInput.focus(); }, 200);
        });
    }

    // Close search
    if (searchCloseBtn) {
        searchCloseBtn.addEventListener('click', () => {
            if (nav) nav.classList.remove('openSearch');
            if (searchIcon) searchIcon.style.display = 'inline';
            setAriaExpanded(searchBox, false);
            if (searchIcon) searchIcon.focus();
        });
    }

    // Open nav menu (mobile)
    if (navOpenBtn) {
        navOpenBtn.addEventListener('click', () => {
            if (nav) {
                nav.classList.add('openNav');
                nav.classList.remove('openSearch');
            }
            setAriaExpanded(navOpenBtn, true);
            setAriaExpanded(navCloseBtn, true);
            setTimeout(() => { if (navCloseBtn) navCloseBtn.focus(); }, 100);
        });
    }

    // Close nav menu (mobile)
    if (navCloseBtn) {
        navCloseBtn.addEventListener('click', () => {
            if (nav) nav.classList.remove('openNav');
            setAriaExpanded(navOpenBtn, false);
            setAriaExpanded(navCloseBtn, false);
            if (navOpenBtn) navOpenBtn.focus();
        });
    }

    // Dark Mode Toggle
    function setTheme(theme) {
        if (theme === 'dark') {
        document.body.classList.add('dark');
    if (themeToggle) themeToggle.classList.replace('uil-moon', 'uil-sun');
        } else {
        document.body.classList.remove('dark');
    if (themeToggle) themeToggle.classList.replace('uil-sun', 'uil-moon');
        }
    }

    let savedTheme = localStorage.getItem('theme') || 'light';
    setTheme(savedTheme);

    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            let newTheme = document.body.classList.contains('dark') ? 'light' : 'dark';
            setTheme(newTheme);
            localStorage.setItem('theme', newTheme);
        });
    }

    // Dynamic body padding from nav height
    function updateBodyOffset() {
    const navEl = document.querySelector('.nav');
    if (!navEl) return;
    const h = navEl.offsetHeight;
    document.body.style.setProperty('padding-top', h + 'px');
    }

    window.addEventListener('load', updateBodyOffset);
    window.addEventListener('resize', updateBodyOffset);
});

