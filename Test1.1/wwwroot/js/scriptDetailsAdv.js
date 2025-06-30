function viewApplicant(applicantId) {
    window.location.href = '/Applicant/Details/' + applicantId;
}

document.addEventListener('DOMContentLoaded', function () {
    // Get the anti-forgery token
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // Handle Accept button clicks
    document.querySelectorAll('.accept').forEach(btn => {
        btn.addEventListener('click', async function (e) {
            e.preventDefault();
            const row = this.closest('tr');
            const applicationId = row.dataset.applicationId;

            try {
                const response = await fetch('/JobAdvertisement/UpdateApplicationStatus', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token
                    },
                    body: JSON.stringify({
                        applicationId: parseInt(applicationId),
                        status: "Accepted"
                    })
    });
  }

                if (response.ok) {
                    location.reload(); // Refresh to see changes
    } else {
                    console.error('Failed to update status:', await response.text());
                    alert('Failed to accept applicant. Please try again.');
                }
            } catch (error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
    }
  }).join("");

  if (isA) {
    tbody.querySelectorAll("button.delete").forEach(btn => {
      btn.addEventListener("click", () => {
        const idx = +btn.dataset.index;
        pending.push(accepted.splice(idx, 1)[0]);
        render("table-a");
        render("table-b");
      });
    });
  } else {
    tbody.querySelectorAll("button.accept").forEach(btn => {
      btn.addEventListener("click", () => {
        const idx = +btn.dataset.index;
        accepted.push(pending.splice(idx, 1)[0]);
        render("table-a");
        render("table-b");
      });
    });
    tbody.querySelectorAll("button.reject").forEach(btn => {
      btn.addEventListener("click", () => {
        const idx = +btn.dataset.index;
        pending.splice(idx, 1);
        render("table-b");
      });
    });

    // Handle Reject/Delete button clicks
    document.querySelectorAll('.reject, .delete').forEach(btn => {
        btn.addEventListener('click', async function (e) {
            e.preventDefault();
            const row = this.closest('tr');
            const applicationId = row.dataset.applicationId;
            const isDelete = btn.classList.contains('delete');

            try {
                const response = await fetch('/JobAdvertisement/UpdateApplicationStatus', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken': token
                    },
                    body: JSON.stringify({
                        applicationId: parseInt(applicationId),
                        status: isDelete ? "Pending" : "Rejected"
                    })
                });

                if (response.ok) {
                    location.reload(); // Refresh to see changes
      } else {
                    console.error('Failed to update status:', await response.text());
                    alert('Failed to update applicant status. Please try again.');
                }
            } catch (error) {
                console.error('Error:', error);
                alert('An error occurred. Please try again.');
      }

      table.querySelectorAll("thead th").forEach(h => {
        h.classList.remove("sort-asc", "sort-desc");
      });
      th.classList.add(state.order === "asc" ? "sort-asc" : "sort-desc");

      render(tableId);
    });
  });
}

["table-a", "table-b"].forEach(id => {
  setupSorting(id);
  render(id);
});





