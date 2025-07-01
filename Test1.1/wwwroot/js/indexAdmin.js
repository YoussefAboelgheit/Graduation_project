document.addEventListener("DOMContentLoaded", function () {
    // ----------- Companies ------------
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

    // ----------- Ad Comparisons ------------
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

    // ----------- Subscription Edit Modal ------------
  const modal = document.getElementById("edit-modal");
  const editButtons = document.querySelectorAll(".edit-btn");
  const closeButton = document.querySelector(".close-button");

  const subscriptionNameEl = document.getElementById("subscription-name");
  const oldPriceEl = document.querySelector(".old-price");
  const newPriceInput = document.getElementById("new-price-input");
    const subscriptionIdInput = document.getElementById("subscription-id");

    // ÝÊÍ ÇáãæÏÇá æÊÚÈÆÉ ÇáÈíÇäÇÊ
    function openModal(id, name, price) {
    subscriptionNameEl.innerText = name;
        oldPriceEl.innerText = `$${price}`;
        subscriptionIdInput.value = id;
    newPriceInput.value = "";

    modal.style.display = "block";
  }

    editButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            const name = this.getAttribute("data-subscription-name");
            const price = this.getAttribute("data-old-price");
            openModal(id, name, price);
    });
  });

    closeButton.addEventListener("click", function () {
        modal.style.display = "none";
  });

    window.addEventListener("click", function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
    }
    currentBox.querySelector(".price").innerText = `$${parseFloat(newPrice).toFixed(2)}`;
    closeModal();
  });
});
