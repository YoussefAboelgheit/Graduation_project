function toggleCard() {
    const flipCard = document.getElementById("flip-card");
    const toggleText = document.getElementById("toggle-text");
  
    if (flipCard.classList.contains("flip")) {
      flipCard.classList.remove("flip");
      toggleText.innerText = "Applicant";
    } else {
      flipCard.classList.add("flip");
      toggleText.innerText = "Company";
    }
  }
  