// Loading Indicator
window.showLoading = function() {
    $('#loadingIndicator').css('display', 'flex');
};

window.hideLoading = function() {
    $('#loadingIndicator').hide();
};

// Auto show loading for all AJAX requests
$(document).ajaxStart(function() {
    showLoading();
});

$(document).ajaxStop(function() {
    hideLoading();
});

// Auto show loading for form submissions
$(document).on('submit', 'form', function() {
    showLoading();
});

// Sidebar active state
$(document).ready(function() {
    const currentPath = window.location.pathname;
    $('.sidebar-nav a').each(function() {
        const href = $(this).attr('href');
        if (href && (currentPath === href || currentPath.startsWith(href + '/'))) {
            $(this).addClass('active');
        }
    });
});
