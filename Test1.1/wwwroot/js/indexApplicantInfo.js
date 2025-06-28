/* ========================= REQUEST 2: Modal JS Functionality ========================= */

// Get modal elements
const modal = document.getElementById('editModal');
const editBtn = document.getElementById('editBtn');
const closeBtn = document.querySelector('.close-btn');

// Show modal when edit button is clicked
editBtn.addEventListener('click', function() {
modal.style.display = 'block';
});

// Hide modal when close button is clicked
closeBtn.addEventListener('click', function() {
modal.style.display = 'none';
});

// Hide modal when clicking outside the modal content
window.addEventListener('click', function(event) {
if (event.target === modal) {
    modal.style.display = 'none';
}
});
