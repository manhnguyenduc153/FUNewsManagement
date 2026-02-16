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
