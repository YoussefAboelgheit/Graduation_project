document.addEventListener('DOMContentLoaded', function () {

    const companyRejectBtns = document.querySelectorAll('.company-box .reject-btn');
    const companyAcceptBtns = document.querySelectorAll('.company-box .accept-btn');

    companyRejectBtns.forEach((btn) => {
    btn.addEventListener('click', function () {
        alert('Company Rejected');

    });
    });

    companyAcceptBtns.forEach((btn) => {
    btn.addEventListener('click', function () {
        alert('Company Accepted');
    });
    });


    const comparisonRejectBtns = document.querySelectorAll('.comparison-actions .reject-btn');
    const comparisonAcceptBtns = document.querySelectorAll('.comparison-actions .accept-btn');

    comparisonRejectBtns.forEach((btn) => {
    btn.addEventListener('click', function () {
        alert('Ad Comparison Rejected');
    });
    });

    comparisonAcceptBtns.forEach((btn) => {
    btn.addEventListener('click', function () {
        alert('Ad Comparison Accepted');
    });
    });
});
// **********************payment***********************
document.addEventListener("DOMContentLoaded", function() {
  const modal = document.getElementById("edit-modal");
  const editButtons = document.querySelectorAll(".edit-btn");
  const closeButton = document.querySelector(".close-button");
  const subscriptionNameEl = document.getElementById("subscription-name");
  const oldPriceEl = document.querySelector(".old-price");
  const newPriceInput = document.getElementById("new-price-input");
  const confirmButton = document.getElementById("confirm-edit-btn");

  let currentBox = null;

  function openModal(box) {
    currentBox = box;
    const name = box.querySelector("h3").innerText;
    const priceText = box.querySelector(".price").innerText;

    subscriptionNameEl.innerText = name;
    oldPriceEl.innerText = priceText;
    newPriceInput.value = "";

    modal.style.display = "block";
  }

  function closeModal() {
    modal.style.display = "none";
    currentBox = null;
  }

  editButtons.forEach(btn => {
    btn.addEventListener("click", function() {
      const box = this.closest(".subscription-box");
      openModal(box);
    });
  });

  closeButton.addEventListener("click", closeModal);

  window.addEventListener("click", function(e) {
    if (e.target === modal) closeModal();
  });

  confirmButton.addEventListener("click", function() {
    const newPrice = newPriceInput.value.trim();
    if (!newPrice || isNaN(newPrice)) {
      alert("Please enter a valid number.");
      return;
    }
    currentBox.querySelector(".price").innerText = `$${parseFloat(newPrice).toFixed(2)}`;
    closeModal();
  });
});
