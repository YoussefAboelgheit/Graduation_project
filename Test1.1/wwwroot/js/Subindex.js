document.addEventListener("DOMContentLoaded", function () {
    const subscribeButtons = document.querySelectorAll(".subscribe-btn");

    subscribeButtons.forEach(button => {
        button.addEventListener("click", async function () {
            const subId = this.dataset.subid;
            const amount = this.dataset.amount;

            try {
                const response = await fetch("/Payment/CreateCheckoutSession", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({ subId, amount })
                });

                if (!response.ok) {
                    alert("An error occurred.");
                    return;
                }

                const result = await response.json();

                const stripe = Stripe("pk_test_51R07Ve7hobWyTND7gR0RjmGxHSN82uIEiotFcYS4rlcHpREL2dQY3yb86YURXyy1BllNDzVeMlEB6L9o3gbKzNBx00qXUaRvrq");
                stripe.redirectToCheckout({ sessionId: result.id });

            } catch (error) {
                console.error("Stripe Checkout Error:", error);
                alert("Failed to connect to payment system.");
            }
        });
    });
});
