const initialData = [
  { name: "mohamed zakaria", age: 28, city: "Cairo" },
  { name: "joo",     age: 34, city: "Alexandria" },
  { name: "hamdy",     age: 22, city: "Giza" },
  { name: "amr",   age: 45, city: "Luxor" },
  { name: "safwat",     age: 31, city: "Aswan" }
];






let pending = [...initialData];   
let accepted = [];                

const sortState = {
  "table-a": { field: null, order: "asc" },
  "table-b": { field: null, order: "asc" }
};

function render(tableId) {
  const isA = tableId === "table-a";
  const data = isA ? accepted : pending;
  const { field, order } = sortState[tableId];

  if (field) {
    data.sort((a, b) => {
      if (a[field] < b[field]) return order === "asc" ? -1 : 1;
      if (a[field] > b[field]) return order === "asc" ? 1 : -1;
      return 0;
    });
  }

  const tbody = document.querySelector(`#${tableId} tbody`);
  tbody.innerHTML = data.map((row, i) => {
    if (isA) {
      return `
        <tr>
          <td>${row.name}</td>
          <td>${row.age}</td>
          <td>${row.city}</td>
          <td>
          <button class="delete" data-index="${i}">Delete</button>
          <button class="View" data-index="${i}">View</button>
          </td>
        </tr>`;
    } else {
      return `
        <tr>
          <td>${row.name}</td>
          <td>${row.age}</td>
          <td>${row.city}</td>
          <td>
            <button class="accept" data-index="${i}">Accept</button>
            <button class="reject" data-index="${i}">Reject</button>
            <button class="View" data-index="${i}">View</button>
          </td>
        </tr>`;
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
  }
}

function setupSorting(tableId) {
  const table = document.getElementById(tableId);
  table.querySelectorAll("thead th[data-field]").forEach(th => {
    th.addEventListener("click", () => {
      const field = th.dataset.field;
      const state = sortState[tableId];

      if (state.field === field) {
        state.order = state.order === "asc" ? "desc" : "asc";
      } else {
        state.field = field;
        state.order = "asc";
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





