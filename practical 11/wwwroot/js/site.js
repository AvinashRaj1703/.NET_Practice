// SmartCampusPortal UI Polish Scripts

document.addEventListener("DOMContentLoaded", function () {
    // Highlight active nav item based on current URL path
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll(".navbar-nav .nav-link");

    navLinks.forEach(link => {
        const href = link.getAttribute("href")?.toLowerCase();
        if (href) {
            if (currentPath === href || (href !== "/" && currentPath.startsWith(href))) {
                link.classList.add("active");
            }
        }
    });

    // Auto-dismiss alerts after 4.5 seconds for a cleaner experience
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            try {
                if (typeof bootstrap !== 'undefined' && bootstrap.Alert) {
                    const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                    if (bsAlert) {
                        bsAlert.close();
                    }
                }
            } catch (e) {
                // Ignore if alert was already closed
            }
        }, 4500);
    });
});
